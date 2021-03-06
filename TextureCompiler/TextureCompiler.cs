﻿using Engine;
using Engine.Saving;
using FreeImageAPI;
using Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OgrePlugin;
using OgrePlugin.Plugin.VirtualTexture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Anomalous.TextureCompiler
{
    enum Channel
    {
        Red,
        Green,
        Blue,
        Alpha
    }

    [Flags]
    public enum OutputFormats
    {
        None = 0,
        BC3 = 1,
        BC5Normal = 1 << 1,
        ETC2 = 1 << 2,
        Uncompressed = 1 << 3,
        All = BC3 | BC5Normal | ETC2 | Uncompressed
    }

    class TextureCompiler : MaterialBuilder
    {
        private List<String> errors = new List<string>();

        private static String SourceFileFormat = "{0}.{1}";
        private static String TempFileFormat = "{0}_tmp.tga";
        private static FREE_IMAGE_FORMAT TempFileImageFormat = FREE_IMAGE_FORMAT.FIF_TARGA;

        private static String MainExeLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static String NVCompressExe = Path.Combine(MainExeLocation, "CompressorBinaries/Nvidia/nvcompress.exe");
        private static String NVCompressArgFormat = "-nocuda {0}";
        private static String NVCompressBC5Format = String.Format(NVCompressArgFormat, "-bc5 \"{0}\" {1}_bc5.dds\"");
        private static String NVCompressBC3nFormat = String.Format(NVCompressArgFormat, "-bc3n \"{0}\" {1}.dds\"");
        private static String NVCompressBC3Format = String.Format(NVCompressArgFormat, "-bc3 \"{0}\" \"{1}.dds\"");

        private static String MaliTextureToolExe = Path.Combine(MainExeLocation, "CompressorBinaries/MaliTextureTool/etcpack.exe");
        private static String MaliTextureToolArgFormat = "\"{0}\" \"{1}\" -c etc2 -f RGBA -ktx -mipmaps -ext TGA"; //-s slow

        private static String PagedTextureNameFormat = String.Format("{{0}}{0}", PagedImage.FileExtension);

        private String sourceDirectory;
        private String destDirectory;

        private HashSet<String> compiledTextures = new HashSet<string>();
        private CompiledTextureInfo compiledTextureInfo = new CompiledTextureInfo();

        private int compressedCount = 0;
        private OutputFormats outputFormats;
        private int maxSize = int.MaxValue;

        public TextureCompiler(String sourceDirectory, String destDirectory, OutputFormats outputFormats, int maxSize)
        {
            this.outputFormats = outputFormats;
            this.sourceDirectory = sourceDirectory;
            this.destDirectory = destDirectory;
            this.maxSize = maxSize;
        }

        public void loadTextureInfo()
        {
            String hashFile = Path.Combine(sourceDirectory, TextureCompilerInterface.TextureHashFileName);
            if (File.Exists(hashFile))
            {
                Saver saver = new Saver();
                using (var stream = File.Open(hashFile, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    compiledTextureInfo = saver.restoreObject<CompiledTextureInfo>(stream);
                }
            }
        }

        public void saveTextureInfo()
        {
            Saver saver = new Saver();
            using (var stream = File.Open(Path.Combine(sourceDirectory, TextureCompilerInterface.TextureHashFileName), FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                saver.saveObject(compiledTextureInfo, stream);
            }
        }

        private void writeSimpleDiffuseSprite(MaterialDescription description, MaterialRepository repo)
        {
            String diffuseSrc = getSourceFullPath(description.DiffuseMapName);
            if (!imageNeedsCompression(diffuseSrc))
            {
                return;
            }

            String diffuseDest = getDestBasePath(description.DiffuseMapName);

            String normalDest = getDestBasePath(description.NormalMapName);
            String normalTmp = getTempPath(description.NormalMapName);

            using (FreeImageBitmap diffuseMap = FreeImageBitmap.FromFile(diffuseSrc))
            {
                using (FreeImageBitmap normalMap = new FreeImageBitmap(diffuseMap.Width, diffuseMap.Height, FreeImageAPI.PixelFormat.Format32bppArgb))
                {
                    normalMap.FillBackground(new RGBQUAD(new FreeImageAPI.Color()
                    {
                        R = 0x80,
                        G = 0x80,
                        B = 0,
                        A = 255
                    }));
                    using (FreeImageBitmap combined = createImageFromChannels(normalMap, Channel.Red, normalMap, Channel.Green, diffuseMap, Channel.Alpha))
                    {
                        saveImage(combined, normalTmp, TempFileImageFormat);
                        compressCompositeNormalMap(diffuseSrc, normalTmp, normalDest, description);
                        deleteFile(normalTmp);
                    }
                }
            }

            Log.Info("Compressing diffuse map {0} directly", description.DiffuseMapName);
            compressDiffuseMap(diffuseSrc, diffuseDest, description);
        }

        public override void buildMaterial(MaterialDescription description, MaterialRepository repo)
        {
            if (description.SimpleDiffuseSprite)
            {
                writeSimpleDiffuseSprite(description, repo);
                return;
            }

            // --------------------
            // Normal Load Process
            // --------------------

            bool saveNormal = shouldSave(description.HasNormalMap, description.NormalMapName);
            bool saveDiffuse = shouldSave(description.HasDiffuseMap, description.DiffuseMapName);
            bool saveSpecular = shouldSave(description.HasSpecularColorMap, description.SpecularMapName);
            bool saveOpacity = shouldSave(description.HasOpacityMap, description.OpacityMapName);

            String normalSrc = getSourceFullPath(description.NormalMapName);
            String diffuseSrc = getSourceFullPath(description.DiffuseMapName);
            String specularSrc = getSourceFullPath(description.SpecularMapName);
            String specularLevelSrc = getSourceFullPath(description.SpecularLevelMapName);
            String glossLevelSrc = getSourceFullPath(description.GlossMapName);
            String opacitySrc = getSourceFullPath(description.OpacityMapName);

            bool compressNormal = description.HasNormalMap && imageNeedsCompression(normalSrc);
            bool compressDiffuse = description.HasDiffuseMap && imageNeedsCompression(diffuseSrc);
            bool compressSpecular = description.HasSpecularColorMap && imageNeedsCompression(specularSrc);
            bool compressSpecularLevel = description.HasSpecularLevelMap && imageNeedsCompression(specularLevelSrc);
            bool compressGlossLevel = description.HasGlossMap && imageNeedsCompression(glossLevelSrc);
            bool compressOpacity = description.HasOpacityMap && imageNeedsCompression(opacitySrc);

            Log.Info("Compiling textures for material {0}", description.Name);
            if (saveNormal)
            {
                String normalDest = getDestBasePath(description.NormalMapName);
                String normalTmp = getTempPath(description.NormalMapName);
                if (compressNormal)
                {
                    Log.Info("Compressing normal map {0}", description.NormalMapName);
                    compressNormalMap(normalSrc, normalDest);
                }
                if (CreateCompositeNormal && (compressNormal || compressGlossLevel || compressOpacity)) //Composite normal maps allow us to skip the 4th texture all together and use the remaining b and alpha channels of this texture for our additional data
                {
                    Log.Info("Compressing composite normal map {0}", description.NormalMapName);
                    if (description.HasOpacityMap && description.HasGlossMap)
                    {
                        addMapToBlueAndAlphaAndCompress(normalSrc, opacitySrc, Channel.Red, glossLevelSrc, Channel.Red, normalTmp, normalDest, description, (src, dest, matDesc) => compressCompositeNormalMap(normalSrc, src, dest, matDesc));
                    }
                    else if (description.HasOpacityMap)
                    {
                        addMapToBlueAndCompress(normalSrc, opacitySrc, Channel.Red, normalTmp, normalDest, description, (src, dest, matDesc) => compressCompositeNormalMap(normalSrc, src, dest, matDesc));
                    }
                    else if (description.HasGlossMap)
                    {
                        //Gloss maps always go on the normal map green
                        addMapToAlphaAndCompress(normalSrc, glossLevelSrc, Channel.Red, normalTmp, normalDest, description, (src, dest, matDesc) => compressCompositeNormalMap(normalSrc, src, dest, matDesc));
                    }
                    else
                    {
                        compressCompositeNormalMap(normalSrc, normalSrc, normalDest, description);
                    }
                }
            }
            if (saveDiffuse)
            {
                String diffuseDest = getDestBasePath(description.DiffuseMapName);
                String diffuseTmp = getTempPath(description.DiffuseMapName);

                if (description.HasSpecularLevelMap && !description.HasSpecularColorMap) //If we don't have a specular color map pack the specular level into the diffuse map
                {
                    if (compressSpecularLevel || compressDiffuse)
                    {
                        Log.Info("Compressing diffuse map {0} with specular level in alpha from {1}", description.DiffuseMapName, description.SpecularLevelMapName);
                        addMapToAlphaAndCompress(diffuseSrc, specularLevelSrc, Channel.Red, diffuseTmp, diffuseDest, description, compressDiffuseMap);
                    }
                }
                else if (description.HasGlossMap && !description.HasOpacityMap) //Pack the gloss level into the diffuse map
                {
                    if (compressGlossLevel || compressDiffuse)
                    {
                        Log.Info("Compressing diffuse map {0} with gloss in alpha from {1}", description.DiffuseMapName, description.GlossMapName);
                        addMapToAlphaAndCompress(diffuseSrc, glossLevelSrc, Channel.Red, diffuseTmp, diffuseDest, description, compressDiffuseMap);
                    }
                }
                else //Just save the diffuse map as is
                {
                    if (compressDiffuse)
                    {
                        Log.Info("Compressing diffuse map {0} directly", description.DiffuseMapName);
                        compressDiffuseMap(diffuseSrc, diffuseDest, description);
                    }
                }
            }
            if (saveSpecular)
            {
                String specularDest = getDestBasePath(description.SpecularMapName);
                String specularTmp = getTempPath(description.SpecularMapName);

                if (description.HasSpecularLevelMap)
                {
                    if (compressSpecularLevel || compressSpecular)
                    {
                        Log.Info("Compressing specular map {0} with specular level in alpha {1}", description.DiffuseMapName, description.SpecularLevelMapName);
                        addMapToAlphaAndCompress(specularSrc, specularLevelSrc, Channel.Red, specularTmp, specularDest, description, compressSpecularMap);
                    }
                }
                else //Just save as is
                {
                    if (compressSpecular)
                    {
                        Log.Info("Compressing specular map {0} directly", description.DiffuseMapName);
                        compressSpecularMap(specularSrc, specularDest, description);
                    }
                }
            }
            if (saveOpacity)
            {
                String opacityDest = getDestBasePath(description.OpacityMapName);
                String opacityTmp = getTempPath(description.OpacityMapName);

                if (description.HasGlossMap)
                {
                    if (compressGlossLevel || compressOpacity)
                    {
                        Log.Info("Compressing opacity map {0} with gloss in green from {1}", description.DiffuseMapName, description.GlossMapName);
                        combineSingleChannelMaps(opacitySrc, Channel.Red, glossLevelSrc, Channel.Red, opacityTmp, opacityDest, description, compressOpacityMap);
                    }
                }
                else //Just save as is
                {
                    if (compressOpacity)
                    {
                        Log.Info("Compressing opacity map {0} directly", description.DiffuseMapName);
                        compressOpacityMap(opacitySrc, opacityDest, description);
                    }
                }
            }
        }

        private void addMapToAlphaAndCompress(String rgbSource, String alphaSource, Channel alphaSourceChannel, String tempFile, String destinationFile, MaterialDescription matDesc, Action<String, String, MaterialDescription> compressFunction)
        {
            Log.Info("Building composite image with RGB {0} and alpha {1}", rgbSource, alphaSource);
            using (FreeImageBitmap rgbMap = FreeImageBitmap.FromFile(rgbSource))
            {
                using (FreeImageBitmap alphaMap = FreeImageBitmap.FromFile(alphaSource))
                {
                    using (FreeImageBitmap combined = createImageFromChannels(alphaMap, alphaSourceChannel, rgbMap))
                    {
                        saveImage(combined, tempFile, TempFileImageFormat);
                        compressFunction(tempFile, destinationFile, matDesc);
                        deleteFile(tempFile);
                    }
                }
            }
        }

        private void addMapToBlueAndCompress(String rgSource, String bSource, Channel bSourceChannel, String tempFile, String destinationFile, MaterialDescription matDesc, Action<String, String, MaterialDescription> compressFunction)
        {
            Log.Info("Building composite image with RG {0} and B {1}", rgSource, bSource);
            using (FreeImageBitmap rgMap = FreeImageBitmap.FromFile(rgSource))
            {
                using (FreeImageBitmap bMap = FreeImageBitmap.FromFile(bSource))
                {
                    using (FreeImageBitmap combined = createImageFromChannels(rgMap, Channel.Red, rgMap, Channel.Green, bMap, bSourceChannel))
                    {
                        saveImage(combined, tempFile, TempFileImageFormat);
                        compressFunction(tempFile, destinationFile, matDesc);
                        deleteFile(tempFile);
                    }
                }
            }
        }

        private void addMapToBlueAndAlphaAndCompress(String rgSource, String bSource, Channel bSourceChannel, String alphaSource, Channel alphaSourceChannel, String tempFile, String destinationFile, MaterialDescription matDesc, Action<String, String, MaterialDescription> compressFunction)
        {
            Log.Info("Building composite image with RG {0} and B {1}", rgSource, bSource);
            using (FreeImageBitmap rgMap = FreeImageBitmap.FromFile(rgSource))
            {
                using (FreeImageBitmap bMap = FreeImageBitmap.FromFile(bSource))
                {
                    using (FreeImageBitmap alphaMap = FreeImageBitmap.FromFile(alphaSource))
                    {
                        using (FreeImageBitmap combined = createImageFromChannels(alphaMap, alphaSourceChannel, rgMap, Channel.Red, rgMap, Channel.Green, bMap, bSourceChannel))
                        {
                            saveImage(combined, tempFile, TempFileImageFormat);
                            compressFunction(tempFile, destinationFile, matDesc);
                            deleteFile(tempFile);
                        }
                    }
                }
            }
        }

        private void combineSingleChannelMaps(String redSource, Channel redSourceChannel, String greenSource, Channel greenSourceChannel, String tempFile, String destinationFile, MaterialDescription matDesc, Action<String, String, MaterialDescription> compressFunction)
        {
            Log.Info("Building composite image with red source {0} and green source {1}", redSource, greenSource);
            using (FreeImageBitmap redMap = FreeImageBitmap.FromFile(redSource))
            {
                using (FreeImageBitmap greenMap = FreeImageBitmap.FromFile(greenSource))
                {
                    using (FreeImageBitmap combined = createImageFromChannels(redMap, redSourceChannel, greenMap, greenSourceChannel))
                    {
                        saveImage(combined, tempFile, TempFileImageFormat);
                        compressFunction(tempFile, destinationFile, matDesc);
                        deleteFile(tempFile);
                    }
                }
            }
        }

        private bool shouldSave(bool hasMap, String mapName)
        {
            if (hasMap && !compiledTextures.Contains(mapName))
            {
                compiledTextures.Add(mapName);
                return true;
            }
            return false;
        }

        private void saveImage(FreeImageBitmap source, String destFile, FREE_IMAGE_FORMAT format)
        {
            using (Stream outStream = File.Open(destFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                source.Save(outStream, format);
            }
            Log.Info("Wrote {0}", destFile);
        }

        private void deleteFile(String path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private FreeImageBitmap createImageFromChannels(FreeImageBitmap alphaSrc, Channel alphaSrcChannel, FreeImageBitmap colorSrc)
        {
            return createImageFromChannels(alphaSrc, alphaSrcChannel, colorSrc, Channel.Red, colorSrc, Channel.Green, colorSrc, Channel.Blue);
        }

        private unsafe FreeImageBitmap createImageFromChannels(FreeImageBitmap alphaSrc, Channel alphaSrcChannel, FreeImageBitmap redSrc, Channel redSrcChannel, FreeImageBitmap greenSrc, Channel greenSrcChannel, FreeImageBitmap blueSrc, Channel blueSrcChannel)
        {
            if (alphaSrc.Width != redSrc.Width || redSrc.Width != greenSrc.Width || greenSrc.Width != blueSrc.Width)
            {
                //Do an error
            }

            int width = redSrc.Width;
            int height = redSrc.Height;

            FreeImageBitmap retVal = new FreeImageBitmap(width, height, FreeImageAPI.PixelFormat.Format32bppArgb);
            return combineImages(alphaSrc, alphaSrcChannel, redSrc, redSrcChannel, greenSrc, greenSrcChannel, blueSrc, blueSrcChannel, retVal);
        }

        private static unsafe FreeImageBitmap combineImages(FreeImageBitmap alphaSrc, Channel alphaSrcChannel, FreeImageBitmap redSrc, Channel redSrcChannel, FreeImageBitmap greenSrc, Channel greenSrcChannel, FreeImageBitmap blueSrc, Channel blueSrcChannel, FreeImageBitmap combined)
        {
            int width = combined.Width;
            int height = combined.Height;

            int alphaWalk = alphaSrc.ColorDepth / 8;
            int redWalk = redSrc.ColorDepth / 8;
            int greenWalk = greenSrc.ColorDepth / 8;
            int blueWalk = blueSrc.ColorDepth / 8;
            int combinedWalk = combined.ColorDepth / 8;

            int alphaChannel = getIndexForChannel(alphaSrcChannel);
            int redChannel = getIndexForChannel(redSrcChannel);
            int greenChannel = getIndexForChannel(greenSrcChannel);
            int blueChannel = getIndexForChannel(blueSrcChannel);

            for (int y = 0; y < height; ++y)
            {
                var alphaScanline = (byte*)alphaSrc.GetScanlinePointer(y).ToPointer();
                var redScanline = (byte*)redSrc.GetScanlinePointer(y).ToPointer();
                var greenScanline = (byte*)greenSrc.GetScanlinePointer(y).ToPointer();
                var blueScanline = (byte*)blueSrc.GetScanlinePointer(y).ToPointer();
                var combinedScanline = (byte*)combined.GetScanlinePointer(y).ToPointer();

                for (int x = 0; x < width; ++x)
                {
                    combinedScanline[FreeImage.FI_RGBA_BLUE] = blueScanline[blueChannel];
                    combinedScanline[FreeImage.FI_RGBA_GREEN] = greenScanline[greenChannel];
                    combinedScanline[FreeImage.FI_RGBA_RED] = redScanline[redChannel];
                    combinedScanline[FreeImage.FI_RGBA_ALPHA] = alphaScanline[alphaChannel];

                    alphaScanline += alphaWalk;
                    redScanline += redWalk;
                    greenScanline += greenWalk;
                    blueScanline += blueWalk;
                    combinedScanline += combinedWalk;
                }
            }

            return combined;
        }

        private static int getIndexForChannel(Channel channel)
        {
            switch (channel)
            {
                case Channel.Alpha:
                    return FreeImage.FI_RGBA_ALPHA;
                case Channel.Red:
                    return FreeImage.FI_RGBA_RED;
                case Channel.Green:
                    return FreeImage.FI_RGBA_GREEN;
                case Channel.Blue:
                    return FreeImage.FI_RGBA_BLUE;
            }
            throw new NotSupportedException(); //Won't get here
        }

        private unsafe FreeImageBitmap createImageFromChannels(FreeImageBitmap redSrc, Channel redSrcChannel, FreeImageBitmap greenSrc, Channel greenSrcChannel, FreeImageBitmap blueSrc, Channel blueSrcChannel)
        {
            if (redSrc.Width != greenSrc.Width || greenSrc.Width != blueSrc.Width)
            {
                //Do an error
            }

            int width = redSrc.Width;
            int height = redSrc.Height;

            FreeImageBitmap retVal = new FreeImageBitmap(width, height, FreeImageAPI.PixelFormat.Format24bppRgb);

            int redWalk = redSrc.ColorDepth / 8;
            int greenWalk = greenSrc.ColorDepth / 8;
            int blueWalk = blueSrc.ColorDepth / 8;
            int retWalk = retVal.ColorDepth / 8;

            int redChannel = getIndexForChannel(redSrcChannel);
            int greenChannel = getIndexForChannel(greenSrcChannel);
            int blueChannel = getIndexForChannel(blueSrcChannel);

            for (int y = 0; y < height; ++y)
            {
                var redScanline = (byte*)redSrc.GetScanlinePointer(y).ToPointer();
                var greenScanline = (byte*)greenSrc.GetScanlinePointer(y).ToPointer();
                var blueScanline = (byte*)blueSrc.GetScanlinePointer(y).ToPointer();
                var retScanline = (byte*)retVal.GetScanlinePointer(y).ToPointer();

                for (int x = 0; x < width; ++x)
                {
                    retScanline[FreeImage.FI_RGBA_BLUE] = blueScanline[blueChannel];
                    retScanline[FreeImage.FI_RGBA_GREEN] = greenScanline[greenChannel];
                    retScanline[FreeImage.FI_RGBA_RED] = redScanline[redChannel];

                    redScanline += redWalk;
                    greenScanline += greenWalk;
                    blueScanline += blueWalk;
                    retScanline += retWalk;
                }
            }

            return retVal;
        }

        private unsafe FreeImageBitmap createImageFromChannels(FreeImageBitmap redSrc, Channel redSrcChannel, FreeImageBitmap greenSrc, Channel greenSrcChannel)
        {
            if (redSrc.Width != greenSrc.Width)
            {
                //Do an error
            }

            int width = redSrc.Width;
            int height = redSrc.Height;
            var c = new FreeImageAPI.Color();

            FreeImageBitmap retVal = new FreeImageBitmap(width, height, FreeImageAPI.PixelFormat.Format24bppRgb);

            int redWalk = redSrc.ColorDepth / 8;
            int greenWalk = greenSrc.ColorDepth / 8;
            int retWalk = retVal.ColorDepth / 8;

            int redChannel = getIndexForChannel(redSrcChannel);
            int greenChannel = getIndexForChannel(greenSrcChannel);

            for (int y = 0; y < height; ++y)
            {
                var redScanline = (byte*)redSrc.GetScanlinePointer(y).ToPointer();
                var greenScanline = (byte*)greenSrc.GetScanlinePointer(y).ToPointer();
                var retScanline = (byte*)retVal.GetScanlinePointer(y).ToPointer();

                for (int x = 0; x < width; ++x)
                {
                    retScanline[FreeImage.FI_RGBA_GREEN] = greenScanline[greenChannel];
                    retScanline[FreeImage.FI_RGBA_RED] = redScanline[redChannel];

                    redScanline += redWalk;
                    greenScanline += greenWalk;
                    retScanline += retWalk;
                }
            }

            return retVal;
        }

        private unsafe byte getColor(int x, int y, FreeImageBitmap src, Channel channel)
        {
            var c = src.GetPixel(x, y);
            switch (channel)
            {
                case Channel.Alpha:
                    return c.A;
                case Channel.Red:
                    return c.R;
                case Channel.Green:
                    return c.G;
                case Channel.Blue:
                    return c.B;
            }

            //uint* pixel = (uint*)src.Scan0.ToPointer();
            //pixel += src.Stride * y + x;

            //int bpp = 4;
            //if(src.PixelFormat == FreeImageAPI.PixelFormat.Format24bppRgb)
            //{
            //    bpp = 3;
            //}

            //byte* pixel = (byte*)src.Bits.ToPointer();
            //pixel += (src.Pitch * (src.Height - y - 1) + x) * bpp;
            //switch (channel)
            //{
            //    case Channel.Alpha:
            //        return pixel[3];
            //    case Channel.Red:
            //        return pixel[2];
            //    case Channel.Green:
            //        return pixel[1];
            //    case Channel.Blue:
            //        return pixel[0];
            //}
            throw new NotSupportedException(); //Won't get here
        }

        //private unsafe byte getColor(int x, int y, FreeImageBitmap src, Channel channel)
        //{
        //    int bpp = 4;
        //    if (src.PixelFormat == FreeImageAPI.PixelFormat.Format24bppRgb)
        //    {
        //        bpp = 3;
        //    }

        //    byte* pixel = (byte*)src.Bits.ToPointer();
        //    pixel += (src.Pitch * (src.Height - y - 1) + x) * bpp;
        //    switch (channel)
        //    {
        //        case Channel.Alpha:
        //            return pixel[3];
        //        case Channel.Red:
        //            return pixel[2];
        //        case Channel.Green:
        //            return pixel[1];
        //        case Channel.Blue:
        //            return pixel[0];
        //    }
        //    throw new NotSupportedException(); //Won't get here
        //}

        private async Task saveUncompressed(String sourceFile, String destFile, bool lossless, FREE_IMAGE_FILTER filter, ImagePageSizeStrategy pageSizeStrategy, Action<FreeImageBitmap> afterResize = null)
        {
            await Task.Run(() =>
            {
                if ((outputFormats & OutputFormats.Uncompressed) != 0)
                {
                    Log.Info("Creating paged data for {0}", sourceFile);
                    using (FreeImageBitmap source = FreeImageBitmap.FromFile(sourceFile))
                    {
                        using (var stream = File.Open(String.Format(PagedTextureNameFormat, destFile), FileMode.Create, FileAccess.ReadWrite))
                        {
                            PagedImage.fromBitmap(source, 128, 1, stream, PagedImage.ImageType.WEBP, maxSize, lossless, filter, pageSizeStrategy, afterResize);
                        }
                    }
                }
            });
        }

        private async Task etc2Compress(String sourceFile, String destFile)
        {
            await Task.Run(() =>
            {
                if ((outputFormats & OutputFormats.ETC2) != 0)
                {
                    String cleanedSrc = sourceDirectory;
                    if (cleanedSrc.EndsWith("/") || cleanedSrc.EndsWith("\\"))
                    {
                        cleanedSrc = cleanedSrc.Substring(0, cleanedSrc.Length - 1);
                    }
                    runExternalCompressionProcess(MaliTextureToolExe, String.Format(MaliTextureToolArgFormat, sourceFile, cleanedSrc));
                    String fileName = Path.GetFileNameWithoutExtension(sourceFile);
                    String renameSrc = Path.Combine(sourceDirectory, fileName + ".ktx");
                    String renameDst = destFile + "_etc2.ktx";
                    try
                    {
                        if (File.Exists(renameDst))
                        {
                            File.Delete(renameDst);
                        }
                    }
                    catch (Exception ex)
                    {
                        logExceptionError(ex, String.Format("Deleting {0}", renameDst));
                    }
                    try
                    {
                        File.Move(renameSrc, renameDst);
                    }
                    catch (Exception ex)
                    {
                        logExceptionError(ex, String.Format("Moving {0} to {1}", renameSrc, renameDst));
                    }
                }
            });
        }

        private async Task bc5Compress(String source, String dest)
        {
            await Task.Run(() =>
            {
                if ((outputFormats & OutputFormats.BC5Normal) != 0)
                {
                    runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC5Format, source, dest));
                }
            });
        }

        private async Task bc3nCompress(String source, String dest)
        {
            await Task.Run(() =>
            {
                if ((outputFormats & OutputFormats.BC3) != 0)
                {
                    runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC3nFormat, source, dest));
                }
            });
        }

        private async Task bc3Compress(String source, String dest)
        {
            await Task.Run(() =>
            {
                if ((outputFormats & OutputFormats.BC3) != 0)
                {
                    runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC3Format, source, dest));
                }
            });
        }

        private void runExternalCompressionProcess(String executable, String args)
        {
            using (Process process = new Process()
            {
                StartInfo = new ProcessStartInfo(executable, args)
                {
                    WorkingDirectory = Path.GetDirectoryName(executable),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                },
            })
            {
                Log.Info(String.Format("{0} {1}", process.StartInfo.FileName, process.StartInfo.Arguments));

                process.OutputDataReceived += Process_OutputDataReceived;
                process.ErrorDataReceived += Process_OutputDataReceived;

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();
                process.Close();
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Log.Info(e.Data);
        }

        public override void destroyMaterial(MaterialPtr materialPtr)
        {

        }

        public override void initializationComplete()
        {
            //Print errors
            if (errors.Count > 0)
            {
                Log.Error("{0} Textures compiled with {1} errors.", compressedCount, errors.Count);
                foreach (var error in errors)
                {
                    Log.Error(error);
                }
            }
            else
            {
                Log.ImportantInfo("{0} Textures compiled with no errors.", compressedCount);
            }
        }

        public override string Name
        {
            get
            {
                return "VirtualTexture";
            }
        }

        public bool CreateCompositeNormal
        {
            get
            {
                return (outputFormats & OutputFormats.Uncompressed) != 0;
            }
        }

        private void logExceptionError(Exception ex, String additionalMessage)
        {
            logError(String.Format("{0} when {1}. Reason: {2}", ex.GetType().Name, additionalMessage, ex.Message));
        }

        private void logError(String error)
        {
            Log.Error(error);
            errors.Add(error);
        }

        private String getTempPath(String fileName)
        {
            return String.Format(TempFileFormat, Path.Combine(sourceDirectory, fileName));
        }

        /// <summary>
        /// Get the full path to the source file including extension.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private String getSourceFullPath(String fileName)
        {
            var file = String.Format(SourceFileFormat, Path.Combine(sourceDirectory, fileName), "tga");
            if (File.Exists(file))
            {
                return file;
            }
            return String.Format(SourceFileFormat, Path.Combine(sourceDirectory, fileName), "png");
        }

        private String getDestBasePath(String filename)
        {
            return Path.Combine(destDirectory, filename);
        }

        private void compressOpacityMap(string source, string dest, MaterialDescription matDesc)
        {
            ++compressedCount;
            Task.WaitAll(
                bc3Compress(source, dest),
                etc2Compress(source, dest)
                );
            //The data that goes in opacity maps for uncompressed textures goes in the normal map instead
        }

        private void compressSpecularMap(string source, string dest, MaterialDescription matDesc)
        {
            ++compressedCount;
            Task.WaitAll(
                bc3Compress(source, dest),
                etc2Compress(source, dest),
                saveUncompressed(source, dest, false, FREE_IMAGE_FILTER.FILTER_LANCZOS3, new FullImageSizeStrategy()))
            ;
        }

        private void compressDiffuseMap(string source, string dest, MaterialDescription materialDesc)
        {
            if (!String.IsNullOrEmpty(materialDesc.TilesetReferenceFile) 
                && VirtualFileSystem.Instance.exists(materialDesc.TilesetReferenceFile))
            {
                Dictionary<String, TiledImageSizeStrategy.Tile> tiles;
                using(var reader = new StreamReader(VirtualFileSystem.Instance.openStream(materialDesc.TilesetReferenceFile, Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read)))
                {
                    var json = reader.ReadToEnd();
                    var jobject = JObject.Parse(json);
                    tiles = jobject["tiles"].ToObject<Dictionary<String, TiledImageSizeStrategy.Tile>>();
                }

                using (var sizeStrategy = new TiledImageSizeStrategy(tiles.Values.ToList()))
                {
                    ++compressedCount;
                    Task.WaitAll(
                        bc3Compress(source, dest),
                        etc2Compress(source, dest),
                        saveUncompressed(source, dest, true, FREE_IMAGE_FILTER.FILTER_LANCZOS3, sizeStrategy)
                    );
                }
            }
            else
            {
                ++compressedCount;
                Task.WaitAll(
                    bc3Compress(source, dest),
                    etc2Compress(source, dest),
                    saveUncompressed(source, dest, true, FREE_IMAGE_FILTER.FILTER_LANCZOS3, new FullImageSizeStrategy())
                );
            }
        }

        private void compressNormalMap(string source, string dest)
        {
            ++compressedCount;
            Task.WaitAll(
                bc5Compress(source, dest),
                bc3nCompress(source, dest),
                etc2Compress(source, dest)
            );
        }

        private void compressCompositeNormalMap(String originalNormalMapSource, String source, String dest, MaterialDescription matDesc)
        {
            Task.WaitAll(
                saveUncompressed(source, dest, true, FREE_IMAGE_FILTER.FILTER_BILINEAR, new FullImageSizeStrategy(), (resized) =>
                {
                    String fileName = String.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(originalNormalMapSource), resized.Width, Path.GetExtension(originalNormalMapSource));
                    fileName = Path.Combine(Path.GetDirectoryName(originalNormalMapSource), fileName);
                    if (File.Exists(fileName))
                    {
                        Log.Info("Using manually supplied resized normal map for {0} size {1}", dest, resized.Width);
                        using (var image = FreeImageBitmap.FromFile(fileName))
                        {
                            if (image.Width != resized.Width || image.Height != resized.Height)
                            {
                                throw new Exception(String.Format("Image {0} does not match expected size {1}x{1}. Please fix source image.", fileName, resized.Width));
                            }
                            combineImages(resized, Channel.Alpha, image, Channel.Red, image, Channel.Green, resized, Channel.Blue, resized);
                        }
                    }
                    else
                    {
                        Log.Info("Using automatic resized normal map for {0} size {1}", dest, resized.Width);
                    }
                })
            );
        }

        private bool imageNeedsCompression(String source)
        {
            if (File.Exists(source))
            {
                return compiledTextureInfo.isChanged(Path.GetFileName(source), source);
            }
            else
            {
                logError(String.Format("File {0} does not exist", source));
                return false;
            }
        }
    }
}

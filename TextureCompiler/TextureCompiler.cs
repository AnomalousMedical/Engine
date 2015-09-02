﻿using Engine.Saving.XMLSaver;
using FreeImageAPI;
using Logging;
using OgrePlugin;
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

        private static String SourceFileFormat = "{0}.tga";
        private static String TempFileFormat = "{0}_tmp.tga";
        private static FREE_IMAGE_FORMAT TempFileImageFormat = FREE_IMAGE_FORMAT.FIF_TARGA;

        private static String MainExeLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static String NVCompressExe = Path.Combine(MainExeLocation, "CompressorBinaries/Nvidia/nvcompress.exe");
        private static String NVCompressArgFormat = "-nocuda {0}";
        private static String NVCompressBC5Format = String.Format(NVCompressArgFormat, "-bc5 {0} {1}_bc5.dds");
        private static String NVCompressBC3nFormat = String.Format(NVCompressArgFormat, "-bc3n {0} {1}.dds");
        private static String NVCompressBC3Format = String.Format(NVCompressArgFormat, "-bc3 {0} {1}.dds");

        private static String MaliTextureToolExe = Path.Combine(MainExeLocation, "CompressorBinaries/MaliTextureTool/etcpack.exe");
        private static String MaliTextureToolArgFormat = "{0} {1} -c etc2 -f RGBA -ktx -mipmaps -ext TGA"; //-s slow

        private String sourceDirectory;
        private String destDirectory;

        private HashSet<String> compiledTextures = new HashSet<string>();
        private CompiledTextureInfo compiledTextureInfo = new CompiledTextureInfo();

        private int compressedCount = 0;
        private OutputFormats outputFormats;

        public TextureCompiler(String sourceDirectory, String destDirectory, OutputFormats outputFormats)
        {
            this.outputFormats = outputFormats;
            this.sourceDirectory = sourceDirectory;
            this.destDirectory = destDirectory;
        }

        public void loadTextureInfo()
        {
            String hashFile = Path.Combine(sourceDirectory, TextureCompilerInterface.TextureHashFileName);
            if (File.Exists(hashFile))
            {
                XmlSaver xmlSaver = new XmlSaver();
                using (XmlTextReader xmlReader = new XmlTextReader(hashFile))
                {
                    compiledTextureInfo = (CompiledTextureInfo)xmlSaver.restoreObject(xmlReader);
                }
            }
        }

        public void saveTextureInfo()
        {
            XmlSaver xmlSaver = new XmlSaver();
            using (XmlTextWriter xmlReader = new XmlTextWriter(Path.Combine(sourceDirectory, TextureCompilerInterface.TextureHashFileName), Encoding.Default))
            {
                xmlReader.Formatting = Formatting.Indented;
                xmlSaver.saveObject(compiledTextureInfo, xmlReader);
            }
        }

        public override void buildMaterial(MaterialDescription description, MaterialRepository repo)
        {
            Log.Info("Compiling textures for material {0}", description.Name);
            if (shouldSave(description.HasNormalMap, description.NormalMapName))
            {
                String source = getSourceFullPath(description.NormalMapName);

                if (imageNeedsCompression(source))
                {
                    Log.Info("Compressing normal map {0}", description.NormalMapName);

                    String dest = getDestBasePath(description.NormalMapName);
                    compressNormalMap(source, dest);
                }
            }
            if (shouldSave(description.HasDiffuseMap, description.DiffuseMapName))
            {
                String diffuseSrc = getSourceFullPath(description.DiffuseMapName);
                String diffuseDest = getDestBasePath(description.DiffuseMapName);
                String diffuseTmp = getTempPath(description.DiffuseMapName);
                bool compressDiffuse = imageNeedsCompression(diffuseSrc);

                if (description.HasSpecularLevelMap && !description.HasSpecularColorMap) //If we don't have a specular color map pack the specular level into the diffuse map
                {
                    String specularLevelSrc = getSourceFullPath(description.SpecularLevelMapName);
                    if (imageNeedsCompression(specularLevelSrc) || compressDiffuse)
                    {
                        Log.Info("Compressing diffuse map {0} with specular level in alpha from {1}", description.DiffuseMapName, description.SpecularLevelMapName);
                        addMapToAlphaAndCompress(diffuseSrc, specularLevelSrc, Channel.Red, diffuseTmp, diffuseDest, compressDiffuseMap);
                    }
                }
                else if (description.HasGlossMap && !description.HasOpacityMap) //Pack the gloss level into the diffuse map
                {
                    String glossLevelSrc = getSourceFullPath(description.GlossMapName);
                    if (imageNeedsCompression(glossLevelSrc) || compressDiffuse)
                    {
                        Log.Info("Compressing diffuse map {0} with gloss in alpha from {1}", description.DiffuseMapName, description.GlossMapName);
                        addMapToAlphaAndCompress(diffuseSrc, glossLevelSrc, Channel.Red, diffuseTmp, diffuseDest, compressDiffuseMap);
                    }
                }
                else //Just save the diffuse map as is
                {
                    if (compressDiffuse)
                    {
                        Log.Info("Compressing diffuse map {0} directly", description.DiffuseMapName);
                        compressDiffuseMap(diffuseSrc, diffuseDest);
                    }
                }
            }
            if (shouldSave(description.HasSpecularColorMap, description.SpecularMapName))
            {
                String specularSrc = getSourceFullPath(description.SpecularMapName);
                String specularDest = getDestBasePath(description.SpecularMapName);
                String specularTmp = getTempPath(description.SpecularMapName);
                bool compressSpecular = imageNeedsCompression(specularSrc);

                if (description.HasSpecularLevelMap)
                {
                    String specularLevelSrc = getSourceFullPath(description.SpecularLevelMapName);
                    if (imageNeedsCompression(specularLevelSrc) || compressSpecular)
                    {
                        Log.Info("Compressing specular map {0} with specular level in alpha {1}", description.DiffuseMapName, description.SpecularLevelMapName);
                        addMapToAlphaAndCompress(specularSrc, specularLevelSrc, Channel.Red, specularTmp, specularDest, compressSpecularMap);
                    }
                }
                else //Just save as is
                {
                    if (compressSpecular)
                    {
                        Log.Info("Compressing specular map {0} directly", description.DiffuseMapName);
                        compressSpecularMap(specularSrc, specularDest);
                    }
                }
            }
            if(shouldSave(description.HasOpacityMap, description.OpacityMapName))
            {
                String opacitySrc = getSourceFullPath(description.OpacityMapName);
                String opacityDest = getDestBasePath(description.OpacityMapName);
                String opacityTmp = getTempPath(description.OpacityMapName);
                bool compressOpacity = imageNeedsCompression(opacitySrc);

                if (description.HasGlossMap)
                {
                    String glossLevelSrc = getSourceFullPath(description.GlossMapName);
                    if (imageNeedsCompression(glossLevelSrc) || compressOpacity)
                    {
                        Log.Info("Compressing opacity map {0} with gloss in green from {1}", description.DiffuseMapName, description.GlossMapName);
                        combineSingleChannelMaps(opacitySrc, Channel.Red, glossLevelSrc, Channel.Red, opacityTmp, opacityDest, compressOpacityMap);
                    }
                }
                else //Just save as is
                {
                    if (compressOpacity)
                    {
                        Log.Info("Compressing opacity map {0} directly", description.DiffuseMapName);
                        compressOpacityMap(opacitySrc, opacityDest);
                    }
                }
            }
        }

        private void addMapToAlphaAndCompress(String rgbSource, String alphaSource, Channel alphaSourceChannel, String tempFile, String destinationFile, Action<String, String> compressFunction)
        {
            Log.Info("Building composite image with RGB {0} and alpha {1}", rgbSource, alphaSource);
            using (FreeImageBitmap rgbMap = FreeImageBitmap.FromFile(rgbSource))
            {
                using (FreeImageBitmap alphaMap = FreeImageBitmap.FromFile(alphaSource))
                {
                    using (FreeImageBitmap combined = createImageFromChannels(alphaMap, alphaSourceChannel, rgbMap))
                    {
                        saveImage(combined, tempFile, TempFileImageFormat);
                        compressFunction(tempFile, destinationFile);
                        deleteFile(tempFile);
                    }
                }
            }
        }

        private void combineSingleChannelMaps(String redSource, Channel redSourceChannel, String greenSource, Channel greenSourceChannel, String tempFile, String destinationFile, Action<String, String> compressFunction)
        {
            Log.Info("Building composite image with red source {0} and green source {1}", redSource, greenSource);
            using (FreeImageBitmap redMap = FreeImageBitmap.FromFile(redSource))
            {
                using (FreeImageBitmap greenMap = FreeImageBitmap.FromFile(greenSource))
                {
                    using (FreeImageBitmap combined = createImageFromChannels(redMap, redSourceChannel, greenMap, greenSourceChannel))
                    {
                        saveImage(combined, tempFile, TempFileImageFormat);
                        compressFunction(tempFile, destinationFile);
                        deleteFile(tempFile);
                    }
                }
            }
        }

        private bool shouldSave(bool hasMap, String mapName)
        {
            if(hasMap && !compiledTextures.Contains(mapName))
            {
                compiledTextures.Add(mapName);
                return true;
            }
            return false;
        }

        private void saveUncompressed(String sourceFile, String destFile)
        {
            if ((outputFormats & OutputFormats.Uncompressed) != 0)
            {
                Log.Info("Compressing {0} to png.", sourceFile);
                using (FreeImageBitmap source = FreeImageBitmap.FromFile(sourceFile))
                {
                    int i = source.Width;
                    String file = String.Format("{0}.png", destFile);
                    Log.Info("Saving {0}", file);
                    saveImage(source, file, FREE_IMAGE_FORMAT.FIF_PNG);
                    i >>= 1;
                    while (i > 0)
                    {
                        file = String.Format("{0}_{1}.png", destFile, i);
                        Log.Info("Saving {0}", file);

                        source.Rescale(new Size(i, i), FREE_IMAGE_FILTER.FILTER_LANCZOS3);
                        saveImage(source, file, FREE_IMAGE_FORMAT.FIF_PNG);
                        i >>= 1;
                    }
                }
            }
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
            if(File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private FreeImageBitmap createImageFromChannels(FreeImageBitmap alphaSrc, Channel alphaSrcChannel, FreeImageBitmap colorSrc)
        {
            return createImageFromChannels(alphaSrc, alphaSrcChannel, colorSrc, Channel.Red, colorSrc, Channel.Green, colorSrc, Channel.Blue);
        }

        private FreeImageBitmap createImageFromChannels(FreeImageBitmap alphaSrc, Channel alphaSrcChannel, FreeImageBitmap redSrc, Channel redSrcChannel, FreeImageBitmap greenSrc, Channel greenSrcChannel, FreeImageBitmap blueSrc, Channel blueSrcChannel)
        {
            if(alphaSrc.Width != redSrc.Width || redSrc.Width != greenSrc.Width || greenSrc.Width != blueSrc.Width)
            {
                //Do an error
            }

            int width = redSrc.Width;
            int height = redSrc.Height;
            Color c = new Color();

            FreeImageBitmap retVal = new FreeImageBitmap(width, height, FreeImageAPI.PixelFormat.Format32bppArgb);

            for (int x = 0; x < width; ++x)
            {
                for(int y = 0; y < height; ++y)
                {
                    c.A = getColor(x, y, alphaSrc, alphaSrcChannel);
                    c.R = getColor(x, y, redSrc, redSrcChannel);
                    c.G = getColor(x, y, greenSrc, greenSrcChannel);
                    c.B = getColor(x, y, blueSrc, blueSrcChannel);
                    retVal.SetPixel(x, y, c);
                }
            }

            return retVal;
        }

        private FreeImageBitmap createImageFromChannels(FreeImageBitmap redSrc, Channel redSrcChannel, FreeImageBitmap greenSrc, Channel greenSrcChannel, FreeImageBitmap blueSrc, Channel blueSrcChannel)
        {
            if (redSrc.Width != greenSrc.Width || greenSrc.Width != blueSrc.Width)
            {
                //Do an error
            }

            int width = redSrc.Width;
            int height = redSrc.Height;
            Color c = new Color();

            FreeImageBitmap retVal = new FreeImageBitmap(width, height, FreeImageAPI.PixelFormat.Format24bppRgb);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    c.R = getColor(x, y, redSrc, redSrcChannel);
                    c.G = getColor(x, y, greenSrc, greenSrcChannel);
                    c.B = getColor(x, y, blueSrc, blueSrcChannel);
                    retVal.SetPixel(x, y, c);
                }
            }

            return retVal;
        }

        private FreeImageBitmap createImageFromChannels(FreeImageBitmap redSrc, Channel redSrcChannel, FreeImageBitmap greenSrc, Channel greenSrcChannel)
        {
            if (redSrc.Width != greenSrc.Width)
            {
                //Do an error
            }

            int width = redSrc.Width;
            int height = redSrc.Height;
            Color c = new Color();

            FreeImageBitmap retVal = new FreeImageBitmap(width, height, FreeImageAPI.PixelFormat.Format24bppRgb);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    c.R = getColor(x, y, redSrc, redSrcChannel);
                    c.G = getColor(x, y, greenSrc, greenSrcChannel);
                    retVal.SetPixel(x, y, c);
                }
            }

            return retVal;
        }

        private byte getColor(int x, int y, FreeImageBitmap src, Channel channel)
        {
            Color c = src.GetPixel(x, y);
            switch(channel)
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
            throw new NotSupportedException(); //Won't get here
        }

        private void etc2Compress(String sourceFile, String destFile)
        {
            if ((outputFormats & OutputFormats.ETC2) != 0)
            {
                runExternalCompressionProcess(MaliTextureToolExe, String.Format(MaliTextureToolArgFormat, sourceFile, sourceDirectory));
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
        }

        private void bc5Compress(String source, String dest)
        {
            if ((outputFormats & OutputFormats.BC5Normal) != 0)
            {
                runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC5Format, source, dest));
            }
        }

        private void bc3nCompress(String source, String dest)
        {
            if ((outputFormats & OutputFormats.BC3) != 0)
            {
                runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC3nFormat, source, dest));
            }
        }

        private void bc3Compress(String source, String dest)
        {
            if ((outputFormats & OutputFormats.BC3) != 0)
            {
                runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC3Format, source, dest));
            }
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
            return String.Format(SourceFileFormat, Path.Combine(sourceDirectory, fileName));
        }

        private String getDestBasePath(String filename)
        {
            return Path.Combine(destDirectory, filename);
        }

        private void compressOpacityMap(string source, string dest)
        {
            ++compressedCount;
            bc3Compress(source, dest);
            etc2Compress(source, dest);
            saveUncompressed(source, dest);
        }

        private void compressSpecularMap(string source, string dest)
        {
            ++compressedCount;
            bc3Compress(source, dest);
            etc2Compress(source, dest);
            saveUncompressed(source, dest);
        }

        private void compressDiffuseMap(string source, string dest)
        {
            ++compressedCount;
            bc3Compress(source, dest);
            etc2Compress(source, dest);
            saveUncompressed(source, dest);
        }

        private void compressNormalMap(string source, string dest)
        {
            ++compressedCount;
            bc5Compress(source, dest);
            bc3nCompress(source, dest);
            etc2Compress(source, dest);
            saveUncompressed(source, dest);
        }

        private bool imageNeedsCompression(String source)
        {
            if(File.Exists(source))
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
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

namespace Anomalous.TextureCompiler
{
    enum Channel
    {
        Red,
        Green,
        Blue,
        Alpha
    }

    class TextureCompiler : MaterialBuilder
    {
        private List<String> errors = new List<string>();

        private static String SourceFileFormat = "{0}.psd";
        private static String TempFileFormat = "{0}_tmp.tga";
        private static FREE_IMAGE_FORMAT TempFileImageFormat = FREE_IMAGE_FORMAT.FIF_TARGA;

        private static String MainExeLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static String NVCompressExe = Path.Combine(MainExeLocation, "CompressorBinaries/Nvidia/nvcompress.exe");
        private static String NVCompressArgFormat = "-nocuda {0}";
        private static String NVCompressBC5Format = String.Format(NVCompressArgFormat, "-bc5 {0} {1}_bc5.dds");
        private static String NVCompressBC3nFormat = String.Format(NVCompressArgFormat, "-bc3n {0} {1}.dds");
        private static String NVCompressBC3Format = String.Format(NVCompressArgFormat, "-bc3 {0} {1}.dds");

        private static String MaliTextureToolExe = Path.Combine(MainExeLocation, "CompressorBinaries/MaliTextureTool/etcpack.exe");
        private static String MaliTextureToolArgFormat = "{0} {1} -c etc2 -f RGBA -ktx -mipmaps -ext PSD"; //-s slow

        private String sourceDirectory;
        private String destDirectory;

        private HashSet<String> compiledTextures = new HashSet<string>();

        public TextureCompiler(String sourceDirectory, String destDirectory)
        {
            this.sourceDirectory = sourceDirectory;
            this.destDirectory = destDirectory;
        }

        public override void buildMaterial(MaterialDescription description, MaterialRepository repo)
        {
            Log.Info("Compiling textures for material {0}", description.Name);
            if (shouldSave(description.HasNormalMap, description.NormalMapName))
            {
                //Normal maps are pretty much always the same.
                Log.Info("Compressing normal map {0}", description.NormalMapName);

                String source = getSourceFullPath(description.NormalMapName);
                String dest = getDestBasePath(description.NormalMapName);
                //DDS
                runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC5Format, source, dest));
                runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC3nFormat, source, dest));

                //ETC2
                etc2Compress(source, dest);

                //Uncompressed
                saveUncompressed(source, dest);
            }
            if (shouldSave(description.HasDiffuseMap, description.DiffuseMapName))
            {
                Log.Info("Compressing diffuse map {0}", description.DiffuseMapName);

                String diffuseSrc = getSourceFullPath(description.DiffuseMapName);
                String diffuseDest = getTempPath(description.DiffuseMapName);
                String diffuseTmp = getTempPath(description.DiffuseMapName);

                if (description.HasSpecularLevelMap && !description.HasSpecularColorMap) //If we don't have a specular color map pack the specular level into the diffuse map
                {
                    String specularLevelSrc = getSourceFullPath(description.SpecularLevelMapName);

                    using (FreeImageBitmap diffuseMap = FreeImageBitmap.FromFile(diffuseSrc))
                    {
                        using (FreeImageBitmap specularLevelMap = FreeImageBitmap.FromFile(specularLevelSrc))
                        {
                            using (FreeImageBitmap combined = createImageFromChannels(specularLevelMap, Channel.Red, diffuseMap))
                            {
                                saveImage(combined, diffuseTmp, TempFileImageFormat);
                                runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC3Format, diffuseTmp, diffuseDest));
                                etc2Compress(diffuseTmp, diffuseDest);
                                saveUncompressed(diffuseTmp, diffuseDest);
                                deleteFile(diffuseTmp);
                            }
                        }
                    }
                }
                else if (description.HasGlossMap) //Pack the gloss level into the diffuse map
                {
                    String glossLevelSrc = getSourceFullPath(description.GlossMapName);

                    using (FreeImageBitmap diffuseMap = FreeImageBitmap.FromFile(diffuseSrc))
                    {
                        using (FreeImageBitmap glossLevelMap = FreeImageBitmap.FromFile(glossLevelSrc))
                        {
                            using (FreeImageBitmap combined = createImageFromChannels(glossLevelMap, Channel.Red, diffuseMap))
                            {
                                saveImage(combined, diffuseTmp, TempFileImageFormat);
                                runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC3Format, diffuseTmp, diffuseDest));
                                etc2Compress(diffuseTmp, diffuseDest);
                                saveUncompressed(diffuseTmp, diffuseDest);
                                deleteFile(diffuseTmp);
                            }
                        }
                    }
                }
                else //Just save the diffuse map as is
                {
                    runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC3Format, diffuseSrc, diffuseDest));
                    etc2Compress(diffuseSrc, diffuseDest);
                    saveUncompressed(diffuseSrc, diffuseDest);
                }
            }
            if (shouldSave(description.HasSpecularColorMap, description.SpecularMapName))
            {
                String specularSrc = getSourceFullPath(description.DiffuseMapName);
                String specularDest = getDestBasePath(description.DiffuseMapName);
                String specularTmp = getTempPath(description.SpecularMapName + "_tmp");

                Log.Info("Compressing specular map {0}", description.SpecularMapName);
                if(description.HasSpecularLevelMap)
                {
                    String specularLevelSrc = getSourceFullPath(description.SpecularLevelMapName);

                    using (FreeImageBitmap specularColorMap = FreeImageBitmap.FromFile(specularSrc))
                    {
                        using (FreeImageBitmap specularLevelMap = FreeImageBitmap.FromFile(specularLevelSrc))
                        {
                            using (FreeImageBitmap combined = createImageFromChannels(specularLevelMap, Channel.Red, specularColorMap))
                            {
                                saveImage(combined, specularTmp, TempFileImageFormat);
                                runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC3Format, specularTmp, specularDest));
                                etc2Compress(specularTmp, specularDest);
                                saveUncompressed(specularTmp, specularDest);
                                deleteFile(specularTmp);
                            }
                        }
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
            Log.Info("Compressing {0} to png.", sourceFile);
            using (FreeImageBitmap source = FreeImageBitmap.FromFile(sourceFile))
            {
                saveImage(source, destFile + ".png", FREE_IMAGE_FORMAT.FIF_PNG);
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

            FreeImageBitmap retVal = new FreeImageBitmap(alphaSrc.Width, alphaSrc.Height, FreeImageAPI.PixelFormat.Format32bppArgb);

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
            catch(Exception ex)
            {
                logExceptionError(ex, String.Format("Deleting {0}", renameDst));
            }
            try
            {
                File.Move(renameSrc, renameDst);
            }
            catch(Exception ex)
            {
                logExceptionError(ex, String.Format("Moving {0} to {1}", renameSrc, renameDst));
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
    }
}

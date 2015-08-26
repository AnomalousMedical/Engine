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
    class TextureCompiler : MaterialBuilder
    {
        private static String MainExeLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static String NVCompressExe = Path.Combine(MainExeLocation, "CompressorBinaries/Nvidia/nvcompress.exe");
        private static String NVCompressArgFormat = "-nocuda {0}";
        private static String NVCompressBC5Format = String.Format(NVCompressArgFormat, "-bc5 {0}.psd {1}_bc5.dds");
        private static String NVCompressBC3nFormat = String.Format(NVCompressArgFormat, "-bc3n {0}.psd {1}.dds");

        private static String MaliTextureToolExe = Path.Combine(MainExeLocation, "CompressorBinaries/MaliTextureTool/etcpack.exe");
        private static String MaliTextureToolArgFormat = "{0}.psd {1} -c etc2 -f RGBA -ktx -mipmaps -ext PSD"; //-s slow

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
            if (description.HasNormalMap && !compiledTextures.Contains(description.NormalMapName))
            {
                //Normal maps are pretty much always the same.
                compiledTextures.Add(description.NormalMapName);
                Log.Info("Compressing normal map {0}", description.NormalMapName);

                String source = Path.Combine(sourceDirectory, description.NormalMapName);
                String dest = Path.Combine(destDirectory, description.NormalMapName);
                //DDS
                runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC5Format, source, dest));
                runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC3nFormat, source, dest));

                //ETC2
                etc2Compress(source);
            }
        }

        private void etc2Compress(String sourceFile)
        {
            runExternalCompressionProcess(MaliTextureToolExe, String.Format(MaliTextureToolArgFormat, sourceFile, sourceDirectory));
            String fileName = Path.GetFileNameWithoutExtension(sourceFile);
            String renameSrc = Path.Combine(sourceDirectory, fileName + ".ktx");
            String renameDst = Path.Combine(destDirectory, fileName + "_etc2.ktx");
            File.Move(renameSrc, renameDst);
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
    }
}

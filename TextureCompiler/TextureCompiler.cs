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
        private static String MaliTextureToolExe = Path.Combine(MainExeLocation, "CompressorBinaries/MaliTextureTool/etcpack.exe");

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
                runExternalCompressionProcess(NVCompressExe, String.Format(NVCompressBC5Format, source, dest));
            }
        }

        private void runExternalCompressionProcess(String executable, String args)
        {
            using (Process process = new Process()
            {
                StartInfo = new ProcessStartInfo(executable, args)
                {
                    WorkingDirectory = sourceDirectory,
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

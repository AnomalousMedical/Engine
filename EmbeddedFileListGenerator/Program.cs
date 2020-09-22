using System;
using System.IO;
using System.Text;

namespace EmbeddedFileListGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 2)
            {
                Console.WriteLine("You must pass a scan directory and output file.");
            }

            var scanDir = args[0];
            var outFile = args[1];

            var scanDirParent = Path.GetDirectoryName(scanDir);

            var sb = new StringBuilder();
            sb.Append(@"using System;
using System.Collections.Generic;
using System.Text;

namespace OgreNextPlugin
{
    partial class HlmsEmbeddedResourceArchive
    {
        private static void SetupFileMap()
        {
");

            foreach (var file in Directory.EnumerateFiles(scanDir, "*", SearchOption.AllDirectories))
            {
                var partialFile = file.Substring(scanDirParent.Length).Replace("\\", "/");
                var embedded = partialFile.Replace("/", ".").Substring(1);

                using var stream = File.OpenRead(file);

                sb.AppendLine(@$"
                fileMap[""{partialFile}""] = new HlmsEmbeddedFileInfo()
                {{
                    Size = {stream.Length},
                    BaseName = ""{Path.GetFileName(partialFile)}"",
                    FileName = ""{partialFile}"",
                    Path = ""{Path.GetDirectoryName(partialFile).Replace('\\', '/')}"",
                    EmbeddedResourcePath = ""{embedded}"",
                }};");

                //sb.AppendLine($"fileMap[\"{partialFile}\"] = \"OgreNextPlugin{embedded}\";");
            }

            sb.Append(@"
        }
    }
}");

            File.WriteAllText(outFile, sb.ToString());
        }
    }
}

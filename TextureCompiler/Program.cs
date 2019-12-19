using Anomalous.OSPlatform;
using Anomalous.OSPlatform.Win32;
using Engine;
using Engine.Resources;
using Microsoft.Extensions.DependencyInjection;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.TextureCompiler
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                String sourceDirectory = args[0];
                String destDirectory = args[1];
                OutputFormats outFormats = OutputFormats.Uncompressed;
                if(args.Length > 2)
                {
                    Enum.TryParse(args[2], out outFormats);
                }

                int maxSize = int.MaxValue;
                if(args.Length > 3)
                {
                    if(!int.TryParse(args[3], out maxSize))
                    {
                        maxSize = int.MaxValue;
                    }
                }

                Environment.CurrentDirectory = sourceDirectory;

                Logging.Log.Default.addLogListener(new Logging.LogConsoleListener());
                WindowsRuntimePlatformInfo.Initialize();
                PluginManager pluginManager = new PluginManager(new ConfigFile("woot.txt"), new ServiceCollection());
                VirtualFileSystem.Instance.addArchive(destDirectory);

                TextureCompilerInterface.CompileTextures(sourceDirectory, destDirectory, pluginManager, outFormats, maxSize);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

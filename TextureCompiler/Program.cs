using Anomalous.OSPlatform;
using Engine;
using Engine.Resources;
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
                OutputFormats outFormats = OutputFormats.All;
                if(args.Length >= 2)
                {
                    Enum.TryParse(args[2], out outFormats);
                }

                Environment.CurrentDirectory = sourceDirectory;

                Logging.Log.Default.addLogListener(new Logging.LogConsoleListener());
                NativePlatformPlugin.StaticInitialize();
                PluginManager pluginManager = new PluginManager(new ConfigFile("woot.txt"));
                VirtualFileSystem.Instance.addArchive(destDirectory);

                TextureCompilerInterface.CompileTextures(sourceDirectory, destDirectory, pluginManager, outFormats);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

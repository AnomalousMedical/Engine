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
            String sourceDirectory = args[0];
            String destDirectory = Environment.CurrentDirectory;

            Logging.Log.Default.addLogListener(new Logging.LogConsoleListener());
            NativePlatformPlugin.StaticInitialize();
            PluginManager pluginManager = new PluginManager(new ConfigFile("woot.txt"));
            VirtualFileSystem.Instance.addArchive(destDirectory);

            pluginManager.addSubsystemResources("TextureCompiler", new MaterialParserResourceListener(sourceDirectory, destDirectory));
            var resourceManager = pluginManager.createLiveResourceManager("TextureCompiler");
            var subsystem = resourceManager.getSubsystemResource("TextureCompiler");
            var group = subsystem.addResourceGroup("TextureCompiler");
            group.addResource("", "EngineArchive", true);

            resourceManager.initializeResources();

            Console.WriteLine("All Textures compiled. Press any key to quit.");
            Console.ReadKey();
        }
    }
}

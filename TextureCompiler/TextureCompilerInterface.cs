using Engine;
using Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.TextureCompiler
{
    public static class TextureCompilerInterface
    {
        public static readonly String TextureHashFileName = "TextureHashes.hsh";

        public static void CompileTextures(String sourceDirectory, String destDirectory, PluginManager pluginManager)
        {
            try
            {
                if (String.IsNullOrEmpty(sourceDirectory) || !Directory.Exists(sourceDirectory))
                {
                    Log.Error("Source directory {0} does not exist.", sourceDirectory);
                    return;
                }

                if (String.IsNullOrEmpty(destDirectory) || !Directory.Exists(destDirectory))
                {
                    Log.Error("Destination directory {0} does not exist.", destDirectory);
                    return;
                }

                var listener = new MaterialParserResourceListener(sourceDirectory, destDirectory);
                listener.loadTextureInfo();

                var resourceManager = pluginManager.createResourceManagerForListener("TextureCompiler", listener);
                var subsystem = resourceManager.getSubsystemResource("TextureCompiler");
                var group = subsystem.addResourceGroup("TextureCompiler");
                group.addResource("", "EngineArchive", true);

                resourceManager.initializeResources();

                listener.saveTextureInfo();
            }
            catch(Exception ex)
            {
                Log.Error("{0} commpiling textures. Reason: {1}", ex.GetType().Name, ex.Message);
                Log.Default.printException(ex);
            }
        }
    }
}

using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.TextureCompiler
{
    public static class TextureCompilerInterface
    {
        public static void CompileTextures(String sourceDirectory, String destDirectory, PluginManager pluginManager)
        {
            var listener = new MaterialParserResourceListener(sourceDirectory, destDirectory);
            listener.loadTextureInfo();

            var resourceManager = pluginManager.createResourceManagerForListener("TextureCompiler", listener);
            var subsystem = resourceManager.getSubsystemResource("TextureCompiler");
            var group = subsystem.addResourceGroup("TextureCompiler");
            group.addResource("", "EngineArchive", true);

            resourceManager.initializeResources();

            listener.saveTextureInfo();
        }
    }
}

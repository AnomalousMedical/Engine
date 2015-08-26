using Anomalous.OSPlatform;
using Engine;
using Engine.Resources;
using Newtonsoft.Json;
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
        class TextureCompiler : MaterialBuilder
        {
            public override void buildMaterial(MaterialDescription description, MaterialRepository repo)
            {
                Logging.Log.Debug("Material {0} seen", description.Name);
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

        class ResourceListenerBridge : ResourceListener
        {
            TextureCompiler textureCompiler = new TextureCompiler();
            MaterialParserManager materialParser = new MaterialParserManager();
            public ResourceListenerBridge()
            {
                materialParser.addMaterialBuilder(textureCompiler);
            }

            public void initializeResources(IEnumerable<ResourceGroup> groups)
            {
                materialParser.initializeResources(groups);
            }

            public void resourceAdded(ResourceGroup group, Engine.Resources.Resource resource)
            {
                materialParser.resourceAdded(group, resource);
            }

            public void resourceGroupAdded(ResourceGroup group)
            {
                materialParser.resourceGroupAdded(group);
            }

            public void resourceGroupRemoved(ResourceGroup group)
            {
                materialParser.resourceGroupRemoved(group);
            }

            public void resourceRemoved(ResourceGroup group, Engine.Resources.Resource resource)
            {
                materialParser.resourceRemoved(group, resource);
            }
        }

        static void Main(string[] args)
        {
            Logging.Log.Default.addLogListener(new Logging.LogConsoleListener());
            NativePlatformPlugin.StaticInitialize();
            PluginManager pluginManager = new PluginManager(new ConfigFile("woot.txt"));
            VirtualFileSystem.Instance.addArchive(Environment.CurrentDirectory);

            pluginManager.addSubsystemResources("Ogre", new ResourceListenerBridge());
            var resourceManager = pluginManager.createLiveResourceManager("TextureCompiler");
            var subsystem = resourceManager.getSubsystemResource("Ogre");
            var group = subsystem.addResourceGroup("TextureCompiler");
            group.addResource("", "EngineArchive", true);

            resourceManager.initializeResources();
        }
    }
}

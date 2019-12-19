using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anomalous.OgreOpenVr
{
    public class OgreOpenVrInterface : PluginInterface
    {
        public string Name => "OgreOpenVr";
        private OgreFramework ogreFramework;
        private UpdateTimer mainTimer;

        public void Dispose()
        {
            if (ogreFramework != null)
            {
                ogreFramework.Dispose();
                ogreFramework = null;
            }
        }

        public void createDebugCommands(List<CommandManager> commands)
        {

        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public void initialize(PluginManager pluginManager, IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<OgreFramework>(s =>{
                var ogreFramework = new OgreFramework();
                mainTimer.addUpdateListener(new OgreFrameworkUpdateListener(ogreFramework));
                return ogreFramework;
            });
        }

        public void link(PluginManager pluginManager)
        {
            //Temp, load resources from fs
            //C:\Development\openvrtest\Install\media\textures
            VirtualFileSystem.Instance.addArchive("C:/Development/openvrtest/Install/media");
            OgreResourceGroupManager.getInstance().addResourceLocation("ShadersDX11/GuiResource_Gui", "EngineArchive", "Vr", false);
            OgreResourceGroupManager.getInstance().addResourceLocation("ShadersDX11", "EngineArchive", "Vr", false);
            OgreResourceGroupManager.getInstance().addResourceLocation("textures", "EngineArchive", "Vr", true);
            OgreResourceGroupManager.getInstance().initializeAllResourceGroups();

            //var ogreRoot = pluginManager.GlobalScope.Resolve<Root>();
            //ogreFramework = new OgreFramework();
            //ogreFramework.Init(ogreRoot);
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            this.mainTimer = mainTimer;
        }

        public void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap)
        {

        }
    }
}

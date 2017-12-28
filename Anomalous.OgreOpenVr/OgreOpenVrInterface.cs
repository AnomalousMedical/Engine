using Autofac;
using Engine;
using Engine.Platform;
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

        public void Dispose()
        {
            if(ogreFramework != null)
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

        public void initialize(PluginManager pluginManager, ContainerBuilder builder)
        {
            
        }

        public void link(PluginManager pluginManager)
        {
            var ogreRoot = pluginManager.GlobalScope.Resolve<Root>();
            ogreFramework = new OgreFramework();
            ogreFramework.Init(ogreRoot);
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            mainTimer.addUpdateListener(new OgreFrameworkUpdateListener(ogreFramework));
        }

        public void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap)
        {
            
        }
    }
}

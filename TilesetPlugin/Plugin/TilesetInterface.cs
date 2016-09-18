using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using Engine.Resources;
using Engine.Command;

namespace TilesetPlugin
{
    public class TilesetInterface : PluginInterface
    {
        public const String PluginName = "TilesetPlugin";

        public TilesetInterface(PluginManager pluginManager)
        {
            pluginManager.Injector.addSingleton<TilesetInterface>(() => this);
        }

        public void Dispose()
        {
            
        }

        public void initialize(PluginManager pluginManager)
        {
            

	        //pluginManager.addSubsystemResources("Bullet", fileManager);
        }

        public void link(PluginManager pluginManager)
        {

        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            
        }

        public string Name
        {
            get
            {
                return PluginName;
            }
        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public void createDebugCommands(List<CommandManager> commands)
        {
            
        }

        public void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap)
        {

        }
    }
}

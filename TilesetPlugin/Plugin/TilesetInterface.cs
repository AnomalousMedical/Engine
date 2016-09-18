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

        private TilesetLoader loader;
        private TilesetManager manager;

        public TilesetInterface(PluginManager pluginManager)
        {
            manager = new TilesetManager();
            loader = new TilesetLoader(manager);

            pluginManager.Injector.addSingleton<TilesetManager>(() => manager);
        }

        public void Dispose()
        {
            
        }

        public void initialize(PluginManager pluginManager)
        {
	        pluginManager.addSubsystemResources("Tileset", loader);
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

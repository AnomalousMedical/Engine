using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using Engine.Resources;
using Engine.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Anomalous.TilesetPlugin
{
    public class TilesetInterface : PluginInterface
    {
        public const String PluginName = "Anomalous.TilesetPlugin";

        public TilesetInterface(PluginManager pluginManager)
        {
            
        }

        public void Dispose()
        {
            
        }

        public void initialize(PluginManager pluginManager, IServiceCollection serviceCollection)
        {
            var manager = new TilesetManager();
            var loader = new TilesetLoader(manager);
            serviceCollection.TryAddSingleton<TilesetManager>(manager);
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

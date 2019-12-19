using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine.Command;
using Microsoft.Extensions.DependencyInjection;

namespace Engine
{
    public class BehaviorPluginInterface : PluginInterface
    {
        private static BehaviorPluginInterface instance;
        public static BehaviorPluginInterface Instance
        {
            get
            {
                return instance;
            }
        }

        private UpdateTimer timer;
        private EventManager eventManager;
        private BehaviorDebugInterface debugInterface;

        public BehaviorPluginInterface()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                throw new Exception("Cannot create the BehaviorInterface more than once. Only call the constructor one time.");
            }
        }

        public void initialize(PluginManager pluginManager, IServiceCollection serviceCollection)
        {
            pluginManager.addCreateSimElementManagerCommand(new AddSimElementManagerCommand("Create Behavior Manager Definition", BehaviorManagerDefinition.Create));

            pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create Behavior Definition", BehaviorDefinition.Create));
        }

        public void link(PluginManager pluginManager)
        {

        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            this.timer = mainTimer;
            this.eventManager = eventManager;
        }

        public string Name
        {
            get
            {
                return "Behavior";
            }
        }

        public DebugInterface getDebugInterface()
        {
            if (debugInterface == null)
            {
                debugInterface = new BehaviorDebugInterface();
            }
            return debugInterface;
        }

        public void Dispose()
        {
            
        }

        /// <summary>
        /// This function will create any debug commands for the plugin and add them to the commands list.
        /// </summary>
        /// <param name="commands">A list of CommandManagers to add debug commands to.</param>
        public void createDebugCommands(List<CommandManager> commands)
        {

        }

        public void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap)
        {

        }

        public UpdateTimer Timer
        {
            get
            {
                return timer;
            }
        }

        public EventManager EventManager
        {
            get
            {
                return eventManager;
            }
        }
    }
}

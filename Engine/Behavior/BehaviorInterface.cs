using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Timers;

namespace Engine
{
    public class BehaviorInterface : PluginInterface
    {
        private static BehaviorInterface instance;
        public static BehaviorInterface Instance
        {
            get
            {
                return instance;
            }
        }

        private UpdateTimer timer;
        private EventManager eventManager;

        public BehaviorInterface()
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

        public void initialize(PluginManager pluginManager)
        {
            pluginManager.addCreateSimElementManagerCommand(new EngineCommand("createBehaviorManagerDefinition", "Create Behavior Manager Definition", "Create a new Behavior Manager.", new CreateElementManagerDefinition(BehaviorManagerDefinition.Create)));

            pluginManager.addCreateSimElementCommand(new EngineCommand("createBehaviorDefinition", "Create Behavior Definition", "Create a new Behavior Definition", new CreateElementDefinition(BehaviorDefinition.Create)));
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            this.timer = mainTimer;
            this.eventManager = eventManager;
        }

        public string getName()
        {
            return "Behavior";
        }

        public void Dispose()
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

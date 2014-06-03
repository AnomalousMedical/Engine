using Engine;
using Engine.Command;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    class BEPUIkInterface : PluginInterface
    {
        public const String PluginName = "BEPUikPlugin";

        public BEPUIkInterface()
        {

        }

        public void Dispose()
        {

        }

        public void initialize(PluginManager pluginManager)
        {
            pluginManager.addCreateSimElementManagerCommand(new AddSimElementManagerCommand("Create BEPU Ik Scene Definition", new CreateSimElementManager(BEPUIkSceneDefinition.Create)));
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            
        }

        public string getName()
        {
            return PluginName;
        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public void createDebugCommands(List<CommandManager> commands)
        {
            
        }
    }
}

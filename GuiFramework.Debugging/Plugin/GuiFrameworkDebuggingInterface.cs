using Engine;
using Engine.Platform;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework.Debugging
{
    public class GuiFrameworkDebuggingInterface : PluginInterface
    {
        public static Object ToolsEventLayers { get; set; }

        internal GuiFrameworkDebuggingInterface()
        {

        }

        public void Dispose()
        {

        }

        public void initialize(PluginManager pluginManager)
        {
            
        }

        public void link(PluginManager pluginManager)
        {
            MyGUIInterface.Instance.CommonResourceGroup.addResource(GetType().AssemblyQualifiedName, "EmbeddedScalableResource", true);
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            
        }

        public string Name
        {
            get
            {
                return "GuiFramework.Debugging";
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

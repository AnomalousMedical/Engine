using Engine;
using Engine.Platform;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GuiFramework.Plugin
{
    public class GuiFrameworkInterface : PluginInterface
    {
        public const String PluginName = "GuiFramework";

        public GuiFrameworkInterface()
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
            //Load Core Resources
            ResourceManager.Instance.load("GuiFramework.Resources.MyGUI_Skin.xml");
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
            renamedTypeMap.addRenamedType("Medical.Controller.AnomalousMvc.ViewLocations", typeof(Medical.ViewLocations));
            renamedTypeMap.addRenamedType("Medical.WindowAlignment", typeof(Medical.WindowAlignment));
            renamedTypeMap.addRenamedType("Medical.LayoutElementName", typeof(Medical.LayoutElementName));
            renamedTypeMap.addRenamedType("Medical.BorderLayoutElementName", typeof(Medical.BorderLayoutElementName));
            renamedTypeMap.addRenamedType("Medical.BorderLayoutLocations", typeof(Medical.BorderLayoutLocations));
            renamedTypeMap.addRenamedType("Medical.MDILayoutElementName", typeof(Medical.MDILayoutElementName));
        }
    }
}

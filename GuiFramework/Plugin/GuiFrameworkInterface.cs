using Anomalous.OSPlatform;
using Autofac;
using Engine;
using Engine.Platform;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework
{
    public class GuiFrameworkInterface : PluginInterface
    {
        public const String PluginName = "GuiFramework";

        public static GuiFrameworkInterface Instance { get; private set; }

        internal GuiFrameworkInterface()
        {
            if(Instance != null)
            {
                throw new Exception("Only create one instance of GuiFrameworkInterface");
            }

            Instance = this;
        }

        public void Dispose()
        {

        }

        public void initialize(PluginManager pluginManager, ContainerBuilder builder)
        {
            
        }

        public void link(PluginManager pluginManager)
        {
            MyGUIInterface.Instance.CommonResourceGroup.addResource(GetType().AssemblyQualifiedName, "EmbeddedScalableResource", true);
            //Load Core Resources
            ResourceManager.Instance.load("Anomalous.GuiFramework.Resources.Imagesets.xml");
            ResourceManager.Instance.load("Anomalous.GuiFramework.Resources.MyGUI_Skin.xml");
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            AnimatedLayoutContainerManager.Timer = mainTimer;
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
            renamedTypeMap.addRenamedType("Medical.Controller.AnomalousMvc.ViewLocations", typeof(ViewLocations));
            renamedTypeMap.addRenamedType("Medical.WindowAlignment", typeof(WindowAlignment));
            renamedTypeMap.addRenamedType("Medical.LayoutElementName", typeof(LayoutElementName));
            renamedTypeMap.addRenamedType("Medical.BorderLayoutElementName", typeof(BorderLayoutElementName));
            renamedTypeMap.addRenamedType("Medical.BorderLayoutLocations", typeof(BorderLayoutLocations));
            renamedTypeMap.addRenamedType("Medical.MDILayoutElementName", typeof(MDILayoutElementName));
        }

        /// <summary>
        /// Handle changing the cursors for a given NativeOSWindow automatically. This will stop managing cursors
        /// when the window is disposed.
        /// </summary>
        /// <param name="osWindow"></param>
        public void handleCursors(NativeOSWindow osWindow)
        {
            new CursorManager(osWindow, PointerManager.Instance);
        }
    }
}

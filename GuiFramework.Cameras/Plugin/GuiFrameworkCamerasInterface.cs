using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework.Cameras
{
    public class GuiFrameworkCamerasInterface : PluginInterface
    {
        /// <summary>
        /// How long it takes a camera to move automatically
        /// </summary>
        public static float CameraTransitionTime { get; set; }

        public static KeyboardButtonCode PanKey { get; set; }

        public static MouseButtonCode DefaultCameraButton { get; set; }

        public static Object MoveCameraEventLayer { get; set; }

        public static Object SelectWindowEventLayer { get; set; }

        public static TouchType TouchType { get; set; }

        static GuiFrameworkCamerasInterface()
        {
            CameraTransitionTime = 0.5f;
            PanKey = KeyboardButtonCode.KC_LCONTROL;
            DefaultCameraButton = MouseButtonCode.MB_BUTTON1;
            TouchType = TouchType.None;
        }

        internal GuiFrameworkCamerasInterface()
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
                return "GuiFramework.Cameras";
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
            renamedTypeMap.addRenamedType("Medical.CameraPosition", typeof(CameraPosition));
        }
    }
}

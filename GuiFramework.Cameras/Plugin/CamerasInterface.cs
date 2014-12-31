using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework.Cameras
{
    public class CamerasInterface : PluginInterface
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

        public static event SceneViewWindowEvent WindowCreated;

        public static event SceneViewWindowEvent WindowDestroyed;

        static CamerasInterface()
        {
            CameraTransitionTime = 0.5f;
            PanKey = KeyboardButtonCode.KC_LCONTROL;
            DefaultCameraButton = MouseButtonCode.MB_BUTTON1;
            TouchType = TouchType.None;
        }

        internal static void fireWindowCreated(SceneViewWindow window)
        {
            if(WindowCreated != null)
            {
                WindowCreated.Invoke(window);
            }
        }

        internal static void fireWindowDestroyed(SceneViewWindow window)
        {
            if (WindowDestroyed != null)
            {
                WindowDestroyed.Invoke(window);
            }
        }

        internal CamerasInterface()
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

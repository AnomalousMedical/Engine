using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace MyGUIPlugin
{
    public class Gui : IDisposable
    {
        static Gui instance;

        public static Gui Instance
        {
            get
            {
                return instance;
            }
        }

        IntPtr gui;

        public Gui()
        {
            if (instance != null)
            {
                throw new Exception("Only create the Gui class one time");
            }
            gui = Gui_Create();
            instance = this;
        }

        public void Dispose()
        {
            Widget.destroyAllWrappers();
            Gui_Delete(gui);
        }

        public void initialize(String coreConfig, String logFile)
        {
            Gui_initialize(gui, coreConfig, logFile);
        }

        public void shutdown()
        {
            Gui_shutdown(gui);
        }

        public Widget createWidgetT(String type, String skin, int left, int top, int width, int height, Align align, String layer, String name)
        {
            return Widget.getWidget(Gui_createWidgetT(gui, type, skin, left, top, width, height, align, layer, name));
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Gui_Create();

        [DllImport("MyGUIWrapper")]
        private static extern void Gui_Delete(IntPtr gui);

        [DllImport("MyGUIWrapper")]
        private static extern void Gui_initialize(IntPtr gui, String coreConfig, String logFile);

        [DllImport("MyGUIWrapper")]
        private static extern void Gui_shutdown(IntPtr gui);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Gui_createWidgetT(IntPtr gui, String type, String skin, int left, int top, int width, int height, Align align, String layer, String name);

#endregion
    }
}

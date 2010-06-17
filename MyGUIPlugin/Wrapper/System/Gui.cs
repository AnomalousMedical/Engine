using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using Engine.Platform;

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

        public bool injectMouseMove(int absx, int absy, int absz)
        {
            return Gui_injectMouseMove(gui, absx, absy, absz);
        }

        public bool injectMousePress(int absx, int absy, MouseButtonCode id)
        {
            return Gui_injectMousePress(gui, absx, absy, id);
        }

        public bool injectMouseRelease(int absx, int absy, MouseButtonCode id)
        {
            return Gui_injectMouseRelease(gui, absx, absy, id);
        }

        public bool injectKeyPress(KeyboardButtonCode key, uint text)
        {
            return Gui_injectKeyPress(gui, key, text);
        }

        public bool injectKeyRelease(KeyboardButtonCode key)
        {
            return Gui_injectKeyRelease(gui, key);
        }

        public void setVisiblePointer(bool visible)
        {
            Gui_setVisiblePointer(gui, visible);
        }

        public bool isVisiblePointer()
        {
            return Gui_isVisiblePointer(gui);
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

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Gui_injectMouseMove(IntPtr gui, int absx, int absy, int absz);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Gui_injectMousePress(IntPtr gui, int absx, int absy, MouseButtonCode id);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Gui_injectMouseRelease(IntPtr gui, int absx, int absy, MouseButtonCode id);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Gui_injectKeyPress(IntPtr gui, KeyboardButtonCode key, uint text);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Gui_injectKeyRelease(IntPtr gui, KeyboardButtonCode key);

        [DllImport("MyGUIWrapper")]
        private static extern void Gui_setVisiblePointer(IntPtr gui, bool visible);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Gui_isVisiblePointer(IntPtr gui);

#endregion
    }
}

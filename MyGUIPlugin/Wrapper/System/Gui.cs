using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using Engine.Platform;

namespace MyGUIPlugin
{
    internal delegate void MouseEvent(int x, int y, MouseButtonCode button);
    internal delegate void UpdateEvent(float updateTime);

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

        internal event MouseEvent MouseButtonPressed;
        internal event MouseEvent MouseButtonReleased;
        internal event UpdateEvent Update;

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
            WidgetManager.destroyAllWrappers();
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

        /// <summary>
        /// Create a widget.
        /// </summary>
        /// <param name="type">Widget type.</param>
        /// <param name="skin">Widget skin.</param>
        /// <param name="left">Widget x pos.</param>
        /// <param name="top">Widget y pos.</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="align">Widget align.</param>
        /// <param name="layer">Layer where the widget will be created.</param>
        /// <param name="name">The name to find the widget later.</param>
        /// <returns></returns>
        public Widget createWidgetT(String type, String skin, int left, int top, int width, int height, Align align, String layer, String name)
        {
            return WidgetManager.getWidget(Gui_createWidgetT(gui, type, skin, left, top, width, height, align, layer, name));
        }

        /// <summary>
        /// Create a widget using coords relative to the parent.
        /// </summary>
        /// <param name="type">Widget type.</param>
        /// <param name="skin">Widget skin.</param>
        /// <param name="left">Widget x pos.</param>
        /// <param name="top">Widget y pos.</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="align">Widget align.</param>
        /// <param name="layer">Layer where the widget will be created.</param>
        /// <param name="name">The name to find the widget later.</param>
        /// <returns></returns>
        public Widget createWidgetRealT(String type, String skin, int left, int top, int width, int height, Align align, String layer, String name)
        {
            return WidgetManager.getWidget(Gui_createWidgetRealT(gui, type, skin, left, top, width, height, align, layer, name));
        }

        public int getViewWidth()
        {
            return Gui_getViewWidth(gui);
        }

        public int getViewHeight()
        {
            return Gui_getViewHeight(gui);
        }

        public bool injectMouseMove(int absx, int absy, int absz)
        {
            return Gui_injectMouseMove(gui, absx, absy, absz);
        }

        public bool injectMousePress(int absx, int absy, MouseButtonCode id)
        {
            bool handled = Gui_injectMousePress(gui, absx, absy, id);
            if (MouseButtonPressed != null)
            {
                MouseButtonPressed.Invoke(absx, absy, id);
            }
            return handled;
        }

        public bool injectMouseRelease(int absx, int absy, MouseButtonCode id)
        {
            bool handled = Gui_injectMouseRelease(gui, absx, absy, id);
            if (MouseButtonReleased != null)
            {
                MouseButtonReleased.Invoke(absx, absy, id);
            }
            return handled;
        }

        public bool injectKeyPress(KeyboardButtonCode key, uint text)
        {
            return Gui_injectKeyPress(gui, key, text);
        }

        public bool injectKeyRelease(KeyboardButtonCode key)
        {
            return Gui_injectKeyRelease(gui, key);
        }

        public void destroyWidget(Widget widget)
        {
            Gui_destroyWidget(gui, WidgetManager.deleteWrapperAndChildren(widget));
        }

        public Widget findWidgetT(String name)
        {
            return WidgetManager.getWidget(Gui_findWidgetT(gui, name));
        }

        public Widget findWidgetT(String name, String prefix)
        {
            return WidgetManager.getWidget(Gui_findWidgetT2(gui, name, prefix));
        }

        public void setVisiblePointer(bool visible)
        {
            Gui_setVisiblePointer(gui, visible);
        }

        public bool isVisiblePointer()
        {
            return Gui_isVisiblePointer(gui);
        }

        public bool load(String file)
        {
            return Gui_load(gui, file);
        }

        public bool HandledMouseButtons { get; internal set; }

        public bool HandledKeyboardButtons { get; internal set; }

        internal void fireUpdateEvent(float updateTime)
        {
            if (Update != null)
            {
                Update.Invoke(updateTime);
            }
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
        private static extern IntPtr Gui_createWidgetRealT(IntPtr gui, String type, String skin, int left, int top, int width, int height, Align align, String layer, String name);

        [DllImport("MyGUIWrapper")]
        private static extern int Gui_getViewWidth(IntPtr gui);

        [DllImport("MyGUIWrapper")]
        private static extern int Gui_getViewHeight(IntPtr gui);

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
        private static extern void Gui_destroyWidget(IntPtr gui, IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Gui_findWidgetT(IntPtr gui, String name);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Gui_findWidgetT2(IntPtr gui, String name, String prefix);

        [DllImport("MyGUIWrapper")]
        private static extern void Gui_setVisiblePointer(IntPtr gui, bool visible);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Gui_isVisiblePointer(IntPtr gui);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Gui_load(IntPtr gui, String file);

#endregion
    }
}

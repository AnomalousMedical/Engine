﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using Engine.Platform;

namespace MyGUIPlugin
{
    internal delegate void MouseEvent(int x, int y, MouseButtonCode button);
    public delegate void UpdateEvent(float updateTime);

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

        public event UpdateEvent Update;

        IntPtr gui;

        public Gui()
        {
            if (instance != null)
            {
                throw new Exception("Only create the Gui class one time");
            }
            gui = Gui_Create();
            instance = this;
            Disposing = false;
        }

        public void Dispose()
        {
            Disposing = true;
            PointerManager.Instance.Dispose();
            WidgetManager.destroyAllWrappers();
            Gui_Delete(gui);
        }

        public void initialize(String coreConfig)
        {
            Gui_initialize(gui, coreConfig);
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

        public void destroyWidget(Widget widget)
        {
            Gui_destroyWidget(gui, widget.WidgetPtr);
        }

        public Widget findWidgetT(String name)
        {
            return WidgetManager.getWidget(Gui_findWidgetT(gui, name));
        }

        public Widget findWidgetT(String name, String prefix)
        {
            return WidgetManager.getWidget(Gui_findWidgetT2(gui, name, prefix));
        }

        public void keepWidgetOnscreen(Widget widget)
        {
            bool needsMoved = false;
            IntVector2 location = new IntVector2(widget.Left, widget.Top);
            IntSize2 size = new IntSize2(widget.Width, widget.Height);

            int viewWidth = getViewWidth();
            if (location.x + size.Width > viewWidth)
            {
                needsMoved = true;
                location.x = viewWidth - size.Width;
                if (location.x < 0)
                {
                    location.x = 0;
                }
            }

            int viewHeight = getViewHeight();
            if (location.y + size.Height > viewHeight)
            {
                needsMoved = true;
                location.y = viewHeight - size.Height;
                if (location.y < 0)
                {
                    location.y = 0;
                }
            }

            if (needsMoved)
            {
                widget.setPosition(location.x, location.y);
            }
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

        internal bool Disposing { get; private set; }

#region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Gui_Create();

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Gui_Delete(IntPtr gui);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Gui_initialize(IntPtr gui, String coreConfig);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Gui_shutdown(IntPtr gui);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Gui_createWidgetT(IntPtr gui, String type, String skin, int left, int top, int width, int height, Align align, String layer, String name);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Gui_createWidgetRealT(IntPtr gui, String type, String skin, int left, int top, int width, int height, Align align, String layer, String name);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int Gui_getViewWidth(IntPtr gui);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int Gui_getViewHeight(IntPtr gui);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Gui_destroyWidget(IntPtr gui, IntPtr widget);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Gui_findWidgetT(IntPtr gui, String name);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Gui_findWidgetT2(IntPtr gui, String name, String prefix);

#endregion
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class ScrollView : Widget
    {
        public ScrollView(IntPtr scrollView)
            :base(scrollView)
        {

        }

        //ScrollView
        void setCanvasSize(int width, int height)
        {
            ScrollView_setCanvasSize(widget, width, height);
        }

        public IntSize2 CanvasSize
        {
            get
            {
                return ScrollView_getCanvasSize(widget).toIntSize2();
            }
            set
            {
                ScrollView_setCanvasSize(widget, (int)value.Width, (int)value.Height);
            }
        }

        public bool VisibleVScroll
        {
            get
            {
                return ScrollView_isVisibleVScroll(widget);
            }
            set
            {
                ScrollView_setVisibleVScroll(widget, value);
            }
        }

        public bool VisibleHScroll
        {
            get
            {
                return ScrollView_isVisibleHScroll(widget);
            }
            set
            {
                ScrollView_setVisibleHScroll(widget, value);
            }
        }

        public Align CanvasAlign
        {
            get
            {
                return ScrollView_getCanvasAlign(widget);
            }
            set
            {
                ScrollView_setCanvasAlign(widget, value);
            }
        }

        public IntVector2 CanvasPosition
        {
            get
            {
                return ScrollView_getCanvasPosition(widget).toIntVector2();
            }
            set
            {
                ScrollView_setCanvasPosition(widget, value.x, value.y);
            }
        }

        public bool FavorVertical
        {
            get
            {
                return ScrollView_getFavorVertical(widget);
            }
            set
            {
                ScrollView_setFavorVertical(widget, value);
            }
        }

        public bool AllowMouseScroll
        {
            get
            {
                return ScrollView_getAllowMouseScroll(widget);
            }
            set
            {
                ScrollView_setAllowMouseScroll(widget, value);
            }
        }

        public IntCoord ViewCoord
        {
            get
            {
                return ScrollView_getViewCoord(widget);
            }
        }

        public event MyGUIEvent CanvasPositionChanged
        {
            add
            {
                eventManager.addDelegate<EventCanvasPositionChangedTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventCanvasPositionChangedTranslator>(value);
            }
        }

#region PInvoke

        //ScrollView
        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollView_setVisibleVScroll(IntPtr scrollView, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ScrollView_isVisibleVScroll(IntPtr scrollView);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollView_setVisibleHScroll(IntPtr scrollView, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ScrollView_isVisibleHScroll(IntPtr scrollView);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollView_setCanvasAlign(IntPtr scrollView, Align value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern Align ScrollView_getCanvasAlign(IntPtr scrollView);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollView_setCanvasSize(IntPtr scrollView, int width, int height);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern ThreeIntHack ScrollView_getCanvasSize(IntPtr scrollView);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern ThreeIntHack ScrollView_getCanvasPosition(IntPtr scrollView);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollView_setCanvasPosition(IntPtr scrollView, int x, int y);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollView_setFavorVertical(IntPtr scrollView, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ScrollView_getFavorVertical(IntPtr scrollView);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollView_setAllowMouseScroll(IntPtr scrollView, bool value);
        
        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ScrollView_getAllowMouseScroll(IntPtr scrollView);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntCoord ScrollView_getViewCoord(IntPtr scrollView);

#endregion
    }
}

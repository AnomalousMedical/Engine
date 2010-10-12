using System;
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

        public Size2 CanvasSize
        {
            get
            {
                return ScrollView_getCanvasSize(widget).toSize();
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

        public IntCoord ClientCoord
        {
            get
            {
                return ScrollView_getClientCoord(widget);
            }
        }

        public Vector2 CanvasPosition
        {
            get
            {
                return ScrollView_getCanvasPosition(widget).toVector2();
            }
            set
            {
                ScrollView_setCanvasPosition(widget, (int)value.x, (int)value.y);
            }
        }

#region PInvoke

        //ScrollView
        [DllImport("MyGUIWrapper")]
        private static extern void ScrollView_setVisibleVScroll(IntPtr scrollView, bool value);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ScrollView_isVisibleVScroll(IntPtr scrollView);

        [DllImport("MyGUIWrapper")]
        private static extern void ScrollView_setVisibleHScroll(IntPtr scrollView, bool value);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ScrollView_isVisibleHScroll(IntPtr scrollView);

        [DllImport("MyGUIWrapper")]
        private static extern void ScrollView_setCanvasAlign(IntPtr scrollView, Align value);

        [DllImport("MyGUIWrapper")]
        private static extern Align ScrollView_getCanvasAlign(IntPtr scrollView);

        [DllImport("MyGUIWrapper")]
        private static extern void ScrollView_setCanvasSize(IntPtr scrollView, int width, int height);

        [DllImport("MyGUIWrapper")]
        private static extern ThreeIntHack ScrollView_getCanvasSize(IntPtr scrollView);

        [DllImport("MyGUIWrapper")]
        private static extern IntCoord ScrollView_getClientCoord(IntPtr scrollView);

        [DllImport("MyGUIWrapper")]
        private static extern ThreeIntHack ScrollView_getCanvasPosition(IntPtr scrollView);

        [DllImport("MyGUIWrapper")]
        private static extern void ScrollView_setCanvasPosition(IntPtr scrollView, int x, int y);

#endregion
    }
}

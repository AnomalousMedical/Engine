using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class ScrollBar : Widget
    {
        internal ScrollBar(IntPtr vScroll)
            :base(vScroll)
        {
            
        }

        public uint ScrollRange
        {
            get
            {
                return ScrollBar_getScrollRange(widget).ToUInt32();
            }
            set
            {
                ScrollBar_setScrollRange(widget, new UIntPtr(value));
            }
        }

        public uint ScrollPosition
        {
            get
            {
                return ScrollBar_getScrollPosition(widget).ToUInt32();
            }
            set
            {
                ScrollBar_setScrollPosition(widget, new UIntPtr(value));
            }
        }

        public uint ScrollPage
        {
            get
            {
                return ScrollBar_getScrollPage(widget).ToUInt32();
            }
            set
            {
                ScrollBar_setScrollPage(widget, new UIntPtr(value));
            }
        }

        public uint ScrollIncrement
        {
            get
            {
                return ScrollBar_getScrollIncrement(widget).ToUInt32();
            }
            set
            {
                ScrollBar_setScrollIncrement(widget, new UIntPtr(value));
            }
        }

        public uint setScrollViewPage
        {
            get
            {
                return ScrollBar_getScrollViewPage(widget).ToUInt32();
            }
            set
            {
                ScrollBar_setScrollViewPage(widget, new UIntPtr(value));
            }
        }

        public int LineSize
        {
            get
            {
                return ScrollBar_getLineSize(widget);
            }
        }

        public int TrackSize
        {
            get
            {
                return ScrollBar_getTrackSize(widget);
            }
            set
            {
                ScrollBar_setTrackSize(widget, value);
            }
        }

        public int MinTrackSize
        {
            get
            {
                return ScrollBar_getMinTrackSize(widget);
            }
            set
            {
                ScrollBar_setMinTrackSize(widget, value);
            }
        }

        public bool MoveToClick
        {
            get
            {
                return ScrollBar_getMoveToClick(widget);
            }
            set
            {
                ScrollBar_setMoveToClick(widget, value);
            }
        }

        public event MyGUIEvent ScrollChangePosition
        {
            add
            {
                eventManager.addDelegate<ScrollChangePositionET>(value);
            }
            remove
            {
                eventManager.removeDelegate<ScrollChangePositionET>(value);
            }
        }

#region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollBar_setScrollRange(IntPtr vscroll, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr ScrollBar_getScrollRange(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollBar_setScrollPosition(IntPtr vscroll, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr ScrollBar_getScrollPosition(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollBar_setScrollPage(IntPtr vscroll, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr ScrollBar_getScrollPage(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollBar_setScrollIncrement(IntPtr vscroll, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr ScrollBar_getScrollIncrement(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollBar_setScrollViewPage(IntPtr vscroll, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr ScrollBar_getScrollViewPage(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int ScrollBar_getLineSize(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollBar_setTrackSize(IntPtr vscroll, int value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int ScrollBar_getTrackSize(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollBar_setMinTrackSize(IntPtr vscroll, int value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int ScrollBar_getMinTrackSize(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ScrollBar_setMoveToClick(IntPtr vscroll, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ScrollBar_getMoveToClick(IntPtr vscroll);

#endregion
    }
}

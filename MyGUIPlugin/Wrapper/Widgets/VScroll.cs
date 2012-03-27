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
                return VScroll_getScrollRange(widget).ToUInt32();
            }
            set
            {
                VScroll_setScrollRange(widget, new UIntPtr(value));
            }
        }

        public uint ScrollPosition
        {
            get
            {
                return VScroll_getScrollPosition(widget).ToUInt32();
            }
            set
            {
                VScroll_setScrollPosition(widget, new UIntPtr(value));
            }
        }

        public uint ScrollPage
        {
            get
            {
                return VScroll_getScrollPage(widget).ToUInt32();
            }
            set
            {
                VScroll_setScrollPage(widget, new UIntPtr(value));
            }
        }

        public uint ScrollIncrement
        {
            get
            {
                return VScroll_getScrollIncrement(widget).ToUInt32();
            }
            set
            {
                VScroll_setScrollIncrement(widget, new UIntPtr(value));
            }
        }

        public uint setScrollViewPage
        {
            get
            {
                return VScroll_getScrollViewPage(widget).ToUInt32();
            }
            set
            {
                VScroll_setScrollViewPage(widget, new UIntPtr(value));
            }
        }

        public int LineSize
        {
            get
            {
                return VScroll_getLineSize(widget);
            }
        }

        public int TrackSize
        {
            get
            {
                return VScroll_getTrackSize(widget);
            }
            set
            {
                VScroll_setTrackSize(widget, value);
            }
        }

        public int MinTrackSize
        {
            get
            {
                return VScroll_getMinTrackSize(widget);
            }
            set
            {
                VScroll_setMinTrackSize(widget, value);
            }
        }

        public bool MoveToClick
        {
            get
            {
                return VScroll_getMoveToClick(widget);
            }
            set
            {
                VScroll_setMoveToClick(widget, value);
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
        private static extern void VScroll_setScrollRange(IntPtr vscroll, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr VScroll_getScrollRange(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VScroll_setScrollPosition(IntPtr vscroll, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr VScroll_getScrollPosition(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VScroll_setScrollPage(IntPtr vscroll, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr VScroll_getScrollPage(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VScroll_setScrollIncrement(IntPtr vscroll, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr VScroll_getScrollIncrement(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VScroll_setScrollViewPage(IntPtr vscroll, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr VScroll_getScrollViewPage(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int VScroll_getLineSize(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VScroll_setTrackSize(IntPtr vscroll, int value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int VScroll_getTrackSize(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VScroll_setMinTrackSize(IntPtr vscroll, int value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int VScroll_getMinTrackSize(IntPtr vscroll);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void VScroll_setMoveToClick(IntPtr vscroll, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool VScroll_getMoveToClick(IntPtr vscroll);

#endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class VScroll : Widget
    {
        internal VScroll(IntPtr vScroll)
            :base(vScroll)
        {
            
        }

        public void setScrollRange(uint value)
        {
            VScroll_setScrollRange(widget, new UIntPtr(value));
        }

        public uint getScrollRange()
        {
            return VScroll_getScrollRange(widget).ToUInt32();
        }

        public void setScrollPosition(uint value)
        {
            VScroll_setScrollPosition(widget, new UIntPtr(value));
        }

        public uint getScrollPosition()
        {
            return VScroll_getScrollPosition(widget).ToUInt32();
        }

        public void setScrollPage(uint value)
        {
            VScroll_setScrollPage(widget, new UIntPtr(value));
        }

        public uint getScrollPage()
        {
            return VScroll_getScrollPage(widget).ToUInt32();
        }

        public void setScrollViewPage(uint value)
        {
            VScroll_setScrollViewPage(widget, new UIntPtr(value));
        }

        public uint getScrollViewPage()
        {
            return VScroll_getScrollViewPage(widget).ToUInt32();
        }

        public int getLineSize()
        {
            return VScroll_getLineSize(widget);
        }

        public void setTrackSize(int value)
        {
            VScroll_setTrackSize(widget, value);
        }

        public int getTrackSize()
        {
            return VScroll_getTrackSize(widget);
        }

        public void setMinTrackSize(int value)
        {
            VScroll_setMinTrackSize(widget, value);
        }

        public int getMinTrackSize()
        {
            return VScroll_getMinTrackSize(widget);
        }

        public void setMoveToClick(bool value)
        {
            VScroll_setMoveToClick(widget, value);
        }

        public bool getMoveToClick()
        {
            return VScroll_getMoveToClick(widget);
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern void VScroll_setScrollRange(IntPtr vscroll, UIntPtr value);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr VScroll_getScrollRange(IntPtr vscroll);

        [DllImport("MyGUIWrapper")]
        private static extern void VScroll_setScrollPosition(IntPtr vscroll, UIntPtr value);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr VScroll_getScrollPosition(IntPtr vscroll);

        [DllImport("MyGUIWrapper")]
        private static extern void VScroll_setScrollPage(IntPtr vscroll, UIntPtr value);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr VScroll_getScrollPage(IntPtr vscroll);

        [DllImport("MyGUIWrapper")]
        private static extern void VScroll_setScrollViewPage(IntPtr vscroll, UIntPtr value);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr VScroll_getScrollViewPage(IntPtr vscroll);

        [DllImport("MyGUIWrapper")]
        private static extern int VScroll_getLineSize(IntPtr vscroll);

        [DllImport("MyGUIWrapper")]
        private static extern void VScroll_setTrackSize(IntPtr vscroll, int value);

        [DllImport("MyGUIWrapper")]
        private static extern int VScroll_getTrackSize(IntPtr vscroll);

        [DllImport("MyGUIWrapper")]
        private static extern void VScroll_setMinTrackSize(IntPtr vscroll, int value);

        [DllImport("MyGUIWrapper")]
        private static extern int VScroll_getMinTrackSize(IntPtr vscroll);

        [DllImport("MyGUIWrapper")]
        private static extern void VScroll_setMoveToClick(IntPtr vscroll, bool value);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool VScroll_getMoveToClick(IntPtr vscroll);

#endregion
    }
}

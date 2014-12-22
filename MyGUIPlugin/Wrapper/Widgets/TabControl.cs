using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class TabControl : Widget
    {
        public TabControl(IntPtr tabControl)
            : base(tabControl)
        {

        }

        public TabItem addItem(String name)
        {
            return (TabItem)WidgetManager.getWidget(TabControl_addItem(widget, name));
        }

        public TabItem insertItemAt(uint index, String name)
        {
            return (TabItem)WidgetManager.getWidget(TabControl_insertItemAt(widget, new UIntPtr(index), name));
        }

        public void removeItemAt(uint index)
        {
            TabControl_removeItemAt(widget, new UIntPtr(index));
        }        

        public uint IndexSelected
        {
            get
            {
                return TabControl_getIndexSelected(widget).horriblyUnsafeToUInt32();
            }
            set
            {
                TabControl_setIndexSelected(widget, new UIntPtr(value));
            }
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TabControl_removeItemAt(IntPtr tabControl, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TabControl_setIndexSelected(IntPtr tabControl, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern UIntPtr TabControl_getIndexSelected(IntPtr tabControl);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TabControl_insertItemAt(IntPtr tabControl, UIntPtr _index, [MarshalAs(UnmanagedType.LPWStr)] String _name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TabControl_addItem(IntPtr tabControl, [MarshalAs(UnmanagedType.LPWStr)] String _name);

        #endregion
    }
}

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

        public void removeItemAt(uint index)
        {
            TabControl_removeItemAt(widget, new UIntPtr(index));
        }

        public uint IndexSelected
        {
            get
            {
                return TabControl_getIndexSelected(widget).ToUInt32();
            }
            set
            {
                TabControl_setIndexSelected(widget, new UIntPtr(value));
            }
        }

        #region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void TabControl_removeItemAt(IntPtr tabControl, UIntPtr index);

        [DllImport("MyGUIWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void TabControl_setIndexSelected(IntPtr tabControl, UIntPtr index);

        [DllImport("MyGUIWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern UIntPtr TabControl_getIndexSelected(IntPtr tabControl);

        #endregion
    }
}

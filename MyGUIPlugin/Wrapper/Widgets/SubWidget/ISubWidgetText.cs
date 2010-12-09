using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class ISubWidgetText
    {
        private IntPtr subWidgetText;

        internal ISubWidgetText(IntPtr subWidgetText)
        {
            this.subWidgetText = subWidgetText;
        }

        public void setWordWrap(bool value)
        {
            ISubWidgetText_setWordWrap(subWidgetText, value);
        }

        [DllImport("MyGUIWrapper")]
        private static extern void ISubWidgetText_setWordWrap(IntPtr widget, bool value);
    }
}

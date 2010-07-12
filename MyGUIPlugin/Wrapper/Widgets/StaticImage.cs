using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class StaticImage: Widget
    {
        internal StaticImage(IntPtr staticImage)
            : base(staticImage)
        {

        }

        public void setItemResource(String value)
        {
            if (value == null)
            {
                value = "";
            }
            StaticImage_setItemResource(widget, value);
        }

        public void setItemGroup(String value)
        {
            StaticImage_setItemGroup(widget, value);
        }

        public void setItemName(String value)
        {
            StaticImage_setItemName(widget, value);
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern void StaticImage_setItemResource(IntPtr staticImage, String value);

        [DllImport("MyGUIWrapper")]
        private static extern void StaticImage_setItemGroup(IntPtr staticImage, String value);

        [DllImport("MyGUIWrapper")]
        private static extern void StaticImage_setItemName(IntPtr staticImage, String value);

#endregion
    }
}

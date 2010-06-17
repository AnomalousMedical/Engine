using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class Button : Widget
    {
        internal Button(IntPtr button)
            :base(button)
        {

        }

        public void setStateCheck(bool value)
        {
            Button_setStateCheck(widget, value);
        }

        public bool getStateCheck()
        {
            return Button_getStateCheck(widget);
        }

        public void setImageIndex(uint value)
        {
            Button_setImageIndex(widget, new UIntPtr(value));
        }

        public uint getImageIndex()
        {
            return Button_getImageIndex(widget).ToUInt32();
        }

        public void setModeImage(bool value)
        {
            Button_setModeImage(widget, value);
        }

        public bool getModeImage()
        {
            return Button_getModeImage(widget);
        }

        public StaticImage getStaticImage()
        {
            return WidgetManager.getWidget(Button_getStaticImage(widget)) as StaticImage;
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern void Button_setStateCheck(IntPtr button, bool value);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Button_getStateCheck(IntPtr button);

        [DllImport("MyGUIWrapper")]
        private static extern void Button_setImageIndex(IntPtr button, UIntPtr value);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr Button_getImageIndex(IntPtr button);

        [DllImport("MyGUIWrapper")]
        private static extern void Button_setModeImage(IntPtr button, bool value);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Button_getModeImage(IntPtr button);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Button_getStaticImage(IntPtr button);

#endregion
    }
}

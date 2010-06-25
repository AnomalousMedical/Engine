using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class Button : StaticText
    {
        internal Button(IntPtr button)
            :base(button)
        {

        }

        public bool StateCheck
        {
            get
            {
                return Button_getStateCheck(widget);
            }
            set
            {
                Button_setStateCheck(widget, value);
            }
        }

        public uint ImageIndex
        {
            get
            {
                return Button_getImageIndex(widget).ToUInt32();
            }
            set
            {
                Button_setImageIndex(widget, new UIntPtr(value));
            }
        }

        public bool ModeImage
        {
            get
            {
                return Button_getModeImage(widget);
            }
            set
            {
                Button_setModeImage(widget, value);
            }
        }

        public StaticImage StaticImage
        {
            get
            {
                return WidgetManager.getWidget(Button_getStaticImage(widget)) as StaticImage;
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class Button : StaticText
    {
        private StaticImage staticImage = null;

        internal Button(IntPtr button)
            :base(button)
        {

        }

        public bool Selected
        {
            get
            {
                return Button_getStateSelected(widget);
            }
            set
            {
                Button_setStateSelected(widget, value);
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

        public StaticImage ImageBox
        {
            get
            {
                if (staticImage == null)
                {
                    staticImage = WidgetManager.getWidget(Button__getImageBox(widget)) as StaticImage;
                }
                return staticImage;
            }
        }

#region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Button_setStateSelected(IntPtr button, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Button_getStateSelected(IntPtr button);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Button_setModeImage(IntPtr button, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Button_getModeImage(IntPtr button);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Button__getImageBox(IntPtr button);

#endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace MyGUIPlugin
{
    public class StaticText : Widget
    {
        public StaticText(IntPtr staticText)
            :base(staticText)
        {

        }

        public IntCoord getTextRegion()
        {
            return StaticText_getTextRegion(widget);
        }

        public Size2 getTextSize()
        {
            return StaticText_getTextSize(widget).toSize();
        }

        public void setCaptionWithReplacing(String value)
        {
            StaticText_setCaptionWithReplacing(widget, value);
        }

        public String Font
        {
            get
            {
                return Marshal.PtrToStringAnsi(StaticText_getFontName(widget));
            }
            set
            {
                StaticText_setFontName(widget, value);
            }
        }

        public int FontHeight
        {
            get
            {
                return StaticText_getFontHeight(widget);
            }
            set
            {
                StaticText_setFontHeight(widget, value);
            }
        }

        public Align TextAlign
        {
            get
            {
                return StaticText_getTextAlign(widget);
            }
            set
            {
                StaticText_setTextAlign(widget, value);
            }
        }

        public Color TextColor
        {
            get
            {
                return StaticText_getTextColour(widget);
            }
            set
            {
                StaticText_setTextColour(widget, value);
            }
        }

        public String Caption
        {
            get
            {
                return Marshal.PtrToStringUni(StaticText_getCaption(widget));
            }
            set
            {
                if (value != null)
                {
                    StaticText_setCaption(widget, value);
                }
                else
                {
                    StaticText_setCaption(widget, "");
                }
            }
        }

#region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntCoord StaticText_getTextRegion(IntPtr staticText);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern ThreeIntHack StaticText_getTextSize(IntPtr staticText);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void StaticText_setFontName(IntPtr staticText, String font);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr StaticText_getFontName(IntPtr staticText);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void StaticText_setFontHeight(IntPtr staticText, int height);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int StaticText_getFontHeight(IntPtr staticText);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void StaticText_setTextAlign(IntPtr staticText, Align align);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Align StaticText_getTextAlign(IntPtr staticText);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void StaticText_setTextColour(IntPtr staticText, Color colour);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Color StaticText_getTextColour(IntPtr staticText);

        [DllImport("MyGUIWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void StaticText_setCaption(IntPtr staticText, [MarshalAs(UnmanagedType.LPWStr)] String value);

        [DllImport("MyGUIWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr StaticText_getCaption(IntPtr staticText);

        [DllImport("MyGUIWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void StaticText_setCaptionWithReplacing(IntPtr staticText, [MarshalAs(UnmanagedType.LPWStr)] String value);
#endregion
    }
}

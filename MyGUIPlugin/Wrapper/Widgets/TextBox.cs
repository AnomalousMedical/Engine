using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace MyGUIPlugin
{
    public class TextBox : Widget
    {
        public TextBox(IntPtr staticText)
            :base(staticText)
        {

        }

        public IntCoord getTextRegion()
        {
            return TextBox_getTextRegion(widget);
        }

        public Size2 getTextSize()
        {
            return TextBox_getTextSize(widget).toSize();
        }

        public void setCaptionWithReplacing(String value)
        {
            TextBox_setCaptionWithReplacing(widget, value);
        }

        public String Font
        {
            get
            {
                return Marshal.PtrToStringAnsi(TextBox_getFontName(widget));
            }
            set
            {
                TextBox_setFontName(widget, value);
            }
        }

        public int FontHeight
        {
            get
            {
                return TextBox_getFontHeight(widget);
            }
            set
            {
                TextBox_setFontHeight(widget, value);
            }
        }

        public Align TextAlign
        {
            get
            {
                return TextBox_getTextAlign(widget);
            }
            set
            {
                TextBox_setTextAlign(widget, value);
            }
        }

        public Color TextColor
        {
            get
            {
                return TextBox_getTextColour(widget);
            }
            set
            {
                TextBox_setTextColour(widget, value);
            }
        }

        public String Caption
        {
            get
            {
                return Marshal.PtrToStringUni(TextBox_getCaption(widget));
            }
            set
            {
                if (value != null)
                {
                    TextBox_setCaption(widget, value);
                }
                else
                {
                    TextBox_setCaption(widget, "");
                }
            }
        }

#region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntCoord TextBox_getTextRegion(IntPtr staticText);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern ThreeIntHack TextBox_getTextSize(IntPtr staticText);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void TextBox_setFontName(IntPtr staticText, String font);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr TextBox_getFontName(IntPtr staticText);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void TextBox_setFontHeight(IntPtr staticText, int height);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int TextBox_getFontHeight(IntPtr staticText);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void TextBox_setTextAlign(IntPtr staticText, Align align);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Align TextBox_getTextAlign(IntPtr staticText);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void TextBox_setTextColour(IntPtr staticText, Color colour);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Color TextBox_getTextColour(IntPtr staticText);

        [DllImport("MyGUIWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void TextBox_setCaption(IntPtr staticText, [MarshalAs(UnmanagedType.LPWStr)] String value);

        [DllImport("MyGUIWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TextBox_getCaption(IntPtr staticText);

        [DllImport("MyGUIWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void TextBox_setCaptionWithReplacing(IntPtr staticText, [MarshalAs(UnmanagedType.LPWStr)] String value);
#endregion
    }
}

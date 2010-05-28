using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class TextAreaOverlayElement : OverlayElement
    {
        public enum Alignment : uint
	    {
		    Left,
		    Right,
		    Center
	    };

        internal TextAreaOverlayElement(IntPtr overlayElement)
            : base(overlayElement)
        {

        }

        public static String TypeName
        {
            get
            {
                return "TextArea";
            }
        }

        public void setCharHeight(float height)
        {
            TextAreaOverlayElement_setCharHeight(overlayElement, height);
        }

        public float getCharHeight()
        {
            return TextAreaOverlayElement_getCharHeight(overlayElement);
        }

        public void setSpaceWidth(float width)
        {
            TextAreaOverlayElement_setSpaceWidth(overlayElement, width);
        }

        public float getSpaceWidth()
        {
            return TextAreaOverlayElement_getSpaceWidth(overlayElement);
        }

        public void setFontName(String font)
        {
            TextAreaOverlayElement_setFontName(overlayElement, font);
        }

        public String getFontName()
        {
            return Marshal.PtrToStringAnsi(TextAreaOverlayElement_getFontName(overlayElement));
        }

        public void setColorTop(Color color)
        {
            TextAreaOverlayElement_setColorTop(overlayElement, color);
        }

        public Color getColorTop()
        {
            return TextAreaOverlayElement_getColorTop(overlayElement);
        }

        public void setColorBottom(Color color)
        {
            TextAreaOverlayElement_setColorBottom(overlayElement, color);
        }

        public Color getColorBottom()
        {
            return TextAreaOverlayElement_getColorBottom(overlayElement);
        }

        public void setAlignment(Alignment a)
        {
            TextAreaOverlayElement_setAlignment(overlayElement, a);
        }

        public Alignment getAlignment()
        {
            return TextAreaOverlayElement_getAlignment(overlayElement);
        }

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern void TextAreaOverlayElement_setCharHeight(IntPtr textArea, float height);

        [DllImport("OgreCWrapper")]
        private static extern float TextAreaOverlayElement_getCharHeight(IntPtr textArea);

        [DllImport("OgreCWrapper")]
        private static extern void TextAreaOverlayElement_setSpaceWidth(IntPtr textArea, float width);

        [DllImport("OgreCWrapper")]
        private static extern float TextAreaOverlayElement_getSpaceWidth(IntPtr textArea);

        [DllImport("OgreCWrapper")]
        private static extern void TextAreaOverlayElement_setFontName(IntPtr textArea, String font);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr TextAreaOverlayElement_getFontName(IntPtr textArea);

        [DllImport("OgreCWrapper")]
        private static extern void TextAreaOverlayElement_setColor(IntPtr textArea, Color color);

        [DllImport("OgreCWrapper")]
        private static extern Color TextAreaOverlayElement_getColor(IntPtr textArea);

        [DllImport("OgreCWrapper")]
        private static extern void TextAreaOverlayElement_setColorTop(IntPtr textArea, Color color);

        [DllImport("OgreCWrapper")]
        private static extern Color TextAreaOverlayElement_getColorTop(IntPtr textArea);

        [DllImport("OgreCWrapper")]
        private static extern void TextAreaOverlayElement_setColorBottom(IntPtr textArea, Color color);

        [DllImport("OgreCWrapper")]
        private static extern Color TextAreaOverlayElement_getColorBottom(IntPtr textArea);

        [DllImport("OgreCWrapper")]
        private static extern void TextAreaOverlayElement_setAlignment(IntPtr textArea, Alignment a);

        [DllImport("OgreCWrapper")]
        private static extern Alignment TextAreaOverlayElement_getAlignment(IntPtr textArea);

#endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    class TextAreaOverlayElement : OverlayElement
    {
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
    }
}

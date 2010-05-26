using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    class PanelOverlayElement : OverlayElement
    {
        internal PanelOverlayElement(IntPtr overlayElement)
            : base(overlayElement)
        {

        }

        public static String TypeName
        {
            get
            {
                return "Panel";
            }
        }
    }
}

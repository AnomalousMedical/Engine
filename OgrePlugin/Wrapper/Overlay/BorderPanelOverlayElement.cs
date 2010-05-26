using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public class BorderPanelOverlayElement : OverlayElement
    {
        internal BorderPanelOverlayElement(IntPtr overlayElement)
            : base(overlayElement)
        {

        }

        public static String TypeName
	    {
		    get
		    {
			    return "BorderPanel";
		    }
	    }
    }
}

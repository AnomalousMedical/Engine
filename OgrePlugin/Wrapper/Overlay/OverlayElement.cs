using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    public class OverlayElement : IDisposable
    {
        internal static OverlayElement createWrapper(IntPtr nativeObject, object[] args)
        {
	        String type = Marshal.PtrToStringAnsi(OverlayElement_getTypeName(nativeObject));
	        if(type.Equals(TextAreaOverlayElement.TypeName))
	        {
		        return new TextAreaOverlayElement(nativeObject);
	        }
	        else if(type.Equals(PanelOverlayElement.TypeName))
	        {
		        return new PanelOverlayElement(nativeObject);
	        }
	        else if(type.Equals(BorderPanelOverlayElement.TypeName))
	        {
		        return new BorderPanelOverlayElement(nativeObject);
	        }
	        throw new NotImplementedException();
        }

        protected IntPtr overlayElement;

        protected OverlayElement(IntPtr overlayElement)
        {
            this.overlayElement = overlayElement;
        }

        public void Dispose()
        {
            overlayElement = IntPtr.Zero;
        }

        internal IntPtr OgreObject
        {
            get
            {
                return overlayElement;
            }
        }

#region  PInvoke
        [DllImport("OgreCWrapper")]
        private static extern IntPtr OverlayElement_getTypeName(IntPtr overlayElement);
#endregion
    }
}

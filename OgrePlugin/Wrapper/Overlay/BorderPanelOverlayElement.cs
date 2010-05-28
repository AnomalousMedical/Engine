using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class BorderPanelOverlayElement : PanelOverlayElement
    {
        internal BorderPanelOverlayElement(IntPtr overlayElement)
            : base(overlayElement)
        {

        }

        public new static String TypeName
	    {
		    get
		    {
			    return "BorderPanel";
		    }
	    }

        public void setBorderSize(float size)
        {
            BorderPanelOverlayElement_setBorderSize(overlayElement, size);
        }

        public void setBorderSize(float sides, float topAndBottom)
        {
            BorderPanelOverlayElement_setBorderSizeSidesTB(overlayElement, sides, topAndBottom);
        }

        public void setBorderSize(float left, float right, float top, float bottom)
        {
            BorderPanelOverlayElement_setBorderSizeExact(overlayElement, left, right, top, bottom);
        }

        public float getLeftBorderSize()
        {
            return BorderPanelOverlayElement_getLeftBorderSize(overlayElement);
        }

        public float getRightBorderSize()
        {
            return BorderPanelOverlayElement_getRightBorderSize(overlayElement);
        }

        public float getTopBorderSize()
        {
            return BorderPanelOverlayElement_getTopBorderSize(overlayElement);
        }

        public float getBottomBorderSize()
        {
            return BorderPanelOverlayElement_getBottomBorderSize(overlayElement);
        }

        public void setLeftBorderUV(float u1, float v1, float u2, float v2)
        {
            BorderPanelOverlayElement_setLeftBorderUV(overlayElement, u1, v1, u2, v2);
        }

        public void setRightBorderUV(float u1, float v1, float u2, float v2)
        {
            BorderPanelOverlayElement_setRightBorderUV(overlayElement, u1, v1, u2, v2);
        }

        public void setTopBorderUV(float u1, float v1, float u2, float v2)
        {
            BorderPanelOverlayElement_setTopBorderUV(overlayElement, u1, v1, u2, v2);
        }

        public void setBottomBorderUV(float u1, float v1, float u2, float v2)
        {
            BorderPanelOverlayElement_setBottomBorderUV(overlayElement, u1, v1, u2, v2);
        }

        public void setTopLeftBorderUV(float u1, float v1, float u2, float v2)
        {
            BorderPanelOverlayElement_setTopLeftBorderUV(overlayElement, u1, v1, u2, v2);
        }

        public void setTopRightBorderUV(float u1, float v1, float u2, float v2)
        {
            BorderPanelOverlayElement_setTopRightBorderUV(overlayElement, u1, v1, u2, v2);
        }

        public void setBottomLeftBorderUV(float u1, float v1, float u2, float v2)
        {
            BorderPanelOverlayElement_setBottomLeftBorderUV(overlayElement, u1, v1, u2, v2);
        }

        public void setBottomRightBorderUV(float u1, float v1, float u2, float v2)
        {
            BorderPanelOverlayElement_setBottomRightBorderUV(overlayElement, u1, v1, u2, v2);
        }

        public void setBorderMaterialName(String name)
        {
            BorderPanelOverlayElement_setBorderMaterialName(overlayElement, name);
        }

        public String getBorderMaterialName()
        {
            return Marshal.PtrToStringAnsi(BorderPanelOverlayElement_getBorderMaterialName(overlayElement));
        }

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern void BorderPanelOverlayElement_setBorderSize(IntPtr borderPanel, float size);

        [DllImport("OgreCWrapper")]
        private static extern void BorderPanelOverlayElement_setBorderSizeSidesTB(IntPtr borderPanel, float sides, float topAndBottom);

        [DllImport("OgreCWrapper")]
        private static extern void BorderPanelOverlayElement_setBorderSizeExact(IntPtr borderPanel, float left, float right, float top, float bottom);

        [DllImport("OgreCWrapper")]
        private static extern float BorderPanelOverlayElement_getLeftBorderSize(IntPtr borderPanel);

        [DllImport("OgreCWrapper")]
        private static extern float BorderPanelOverlayElement_getRightBorderSize(IntPtr borderPanel);

        [DllImport("OgreCWrapper")]
        private static extern float BorderPanelOverlayElement_getTopBorderSize(IntPtr borderPanel);

        [DllImport("OgreCWrapper")]
        private static extern float BorderPanelOverlayElement_getBottomBorderSize(IntPtr borderPanel);

        [DllImport("OgreCWrapper")]
        private static extern void BorderPanelOverlayElement_setLeftBorderUV(IntPtr borderPanel, float u1, float v1, float u2, float v2);

        [DllImport("OgreCWrapper")]
        private static extern void BorderPanelOverlayElement_setRightBorderUV(IntPtr borderPanel, float u1, float v1, float u2, float v2);

        [DllImport("OgreCWrapper")]
        private static extern void BorderPanelOverlayElement_setTopBorderUV(IntPtr borderPanel, float u1, float v1, float u2, float v2);

        [DllImport("OgreCWrapper")]
        private static extern void BorderPanelOverlayElement_setBottomBorderUV(IntPtr borderPanel, float u1, float v1, float u2, float v2);

        [DllImport("OgreCWrapper")]
        private static extern void BorderPanelOverlayElement_setTopLeftBorderUV(IntPtr borderPanel, float u1, float v1, float u2, float v2);

        [DllImport("OgreCWrapper")]
        private static extern void BorderPanelOverlayElement_setTopRightBorderUV(IntPtr borderPanel, float u1, float v1, float u2, float v2);

        [DllImport("OgreCWrapper")]
        private static extern void BorderPanelOverlayElement_setBottomLeftBorderUV(IntPtr borderPanel, float u1, float v1, float u2, float v2);

        [DllImport("OgreCWrapper")]
        private static extern void BorderPanelOverlayElement_setBottomRightBorderUV(IntPtr borderPanel, float u1, float v1, float u2, float v2);

        [DllImport("OgreCWrapper")]
        private static extern void BorderPanelOverlayElement_setBorderMaterialName(IntPtr borderPanel, String name);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr BorderPanelOverlayElement_getBorderMaterialName(IntPtr borderPanel);

#endregion
    }
}

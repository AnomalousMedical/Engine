using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class PanelOverlayElement : OverlayContainer
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

        public void setTiling(float x, float y)
        {
            PanelOverlayElement_setTiling(overlayElement, x, y);
        }

        public void setTiling(float x, float y, ushort layer)
        {
            PanelOverlayElement_setTilingLayer(overlayElement, x, y, layer);
        }

        public float getTileX()
        {
            return PanelOverlayElement_getTileX(overlayElement);
        }

        public float getTileX(ushort layer)
        {
            return PanelOverlayElement_getTileXLayer(overlayElement, layer);
        }

        public float getTileY()
        {
            return PanelOverlayElement_getTileY(overlayElement);
        }

        public float getTileY(ushort layer)
        {
            return PanelOverlayElement_getTileYLayer(overlayElement, layer);
        }

        public void setUV(float u1, float v1, float u2, float v2)
        {
            PanelOverlayElement_setUV(overlayElement, u1, v1, u2, v2);
        }

        public void getUV(out float u1, out float v1, out float u2, out float v2)
        {
            PanelOverlayElement_getUV(overlayElement, out u1, out v1, out u2, out v2);
        }

        public void setTransparent(bool isTransparent)
        {
            PanelOverlayElement_setTransparent(overlayElement, isTransparent);
        }

        public bool isTransparent()
        {
            return PanelOverlayElement_isTransparent(overlayElement);
        }

#region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void PanelOverlayElement_setTiling(IntPtr panelOverlayElement, float x, float y);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void PanelOverlayElement_setTilingLayer(IntPtr panelOverlayElement, float x, float y, ushort layer);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float PanelOverlayElement_getTileX(IntPtr panelOverlayElement);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float PanelOverlayElement_getTileXLayer(IntPtr panelOverlayElement, ushort layer);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float PanelOverlayElement_getTileY(IntPtr panelOverlayElement);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float PanelOverlayElement_getTileYLayer(IntPtr panelOverlayElement, ushort layer);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void PanelOverlayElement_setUV(IntPtr panelOverlayElement, float u1, float v1, float u2, float v2);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void PanelOverlayElement_getUV(IntPtr panelOverlayElement, out float u1, out float v1, out float u2, out float v2);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void PanelOverlayElement_setTransparent(IntPtr panelOverlayElement, bool isTransparent);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool PanelOverlayElement_isTransparent(IntPtr panelOverlayElement);

#endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    public class Overlay : IDisposable
    {
        internal static Overlay createWrapper(IntPtr nativeObject, object[] args)
        {
            return new Overlay(nativeObject);
        }

        protected IntPtr overlay;

        protected Overlay(IntPtr overlay)
        {
            this.overlay = overlay;
        }

        public void Dispose()
        {
            overlay = IntPtr.Zero;
        }

        public IntPtr OgreObject
        {
            get
            {
                return overlay;
            }
        }

        public OverlayContainer getChild(String name)
        {
            return OverlayManager.getInstance().getObject(Overlay_getChild(overlay, name)) as OverlayContainer;
        }

        public String getName()
        {
            return Marshal.PtrToStringAnsi(Overlay_getName(overlay));
        }

        public void setZOrder(ushort zOrder)
        {
            Overlay_setZOrder(overlay, zOrder);
        }

        public ushort getZOrder()
        {
            return Overlay_getZOrder(overlay);
        }

        public bool isVisible()
        {
            return Overlay_isVisible(overlay);
        }

        public bool isInitialized()
        {
            return Overlay_isInitialized(overlay);
        }

        public void show()
        {
            Overlay_show(overlay);
        }

        public void hide()
        {
            Overlay_hide(overlay);
        }

        public void add2d(OverlayContainer cont)
        {
            Overlay_add2d(overlay, cont.OgreObject);
        }

        public void remove2d(OverlayContainer cont)
        {
            Overlay_remove2d(overlay, cont.OgreObject);
        }

        public void add3d(SceneNode node)
        {
            Overlay_add3d(overlay, node.OgreNode);
        }

        public void remove3d(SceneNode node)
        {
            Overlay_remove3d(overlay, node.OgreNode);
        }

        public void clear()
        {
            Overlay_clear(overlay);
        }

        public void setScroll(float x, float y)
        {
            Overlay_setScroll(overlay, x, y);
        }

        public float getScrollX()
        {
            return Overlay_getScrollX(overlay);
        }

        public float getScrollY()
        {
            return Overlay_getScrollY(overlay);
        }

        public void scroll(float xOff, float yOff)
        {
            Overlay_scroll(overlay, xOff, yOff);
        }

        public void setRotate(float radAngle)
        {
            Overlay_setRotate(overlay, radAngle);
        }

        public float getRotate()
        {
            return Overlay_getRotate(overlay);
        }

        public void rotate(float radAngle)
        {
            Overlay_rotate(overlay, radAngle);
        }

        public void setScale(float x, float y)
        {
            Overlay_setScale(overlay, x, y);
        }

        public float getScaleX()
        {
            return Overlay_getScaleX(overlay);
        }

        public float getScaleY()
        {
            return Overlay_getScaleY(overlay);
        }

#region  PInvoke
        
        [DllImport("OgreCWrapper")]
        private static extern IntPtr Overlay_getChild(IntPtr overlay, String name);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Overlay_getName(IntPtr overlay);

        [DllImport("OgreCWrapper")]
        private static extern void Overlay_setZOrder(IntPtr overlay, ushort zOrder);

        [DllImport("OgreCWrapper")]
        private static extern ushort Overlay_getZOrder(IntPtr overlay);

        [DllImport("OgreCWrapper")]
        private static extern bool Overlay_isVisible(IntPtr overlay);

        [DllImport("OgreCWrapper")]
        private static extern bool Overlay_isInitialized(IntPtr overlay);

        [DllImport("OgreCWrapper")]
        private static extern void Overlay_show(IntPtr overlay);

        [DllImport("OgreCWrapper")]
        private static extern void Overlay_hide(IntPtr overlay);

        [DllImport("OgreCWrapper")]
        private static extern void Overlay_add2d(IntPtr overlay, IntPtr cont);

        [DllImport("OgreCWrapper")]
        private static extern void Overlay_remove2d(IntPtr overlay, IntPtr cont);

        [DllImport("OgreCWrapper")]
        private static extern void Overlay_add3d(IntPtr overlay, IntPtr node);

        [DllImport("OgreCWrapper")]
        private static extern void Overlay_remove3d(IntPtr overlay, IntPtr node);

        [DllImport("OgreCWrapper")]
        private static extern void Overlay_clear(IntPtr overlay);

        [DllImport("OgreCWrapper")]
        private static extern void Overlay_setScroll(IntPtr overlay, float x, float y);

        [DllImport("OgreCWrapper")]
        private static extern float Overlay_getScrollX(IntPtr overlay);

        [DllImport("OgreCWrapper")]
        private static extern float Overlay_getScrollY(IntPtr overlay);

        [DllImport("OgreCWrapper")]
        private static extern void Overlay_scroll(IntPtr overlay, float xOff, float yOff);

        [DllImport("OgreCWrapper")]
        private static extern void Overlay_setRotate(IntPtr overlay, float radAngle);

        [DllImport("OgreCWrapper")]
        private static extern float Overlay_getRotate(IntPtr overlay);

        [DllImport("OgreCWrapper")]
        private static extern void Overlay_rotate(IntPtr overlay, float radAngle);

        [DllImport("OgreCWrapper")]
        private static extern void Overlay_setScale(IntPtr overlay, float x, float y);

        [DllImport("OgreCWrapper")]
        private static extern float Overlay_getScaleX(IntPtr overlay);

        [DllImport("OgreCWrapper")]
        private static extern float Overlay_getScaleY(IntPtr overlay);

#endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace OgreWrapper
{
    public class Viewport : IDisposable
    {
        internal static Viewport createWrapper(IntPtr nativePtr, object[] args)
        {
            return new Viewport(nativePtr);
        }

        IntPtr viewport;

        private Viewport(IntPtr viewport)
        {
            this.viewport = viewport;
        }

        public void Dispose()
        {
            viewport = IntPtr.Zero;
        }

        internal IntPtr OgreViewport
        {
            get
            {
                return viewport;
            }
        }

        /// <summary>
	    /// The visibility mask is a way to exclude objects from rendering for a given viewport. 
	    /// For each object in the frustum, a check is made between this mask and the objects 
	    /// visibility flags.
	    /// </summary>
	    /// <param name="mask">The mask to set.</param>
	    public void setVisibilityMask(uint mask)
        {
            Viewport_setVisibilityMask(viewport, mask);
        }

	    /// <summary>
	    /// Gets a per-viewport visibility mask.
	    /// </summary>
	    /// <returns>The visibility mask.</returns>
        public uint getVisibilityMask()
        {
            return Viewport_getVisibilityMask(viewport);
        }

        public void setBackgroundColor(Color color)
        {
            Viewport_setBackgroundColor(viewport, color);
        }

        public Color getBackgroundColor()
        {
            return Viewport_getBackgroundColor(viewport);
        }

        public float getLeft()
        {
            return Viewport_getLeft(viewport);
        }

        public float getTop()
        {
            return Viewport_getTop(viewport);
        }

        public float getWidth()
        {
            return Viewport_getWidth(viewport);
        }

        public float getHeight()
        {
            return Viewport_getHeight(viewport);
        }

        public int getActualLeft()
        {
            return Viewport_getActualLeft(viewport);
        }

        public int getActualTop()
        {
            return Viewport_getActualTop(viewport);
        }

        public int getActualWidth()
        {
            return Viewport_getActualWidth(viewport);
        }

        public int getActualHeight()
        {
            return Viewport_getActualHeight(viewport);
        }

        public void setDimensions(float left, float top, float width, float height)
        {
            Viewport_setDimensions(viewport, left, top, width, height);
        }

        public void setClearEveryFrame(bool clear)
        {
            Viewport_setClearEveryFrame(viewport, clear);
        }

        public bool getClearEveryFrame()
        {
            return Viewport_getClearEveryFrame(viewport);
        }

        public void setMaterialScheme(String schemeName)
        {
            Viewport_setMaterialScheme(viewport, schemeName);
        }

        public String getMaterialScheme()
        {
            return Marshal.PtrToStringAnsi(Viewport_getMaterialScheme(viewport));
        }

        public void setOverlaysEnabled(bool enabled)
        {
            Viewport_setOverlaysEnabled(viewport, enabled);
        }

        public bool getOverlaysEnabled()
        {
            return Viewport_getOverlaysEnabled(viewport);
        }

        public void setSkiesEnabled(bool enabled)
        {
            Viewport_setSkiesEnabled(viewport, enabled);
        }

        public bool getSkiesEnabled()
        {
            return Viewport_getSkiesEnabled(viewport);
        }

        public void setShadowsEnabled(bool enabled)
        {
            Viewport_setShadowsEnabled(viewport, enabled);
        }

        public bool getShadowsEnabled()
        {
            return Viewport_getShadowsEnabled(viewport);
        }

        public void setRenderQueueInvocationSequenceName(String sequenceName)
        {
            Viewport_setRenderQueueInvocationSequenceName(viewport, sequenceName);
        }

        public String getRenderQueueInvocationSequenceName()
        {
            return Marshal.PtrToStringAnsi(Viewport_getRenderQueueInvocationSequenceName(viewport));
        }

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern void Viewport_setVisibilityMask(IntPtr viewport, uint mask);

        [DllImport("OgreCWrapper")]
        private static extern uint Viewport_getVisibilityMask(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern void Viewport_setBackgroundColor(IntPtr viewport, Color color);

        [DllImport("OgreCWrapper")]
        private static extern Color Viewport_getBackgroundColor(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern float Viewport_getLeft(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern float Viewport_getTop(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern float Viewport_getWidth(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern float Viewport_getHeight(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern int Viewport_getActualLeft(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern int Viewport_getActualTop(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern int Viewport_getActualWidth(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern int Viewport_getActualHeight(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern void Viewport_setDimensions(IntPtr viewport, float left, float top, float width, float height);

        [DllImport("OgreCWrapper")]
        private static extern void Viewport_setClearEveryFrame(IntPtr viewport, bool clear);

        [DllImport("OgreCWrapper")]
        private static extern bool Viewport_getClearEveryFrame(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern void Viewport_setMaterialScheme(IntPtr viewport, String schemeName);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Viewport_getMaterialScheme(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern void Viewport_setOverlaysEnabled(IntPtr viewport, bool enabled);

        [DllImport("OgreCWrapper")]
        private static extern bool Viewport_getOverlaysEnabled(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern void Viewport_setSkiesEnabled(IntPtr viewport, bool enabled);

        [DllImport("OgreCWrapper")]
        private static extern bool Viewport_getSkiesEnabled(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern void Viewport_setShadowsEnabled(IntPtr viewport, bool enabled);

        [DllImport("OgreCWrapper")]
        private static extern bool Viewport_getShadowsEnabled(IntPtr viewport);

        [DllImport("OgreCWrapper")]
        private static extern void Viewport_setRenderQueueInvocationSequenceName(IntPtr viewport, String sequenceName);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Viewport_getRenderQueueInvocationSequenceName(IntPtr viewport);

#endregion
    }
}

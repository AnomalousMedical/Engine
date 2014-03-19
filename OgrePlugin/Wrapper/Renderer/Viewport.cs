using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using Engine.Attributes;

namespace OgreWrapper
{
    public enum FrameBufferType
    {
        FBT_COLOUR = 0x1,
        FBT_DEPTH = 0x2,
        FBT_STENCIL = 0x4
    }

    [NativeSubsystemType]
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

        public void setClearEveryFrame(bool clear, FrameBufferType buffers = FrameBufferType.FBT_COLOUR | FrameBufferType.FBT_DEPTH)
        {
            Viewport_setClearEveryFrame(viewport, clear, (uint)buffers);
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

        public Camera getCamera()
        {
            return Camera.resolvePointer(Viewport_getCamera(viewport));
        }

        public void clear()
        {
            Viewport_clear1(viewport);
        }

        public void clear(FrameBufferType buffers)
        {
            Viewport_clear2(viewport, buffers);
        }

        public void clear(FrameBufferType buffers, Color color)
        {
            Viewport_clear3(viewport, buffers, color);
        }

        public void clear(FrameBufferType buffers, Color color, float depth)
        {
            Viewport_clear4(viewport, buffers, color, depth);
        }

        public void clear(FrameBufferType buffers, Color color, float depth, ushort stencil)
        {
            Viewport_clear5(viewport, buffers, color, depth, stencil);
        }

        public bool AutoUpdated
        {
            get
            {
                return Viewport_isAutoUpdated(viewport);
            }
            set
            {
                Viewport_setAutoUpdated(viewport, value);
            }
        }

#region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Viewport_setVisibilityMask(IntPtr viewport, uint mask);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern uint Viewport_getVisibilityMask(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Viewport_setBackgroundColor(IntPtr viewport, Color color);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Color Viewport_getBackgroundColor(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Viewport_getLeft(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Viewport_getTop(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Viewport_getWidth(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Viewport_getHeight(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int Viewport_getActualLeft(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int Viewport_getActualTop(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int Viewport_getActualWidth(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int Viewport_getActualHeight(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Viewport_setDimensions(IntPtr viewport, float left, float top, float width, float height);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Viewport_setClearEveryFrame(IntPtr viewport, bool clear, uint buffers);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Viewport_getClearEveryFrame(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Viewport_setMaterialScheme(IntPtr viewport, String schemeName);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Viewport_getMaterialScheme(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Viewport_setOverlaysEnabled(IntPtr viewport, bool enabled);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Viewport_getOverlaysEnabled(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Viewport_setSkiesEnabled(IntPtr viewport, bool enabled);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Viewport_getSkiesEnabled(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Viewport_setShadowsEnabled(IntPtr viewport, bool enabled);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Viewport_getShadowsEnabled(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Viewport_setRenderQueueInvocationSequenceName(IntPtr viewport, String sequenceName);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Viewport_getRenderQueueInvocationSequenceName(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Viewport_getCamera(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Viewport_clear1(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Viewport_clear2(IntPtr viewport, FrameBufferType buffers);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Viewport_clear3(IntPtr viewport, FrameBufferType buffers, Color color);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Viewport_clear4(IntPtr viewport, FrameBufferType buffers, Color color, float depth);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Viewport_clear5(IntPtr viewport, FrameBufferType buffers, Color color, float depth, ushort stencil);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Viewport_isAutoUpdated(IntPtr viewport);

        [DllImport("OgreCWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Viewport_setAutoUpdated(IntPtr viewport, bool autoUpdate);

#endregion
    }
}

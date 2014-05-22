using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace OgreWrapper
{
    public class RenderSystem : IDisposable
    {
        internal static RenderSystem createWrapper(IntPtr nativePtr, object[] args)
        {
            return new RenderSystem(nativePtr);
        }

        IntPtr renderSystem;

        internal IntPtr OgreRenderSystem
        {
            get
            {
                return renderSystem;
            }
        }

        public RenderSystem(IntPtr renderSystem)
        {
            this.renderSystem = renderSystem;
        }

        public void Dispose()
        {
            renderSystem = IntPtr.Zero;
        }

        /// <summary>
	    /// Validates the options set for the rendering system, returning a message
        /// if there are problems. 
	    /// </summary>
	    /// <returns>An error message or an empty string if there are no problems.</returns>
        public String validateConfigOptions()
        {
            return RenderSystem_validateConfigOptions(renderSystem);
        }

        public bool hasConfigOption(String name)
        {
            return RenderSystem_hasConfigOption(renderSystem, name);
        }

        public ConfigOption getConfigOption(String name)
        {
            ConfigOption option = new ConfigOption();
            RenderSystem_getConfigOptionInfo(renderSystem, name, option.SetInfo, option.AddValue);
            return option;
        }

        public void _initRenderTargets()
        {
            RenderSystem__initRenderTargets(renderSystem);
        }

        public void setConfigOption(String name, String value)
        {
            RenderSystem_setConfigOption(renderSystem, name, value);
        }

        public void _setViewMatrix(Matrix4x4 view)
        {
            RenderSystem__setViewMatrix(renderSystem, view);
        }

        public void _setProjectionMatrix(Matrix4x4 projection)
        {
            RenderSystem__setProjectionMatrix(renderSystem, projection);
        }

        public void _setViewport(Viewport vp)
        {
            RenderSystem__setViewport(renderSystem, vp.OgreViewport);
        }

        public void addListener(RenderSystemListener listener)
        {
            RenderSystem_addListener(renderSystem, listener.Ptr);
        }

        public void removeListener(RenderSystemListener listener)
        {
            RenderSystem_removeListener(renderSystem, listener.Ptr);
        }

        public void clearFrameBuffer(FrameBufferType buffers, Color color, float depth = 1.0f, ushort stencil = 0)
        {
            RenderSystem_clearFrameBuffer(renderSystem, (uint)buffers, color, depth, stencil);
        }

        public void _setRenderTarget(RenderTarget target)
        {
            RenderSystem__setRenderTarget(renderSystem, target.OgreRenderTarget);
        }

        public String Name
        {
            get
            {
                return Marshal.PtrToStringAnsi(RenderSystem_getName(renderSystem));
            }
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern String RenderSystem_validateConfigOptions(IntPtr renderSystem);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderSystem__initRenderTargets(IntPtr renderSystem);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderSystem_setConfigOption(IntPtr renderSystem, String name, String value);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderSystem__setViewMatrix(IntPtr renderSystem, Matrix4x4 view);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderSystem__setProjectionMatrix(IntPtr renderSystem, Matrix4x4 projection);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderSystem__setViewport(IntPtr renderSystem, IntPtr vp);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool RenderSystem_hasConfigOption(IntPtr renderSystem, String option);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderSystem_getConfigOptionInfo(IntPtr renderSystem, String option, SetConfigInfo setInfo, AddPossibleValue addValues);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderSystem_addListener(IntPtr renderSystem, IntPtr listener);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderSystem_removeListener(IntPtr renderSystem, IntPtr listener);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr RenderSystem_getName(IntPtr renderSystem);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderSystem_clearFrameBuffer(IntPtr renderSystem, uint buffers, Color color, float depth, ushort stencil);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderSystem__setRenderTarget(IntPtr renderSystem, IntPtr target);

        #endregion 
    }
}

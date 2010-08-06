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

        #region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern String RenderSystem_validateConfigOptions(IntPtr renderSystem);

        [DllImport("OgreCWrapper")]
        private static extern void RenderSystem__initRenderTargets(IntPtr renderSystem);

        [DllImport("OgreCWrapper")]
        private static extern void RenderSystem_setConfigOption(IntPtr renderSystem, String name, String value);

        [DllImport("OgreCWrapper")]
        private static extern void RenderSystem__setViewMatrix(IntPtr renderSystem, Matrix4x4 view);

        [DllImport("OgreCWrapper")]
        private static extern void RenderSystem__setProjectionMatrix(IntPtr renderSystem, Matrix4x4 projection);

        [DllImport("OgreCWrapper")]
        private static extern void RenderSystem__setViewport(IntPtr renderSystem, IntPtr vp);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool RenderSystem_hasConfigOption(IntPtr renderSystem, String option);

        [DllImport("OgreCWrapper")]
        private static extern void RenderSystem_getConfigOptionInfo(IntPtr renderSystem, String option, SetConfigInfo setInfo, AddPossibleValue addValues);

        #endregion 
    }
}

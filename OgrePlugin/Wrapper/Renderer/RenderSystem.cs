using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

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

        public void _initRenderTargets()
        {
            RenderSystem__initRenderTargets(renderSystem);
        }

        public void setConfigOption(String name, String value)
        {
            RenderSystem_setConfigOption(renderSystem, name, value);
        }

        #region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern String RenderSystem_validateConfigOptions(IntPtr renderSystem);

        [DllImport("OgreCWrapper")]
        private static extern void RenderSystem__initRenderTargets(IntPtr renderSystem);

        [DllImport("OgreCWrapper")]
        private static extern void RenderSystem_setConfigOption(IntPtr renderSystem, String name, String value);

        #endregion 
    }
}

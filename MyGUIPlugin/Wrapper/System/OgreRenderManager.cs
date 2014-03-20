using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OgreWrapper;

namespace MyGUIPlugin
{
    public class OgreRenderManager : IDisposable
    {
        IntPtr renderManager;

        internal OgreRenderManager(IntPtr renderManager)
        {
            this.renderManager = renderManager;
        }

        public void Dispose()
        {
            renderManager = IntPtr.Zero;
        }

        public void windowResized(int windowWidth, int windowHeight)
        {
            OgreRenderManager_windowResized(renderManager, windowWidth, windowHeight);
        }

        public void destroyTexture(String name)
        {
            OgreRenderManager_destroyTextureString(renderManager, name);
        }

        internal void update()
        {
            OgreRenderManager_update(renderManager);
        }

#region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgreRenderManager_windowResized(IntPtr renderManager, int windowWidth, int windowHeight);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgreRenderManager_destroyTextureString(IntPtr renderManager, String name);

        [DllImport("MyGUIWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void OgreRenderManager_update(IntPtr renderManager);

#endregion
    }
}

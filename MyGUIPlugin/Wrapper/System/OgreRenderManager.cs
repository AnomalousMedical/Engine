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

        public void setRenderWindow(RenderWindow window)
        {
            OgreRenderManager_setRenderWindow(renderManager, window.OgreRenderTarget);
        }

        public void setSceneManager(SceneManager scene)
        {
            OgreRenderManager_setSceneManager(renderManager, scene != null ? scene.OgreSceneManager : IntPtr.Zero);
        }

        public uint getActiveViewport()
        {
            return OgreRenderManager_getActiveViewport(renderManager).ToUInt32();
        }

        public void setActiveViewport(uint num)
        {
            OgreRenderManager_setActiveViewport(renderManager, new UIntPtr(num));
        }

        public void windowMovedOrResized()
        {
            OgreRenderManager_windowMovedOrResized(renderManager);
        }

        public void destroyTexture(String name)
        {
            OgreRenderManager_destroyTextureString(renderManager, name);
        }

#region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgreRenderManager_setRenderWindow(IntPtr renderManager, IntPtr window);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgreRenderManager_setSceneManager(IntPtr renderManager, IntPtr scene);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr OgreRenderManager_getActiveViewport(IntPtr renderManager);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgreRenderManager_setActiveViewport(IntPtr renderManager, UIntPtr num);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgreRenderManager_windowMovedOrResized(IntPtr renderManager);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgreRenderManager_destroyTextureString(IntPtr renderManager, String name);

#endregion
    }
}

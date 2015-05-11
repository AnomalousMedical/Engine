using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OgrePlugin;

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

        public ulong BatchCount
        {
            get
            {
                return OgreRenderManager_getBatchCount(renderManager).ToUInt64();
            }
        }

#region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgreRenderManager_windowResized(IntPtr renderManager, int windowWidth, int windowHeight);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgreRenderManager_destroyTextureString(IntPtr renderManager, String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OgreRenderManager_update(IntPtr renderManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern UIntPtr OgreRenderManager_getBatchCount(IntPtr renderManager);

#endregion
    }
}

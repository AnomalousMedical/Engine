using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class OgrePlatform : IDisposable
    {
        IntPtr ogrePlatform;
        OgreRenderManager renderManager;

        public OgrePlatform()
        {
            ogrePlatform = OgrePlatform_Create();
            renderManager = new OgreRenderManager(OgrePlatform_getRenderManagerPtr(ogrePlatform));
        }

        public void Dispose()
        {
            renderManager.Dispose();
            OgrePlatform_Delete(ogrePlatform);
        }

        public OgreRenderManager getRenderManager()
        {
            return renderManager;
        }

        public void initialize(RenderWindow window, SceneManager sceneManager, String resourceGroup)
        {
            OgrePlatform_initialize(ogrePlatform, window.OgreRenderTarget, sceneManager.OgreSceneManager, resourceGroup);
        }

        public void shutdown()
        {
            OgrePlatform_shutdown(ogrePlatform);
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr OgrePlatform_Create();

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr OgrePlatform_getRenderManagerPtr(IntPtr ogrePlatform);

        [DllImport("MyGUIWrapper")]
        private static extern void OgrePlatform_Delete(IntPtr ogrePlatform);

        [DllImport("MyGUIWrapper")]
        private static extern void OgrePlatform_initialize(IntPtr ogrePlatform, IntPtr renderWindow, IntPtr sceneManager, String resourceGroup);

        [DllImport("MyGUIWrapper")]
        private static extern void OgrePlatform_shutdown(IntPtr ogrePlatform);

#endregion
    }
}

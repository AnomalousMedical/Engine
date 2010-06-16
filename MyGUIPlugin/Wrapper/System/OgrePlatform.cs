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

        public OgrePlatform()
        {
            ogrePlatform = OgrePlatform_Create();
        }

        public void Dispose()
        {
            OgrePlatform_Delete(ogrePlatform);
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
        private static extern void OgrePlatform_Delete(IntPtr ogrePlatform);

        [DllImport("MyGUIWrapper")]
        private static extern void OgrePlatform_initialize(IntPtr ogrePlatform, IntPtr renderWindow, IntPtr sceneManager, String resourceGroup);

        [DllImport("MyGUIWrapper")]
        private static extern void OgrePlatform_shutdown(IntPtr ogrePlatform);

#endregion
    }
}

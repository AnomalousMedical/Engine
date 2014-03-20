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

        public void initialize(int windowWidth, int windowHeight, String resourceGroup, String logName)
        {
            OgrePlatform_initialize(ogrePlatform, windowWidth, windowHeight, resourceGroup, logName);
            renderManager = new OgreRenderManager(OgrePlatform_getRenderManagerPtr(ogrePlatform));
        }

        public void shutdown()
        {
            OgrePlatform_shutdown(ogrePlatform);
        }

#region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OgrePlatform_Create();

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OgrePlatform_getRenderManagerPtr(IntPtr ogrePlatform);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgrePlatform_Delete(IntPtr ogrePlatform);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgrePlatform_initialize(IntPtr ogrePlatform, int windowWidth, int windowHeight, String resourceGroup, String logName);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgrePlatform_shutdown(IntPtr ogrePlatform);

#endregion
    }
}

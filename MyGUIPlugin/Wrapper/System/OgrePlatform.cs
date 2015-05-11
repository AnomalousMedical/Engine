using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgrePlugin;
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

        public OgreRenderManager RenderManager
        {
            get
            {
                return renderManager;
            }
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

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OgrePlatform_Create();

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OgrePlatform_getRenderManagerPtr(IntPtr ogrePlatform);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgrePlatform_Delete(IntPtr ogrePlatform);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgrePlatform_initialize(IntPtr ogrePlatform, int windowWidth, int windowHeight, String resourceGroup, String logName);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgrePlatform_shutdown(IntPtr ogrePlatform);

#endregion
    }
}

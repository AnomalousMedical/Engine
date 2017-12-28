using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Anomalous.OgreOpenVr
{
    public class OgreFramework : IDisposable
    {
        private IntPtr framework;

        public OgreFramework()
        {
            framework = OgreFramework_Create();
        }

        public void Dispose()
        {
            if(framework != IntPtr.Zero)
            {
                OgreFramework_Destroy(framework);
                framework = IntPtr.Zero;
            }
        }

        public void Init(Root root, SceneManager sceneManager)
        {
            OgreFramework_initOgre(framework, root.NativePtr, sceneManager.NativePtr);
        }

        public void Update(double timeSinceLastUpdate)
        {
            OgreFramework_updateOgre(framework, timeSinceLastUpdate);
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OgreFramework_Create();

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OgreFramework_Destroy(IntPtr framework);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool OgreFramework_initOgre(IntPtr framework, IntPtr root, IntPtr sceneManager);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OgreFramework_updateOgre(IntPtr framework, double timeSinceLastFrame);

        #endregion
    }
}

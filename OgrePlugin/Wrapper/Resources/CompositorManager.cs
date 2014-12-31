using Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class CompositorManager
    {
        static CompositorManager instance = new CompositorManager();

        public static CompositorManager getInstance()
        {
            return instance;
        }

        public bool addCompositor(Viewport vp, String compositor, int addPosition = -1)
        {
            return CompositorManager_addCompositor(vp.OgreViewport, compositor, addPosition) != IntPtr.Zero;
        }

        public void removeCompositor(Viewport vp, String compositor)
        {
            CompositorManager_removeCompositor(vp.OgreViewport, compositor);
        }

        public void setCompositorEnabled(Viewport vp, String compositor, bool value)
        {
            CompositorManager_setCompositorEnabled(vp.OgreViewport, compositor, value);
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr CompositorManager_addCompositor(IntPtr vp, String compositor, int addPosition);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void CompositorManager_removeCompositor(IntPtr vp, String compositor);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CompositorManager_setCompositorEnabled(IntPtr vp, String compositor, bool value);

        #endregion
    }
}

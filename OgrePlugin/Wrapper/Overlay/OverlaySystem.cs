using Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class OverlaySystem
    {
        private IntPtr overlaySystem;

        public OverlaySystem()
        {
            overlaySystem = OverlaySystem_Create();
        }

        public void Dispose()
        {
            OverlaySystem_Delete(overlaySystem);
            overlaySystem = IntPtr.Zero;
        }

        [DllImport("OgreCWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OverlaySystem_Create();

        [DllImport("OgreCWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void OverlaySystem_Delete(IntPtr overlaySystem);
    }
}

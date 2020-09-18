using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OgreNextPlugin.Wrapper.Renderer
{
    public class HlmsManager
    {
        internal static void setup()
        {
            HlmsManager_setup();
        }

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void HlmsManager_setup();
    }
}

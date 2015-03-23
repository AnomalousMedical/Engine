using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform
{
    public class AndroidFunctions
    {
        public static void EasyAttributeSetup(float screenDensity)
        {
            AndroidOSWindow_EasyAttributeSetup(screenDensity);
        }

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void AndroidOSWindow_EasyAttributeSetup(float screenDensity);
    }
}

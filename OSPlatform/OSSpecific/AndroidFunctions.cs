using Anomalous.Interop;
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
        private static NativeAction toggleKeyboardCb;

        public static void EasyAttributeSetup(float screenDensity, NativeAction toggleKeyboard)
        {
            toggleKeyboardCb = toggleKeyboard;
            AndroidOSWindow_EasyAttributeSetup(screenDensity, toggleKeyboardCb);
        }

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void AndroidOSWindow_EasyAttributeSetup(float screenDensity, NativeAction toggleKeyboard);
    }
}

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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ToggleKeyboard(bool visible);

        private static ToggleKeyboard toggleKeyboardCb;

        public static void EasyAttributeSetup(float screenDensity, ToggleKeyboard toggleKeyboard)
        {
            toggleKeyboardCb = toggleKeyboard;
            AndroidOSWindow_EasyAttributeSetup(screenDensity, toggleKeyboardCb);
        }

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void AndroidOSWindow_EasyAttributeSetup(float screenDensity, ToggleKeyboard toggleKeyboard);
    }
}

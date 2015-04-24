using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform.Android
{
    class AndroidFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ToggleKeyboard(OnscreenKeyboardMode mode);

        private static ToggleKeyboard toggleKeyboardCb;

        public static void EasyAttributeSetup(float screenDensity, ToggleKeyboard toggleKeyboard)
        {
            toggleKeyboardCb = toggleKeyboard;
            AndroidOSWindow_EasyAttributeSetup(screenDensity, toggleKeyboardCb);
        }

        public static void OutputCurrentABI()
        {
            AndroidApp_outputCurrentABI();
        }

        [DllImport(AndroidPlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void AndroidOSWindow_EasyAttributeSetup(float screenDensity, ToggleKeyboard toggleKeyboard);

        [DllImport(AndroidPlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void AndroidApp_outputCurrentABI();
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform.Win32
{
    public static class Win32Platform
    {
        public static void Initialize()
        {
            new WindowsRuntimePlatformInfo();

            //Make sure the paths are setup correctly on windows to find 32/64 bit binaries.
            try
            {
                //Check bitness and determine path 
                String executionPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                String nativeLibPath;
                if (Environment.Is64BitProcess)
                {
                    nativeLibPath = Path.Combine(executionPath, "x64");
                }
                else
                {
                    nativeLibPath = Path.Combine(executionPath, "x86");
                }
                if (Directory.Exists(nativeLibPath))
                {
                    RuntimePlatformInfo.addPath(nativeLibPath);
                }
            }
            catch (Exception) { }

            Win32Window_setKeyboardPathAndWindow("\"C:\\Program Files\\Common Files\\Microsoft Shared\\ink\\tabtip.exe\"", "IPTip_Main_Window");
        }

        const String LibraryName = "OSHelper";

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Win32Window_setKeyboardPathAndWindow([MarshalAs(UnmanagedType.LPWStr)] String keyboardPath, [MarshalAs(UnmanagedType.LPWStr)] String windowName);
    }
}

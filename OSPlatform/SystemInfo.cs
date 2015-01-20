using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace Anomalous.OSPlatform
{
    public enum RuntimeOperatingSystem
    {
        Windows,
        Mac,
        WinRT,
		iOS
    }

    public class SystemInfo
    {
        private SystemInfo() { }

        public static uint DisplayCount
        {
            get
            {
                return SystemInfo_getDisplayCount();
            }
        }

        public static IntVector2 getDisplayLocation(int displayIndex)
        {
            int x, y;
            SystemInfo_getDisplayLocation(displayIndex, out x, out y);
            return new IntVector2(x, y);
        }

        /// <summary>
        /// Get the operating system we are currently running on.
        /// </summary>
        public static RuntimeOperatingSystem RuntimeOS
        {
            get
            {
                return PlatformConfig_getPlatform();
            }
        }

        #region PInvoke

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern uint SystemInfo_getDisplayCount();

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void SystemInfo_getDisplayLocation(int displayIndex, out int x, out int y);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern RuntimeOperatingSystem PlatformConfig_getPlatform();

        #endregion
    }
}

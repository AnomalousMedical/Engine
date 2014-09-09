using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;

namespace PCPlatform
{
    public class PCSystemTimer : SystemTimer, IDisposable
    {
        IntPtr performanceCounter;

        public PCSystemTimer()
        {
            performanceCounter = PerformanceCounter_Create();
        }

        public void Dispose()
        {
            PerformanceCounter_Delete(performanceCounter);
        }

        public bool initialize()
        {
            return PerformanceCounter_initialize(performanceCounter);
        }

        public long getCurrentTime()
        {
            return PerformanceCounter_getCurrentTime(performanceCounter);
        }

        public bool Accurate
        {
            get
            {
                return PerformanceCounter_isAccurate(performanceCounter);
            }
            set
            {
                PerformanceCounter_setAccurate(performanceCounter, value);
            }
        }

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr PerformanceCounter_Create();

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern void PerformanceCounter_Delete(IntPtr counter);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool PerformanceCounter_initialize(IntPtr counter);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern Int64 PerformanceCounter_getCurrentTime(IntPtr counter);

        [DllImport("PCPlatform", CallingConvention = CallingConvention.Cdecl)]
        private static extern void PerformanceCounter_setAccurate(IntPtr counter, bool accurate);

        [DllImport("PCPlatform", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool PerformanceCounter_isAccurate(IntPtr counter);
    }
}

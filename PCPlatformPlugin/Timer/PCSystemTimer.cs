using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;

namespace PCPlatform
{
    class PCSystemTimer : SystemTimer, IDisposable
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

        [DllImport("PCPlatform")]
        private static extern IntPtr PerformanceCounter_Create();

        [DllImport("PCPlatform")]
        private static extern void PerformanceCounter_Delete(IntPtr counter);

        [DllImport("PCPlatform")]
        private static extern bool PerformanceCounter_initialize(IntPtr counter);

        [DllImport("PCPlatform")]
        private static extern long PerformanceCounter_getCurrentTime(IntPtr counter);
    }
}

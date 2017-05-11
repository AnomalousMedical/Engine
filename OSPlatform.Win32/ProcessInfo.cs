using Engine.Shim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPlatform.Win32
{
    class ProcessInfo : IProcessInfo
    {
        public long PrivateMemorySize64 => System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;

        public long WorkingSet64 => System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;

        public long VirtualMemorySize64 => System.Diagnostics.Process.GetCurrentProcess().VirtualMemorySize64;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Threading
{
    public static class ThreadShim
    {
        public static void Sleep(int ms)
        {
#if ENABLE_LEGACY_SHIMS
            using(var waitEvt = new System.Threading.ManualResetEvent(false))
            {
                waitEvt.WaitOne(ms);
            }
#else
            Thread.Sleep(ms);
#endif
        }
    }
}

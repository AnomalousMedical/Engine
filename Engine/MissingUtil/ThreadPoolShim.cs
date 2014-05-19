using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Threading
{
#if ENABLE_LEGACY_SHIMS
    public delegate void WaitCallback(Object state);
#endif

    public static class ThreadPoolShim
    {
        public static void QueueUserWorkItem(WaitCallback callback, Object state)
        {
#if ENABLE_LEGACY_SHIMS
            Task task = new Task(() => callback(state));
            task.Start();
#else
            ThreadPool.QueueUserWorkItem(callback, state);
#endif
        }
    }
}

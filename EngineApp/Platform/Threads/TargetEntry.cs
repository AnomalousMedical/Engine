using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EngineApp.Platform.Threads
{
    class TargetEntry
    {
        private Delegate func;
        private Object[] args;
        private AutoResetEvent threadEvent = new AutoResetEvent(false);

        public TargetEntry(Delegate func, object[] args)
        {
            this.func = func;
            this.args = args;
        }

        public void invoke()
        {
            func.DynamicInvoke(args);
            threadEvent.Set();
            Finished = true;
        }

        public void wait()
        {
            threadEvent.WaitOne();
        }

        public void cancel()
        {
            threadEvent.Set();
            Finished = true;
        }

        public bool Finished { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Medical.Controller
{
    public class ThreadManagerSynchronizeInvoke : ISynchronizeInvoke
    {
        private AutoResetEvent autoReset = new AutoResetEvent(false);

        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            autoReset.Reset();
            ThreadManager.invokeAndWait(new Action(() =>
            {
                method.DynamicInvoke(args);
                autoReset.Set();
            }));
            return null;
        }

        public object EndInvoke(IAsyncResult result)
        {
            autoReset.WaitOne();
            return null;
        }

        public object Invoke(Delegate method, object[] args)
        {
            ThreadManager.invokeAndWait(method, args);
            return null;
        }

        public bool InvokeRequired
        {
            get { return true; }
        }
    }
}

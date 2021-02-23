using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncContextTest
{
    //Modified from
    //https://devblogs.microsoft.com/pfxteam/await-synchronizationcontext-and-console-apps/
    class SingleThreadSynchronizationContext : SynchronizationContext
    {
        private readonly ConcurrentQueue<KeyValuePair<SendOrPostCallback, object>>
                      m_queue = new ConcurrentQueue<KeyValuePair<SendOrPostCallback, object>>();

        public override void Post(SendOrPostCallback d, object state)
        {
            m_queue.Enqueue(new KeyValuePair<SendOrPostCallback, object>(d, state));
        }

        public void PumpCurrentQueue()
        {
            KeyValuePair<SendOrPostCallback, object> workItem;
            while (m_queue.TryDequeue(out workItem))
            {
                workItem.Key(workItem.Value);
            }
        }
    }
}

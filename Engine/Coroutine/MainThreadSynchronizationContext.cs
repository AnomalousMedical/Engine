using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Engine
{
    //Modified from
    //https://devblogs.microsoft.com/pfxteam/await-synchronizationcontext-and-console-apps/

    /// <summary>
    /// This class will allow async await to work from the main thread and keep the tasks on that thread.
    /// Tasks can get queued up and will be pumped every frame.
    /// </summary>
    public class MainThreadSynchronizationContext : SynchronizationContext
    {
        private readonly ConcurrentQueue<KeyValuePair<SendOrPostCallback, object>>
                      m_queue = new ConcurrentQueue<KeyValuePair<SendOrPostCallback, object>>();

        public override void Post(SendOrPostCallback d, object state)
        {
            m_queue.Enqueue(new KeyValuePair<SendOrPostCallback, object>(d, state));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            //This might be better pooled. I dunno if send is even ever used.
            using (var ev = new ManualResetEventSlim(false))
            {
                m_queue.Enqueue(new KeyValuePair<SendOrPostCallback, object>(s => { d(s); ev.Set(); }, state));
                ev.Wait();
            }
        }

        public void PumpCurrentQueue()
        {
            //Pump any items in the queue
            KeyValuePair<SendOrPostCallback, object> workItem;
            while (m_queue.TryDequeue(out workItem))
            {
                workItem.Key(workItem.Value);
            }
        }
    }
}

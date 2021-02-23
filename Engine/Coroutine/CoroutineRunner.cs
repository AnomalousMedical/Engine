using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Platform;
using Engine.Threads;
using Microsoft.Extensions.Logging;

namespace Engine
{
    /// <summary>
    /// This is the controller class for Coroutines. To execute a new coroutine
    /// call the Start function.
    /// </summary>
    /// <remarks>
    /// Coroutines can be executed using this class. In order to create a new
    /// coroutine a funciton must be created that has a return type of
    /// IEnumerator&lt;YieldAction&gt;. This function can then be executed
    /// inside of a call to Coroutine.Start. Inside of a coroutine it is valid
    /// to call yield return Coroutine.Wait to make the coroutine wait. There
    /// may also be plugin specific wait extensions. It is not valid to call
    /// these functions without proceeding it with a yield return.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CoroutineRunner : ICoroutineRunner
    {
        private List<IEnumerator<YieldAction>> queued = new List<IEnumerator<YieldAction>>();
        private List<IEnumerator<YieldAction>> queuedIter = new List<IEnumerator<YieldAction>>(); //Used to iterate the queue, this and the queud pointer above are swapped
        private List<YieldAction> waitActions = new List<YieldAction>();
        private readonly ILogger<CoroutineRunner> logger;

        public CoroutineRunner(ILogger<CoroutineRunner> logger)
        {
            this.logger = logger;
        }

        public void Run(IEnumerator<YieldAction> coroutine)
        {
            //Fire coroutine start right away. If it returns anything it will get queued.
            if (coroutine.MoveNext()) //Fire the first steps right away
            {
                //If it returns anything add it to the queue
                coroutine.Current.setEnumeration(coroutine);
            }
        }

        public void Queue(IEnumerator<YieldAction> coroutine)
        {
            ThreadManager.invoke(() =>
            {
                queued.Add(coroutine);
            });
        }

        public YieldAction WaitSeconds(double seconds)
        {
            WaitAction wait = new WaitAction(this, (Int64)(seconds * Clock.SecondsToMicro));
            waitActions.Add(wait);
            return wait;
        }

        public YieldAction Await(Func<Task> task)
        {
            AwaitAction wait = new AwaitAction(this, task(), logger);
            waitActions.Add(wait);
            return wait;
        }

        public YieldAction Await(Task task)
        {
            AwaitAction wait = new AwaitAction(this, task, logger);
            waitActions.Add(wait);
            return wait;
        }

        /// <summary>
        /// This is an internal function to continue executing a coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine to continue executing.</param>
        internal void Continue(IEnumerator<YieldAction> coroutine)
        {
            queued.Add(coroutine);
        }

        /// <summary>
        /// This is an internal function to update all coroutines. It should be
        /// called once per frame. It will cause all queued coroutines to
        /// execute and then clear the queue. It will also update all the
        /// WaitAction timers and execute any coroutines that have waited their
        /// alloted time.
        /// </summary>
        /// <param name="time">The amount of time since the last update in seconds.</param>
        internal void Update(Clock clock)
        {
            for (int i = 0; i < waitActions.Count;)
            {
                if (waitActions[i].tick(clock))
                {
                    waitActions.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }

            //Swap queued and queuedIter pointers
            var deck = queued;
            queued = queuedIter;
            queuedIter = deck;

            foreach (IEnumerator<YieldAction> coroutine in queuedIter)
            {
                if (coroutine.MoveNext())
                {
                    coroutine.Current.setEnumeration(coroutine);
                }
            }
            queuedIter.Clear();
        }
    }
}

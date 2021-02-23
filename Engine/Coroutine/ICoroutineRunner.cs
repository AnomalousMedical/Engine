using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Engine
{
    public interface ICoroutineRunner
    {
        /// <summary>
        /// Start executing a coroutine. Any code up to the first yield return in the enumerator
        /// will execute right away on the current thread. On background threads use Queue
        /// to queue a coroutine to run later.
        /// </summary>
        /// <param name="coroutine">The coroutine to start executing.</param>
        void Run(IEnumerator<YieldAction> coroutine);

        /// <summary>
        /// Queue a coroutine to start running on the next update. This is safe to use from any
        /// thread and the entire coroutine will execute on the main thread.
        /// </summary>
        /// <param name="coroutine"></param>
        void Queue(IEnumerator<YieldAction> coroutine);

        /// <summary>
        /// Yield a coroutine until the given task is completed. Since coroutines run on the
        /// main thread they will use a synchronization context that keeps all await tasks
        /// on the main thread.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        YieldAction Await(Func<Task> task);

        /// <summary>
        /// Yield a coroutine until the given task is completed. Since coroutines run on the
        /// main thread they will use a synchronization context that keeps all await tasks
        /// on the main thread.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        YieldAction Await(Task task);

        /// <summary>
        /// Wait the given number of seconds before continuing the coroutine.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        YieldAction WaitSeconds(double seconds);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Engine
{
    public interface ICoroutineRunner
    {
        /// <summary>
        /// Start running a coroutine.
        /// </summary>
        /// <param name="coroutine"></param>
        void Start(IEnumerator<YieldAction> coroutine);

        /// <summary>
        /// Yield a coroutine until the given task is completed. You can write
        /// complex statements in here, but there are no guarentees about which thread
        /// your code will run on. The passed in task will be started on the current
        /// thread right away, but anything after await statements will depend on the current
        /// context. This might be fine if you need to further post process your data, but don't
        /// assume you are running on the main thread inside async calls. This only applies to
        /// code in the task. Other code in your coroutine enum will still only run on the main
        /// thread.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        YieldAction Await(Func<Task> task);

        /// <summary>
        /// Wait the given number of seconds before continuing the coroutine.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        YieldAction WaitSeconds(double seconds);
    }
}
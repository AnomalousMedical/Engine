using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// The scoped coroutine makes sure that coroutines will not be fired again after their
    /// associated scope has been destroyed. If this is bound to other objects in the scope
    /// that schedule their tasks through it none of them will receive any more signals after
    /// the scope is Disposed.
    /// </summary>
    public interface IScopedCoroutine
    {
        YieldAction Await(Func<Task> task);
        YieldAction Await(Task task);
        void Queue(IEnumerator<YieldAction> coroutine);
        void Run(IEnumerator<YieldAction> coroutine);

        /// <summary>
        /// Run a task using the coroutine runner. The task will be started on a tiny 1 step
        /// coroutine with it inside. The task will pump all the way to completion like any
        /// coroutine task. Essentially the task will run and any coroutine stuff can be safely
        /// ignored. Good for loading resources, but for precise timing use coroutines directly with Run.
        /// Tasks execute on their own queue and they will not stop like coroutines started by this
        /// scope. This is true of yield return coroutine.Async as well.
        /// </summary>
        /// <param name="t">The task to run.</param>
        public void RunTask(Task t);

        /// <summary>
        /// Run a task using the coroutine runner. The task will be started on a tiny 1 step
        /// coroutine with it inside. The task will pump all the way to completion like any
        /// coroutine task. Essentially the task will run and any coroutine stuff can be safely
        /// ignored. Good for loading resources, but for precise timing use coroutines directly with Run.
        /// Tasks execute on their own queue and they will not stop like coroutines started by this
        /// scope. This is true of yield return coroutine.Async as well.
        /// </summary>
        /// <param name="t">The task to run.</param>
        public void RunTask(Func<Task> t);

        YieldAction WaitSeconds(double seconds);
    }
}
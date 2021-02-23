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
        YieldAction WaitSeconds(double seconds);
    }
}
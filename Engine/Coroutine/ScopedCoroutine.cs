using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    class ScopedCoroutine : IScopedCoroutine, IDisposable
    {
        private readonly CoroutineRunner coroutineRunner;
        private bool allowPassthrough = true;

        public ScopedCoroutine(CoroutineRunner coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;
        }

        public void Dispose()
        {
            allowPassthrough = false;
        }

        public void Run(IEnumerator<YieldAction> coroutine)
        {
            coroutineRunner.Run(Enumerate(coroutine));
        }

        public void Queue(IEnumerator<YieldAction> coroutine)
        {
            coroutineRunner.Queue(Enumerate(coroutine));
        }

        private IEnumerator<YieldAction> Enumerate(IEnumerator<YieldAction> coroutine)
        {
            while (allowPassthrough && coroutine.MoveNext())
            {
                yield return coroutine.Current;
            }
        }

        public YieldAction WaitSeconds(double seconds)
        {
            return coroutineRunner.WaitSeconds(seconds);
        }

        public YieldAction Await(Func<Task> task)
        {
            return coroutineRunner.Await(task);
        }

        public YieldAction Await(Task task)
        {
            return coroutineRunner.Await(task);
        }

        public void RunTask(Task t)
        {
            coroutineRunner.RunTask(t);
        }

        public void RunTask(Func<Task> t)
        {
            coroutineRunner.RunTask(t);
        }
    }
}

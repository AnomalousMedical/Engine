using Engine.Platform;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// This is a YieldAction that will wait for a given amount of time before
    /// continuing execution of a coroutine.
    /// </summary>
    class AwaitAction : YieldAction
    {
        private readonly Task task;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="delay">The amount of time to wait in milliseconds.</param>
        public AwaitAction(CoroutineRunner coroutineRunner, Task task, ILogger logger)
            :base(coroutineRunner)
        {
            this.task = task;
            this.logger = logger;
        }

        /// <summary>
        /// Update the timer and queue the coroutine if enough time has passed.
        /// This will return true if the wait is finished.
        /// </summary>
        /// <param name="seconds">The amount of seconds since the last update.</param>
        /// <returns>True if the wait is completed, false if it has more time to go.</returns>
        public override bool tick(Clock clock)
        {
            if (task.IsCompleted)
            {
                switch (task.Status)
                {
                    case TaskStatus.Faulted:
                        //An error happened in the task.
                        //This is the main thread so just propagate causing the program to crash most likely
                        //No way to recover from errors here and coroutines can't just be left to hang since that could cause more issues.
                        throw new CoroutineException($"{task.Exception.GetType().Name} running Task for coroutine.\nMessage: {task.Exception.Message}", task.Exception);
                    default:
                        execute();
                        break;
                }
                return true;
            }
            return false;
        }
    }
}

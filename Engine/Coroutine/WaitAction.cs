using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This is a YieldAction that will wait for a given amount of time before
    /// continuing execution of a coroutine.
    /// </summary>
    class WaitAction : YieldAction
    {
        private Int64 delay;
        private Int64 elapsed;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="delay">The amount of time to wait in milliseconds.</param>
        public WaitAction(Int64 delayMs)
        {
            this.delay = delayMs * 1000; //Clock uses microseconds, so convert.
            this.elapsed = 0;
        }

        /// <summary>
        /// Update the timer and queue the coroutine if enough time has passed.
        /// This will return true if the wait is finished.
        /// </summary>
        /// <param name="seconds">The amount of seconds since the last update.</param>
        /// <returns>True if the wait is completed, false if it has more time to go.</returns>
        public bool tick(Clock clock)
        {
            elapsed += clock.DeltaTimeMicro;
            if (elapsed > delay)
            {
                execute();
                return true;
            }
            return false;
        }
    }
}

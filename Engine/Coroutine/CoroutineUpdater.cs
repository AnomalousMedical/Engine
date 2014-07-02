using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace Engine
{
    /// <summary>
    /// This class updates the Coroutines from the timer.
    /// </summary>
    class CoroutineUpdater : UpdateListener
    {
        /// <summary>
        /// Called when the loop ticks.
        /// </summary>
        /// <param name="clock">The Clock containing the time since the last update.</param>
        public void sendUpdate(Clock clock)
        {
            Coroutine.Update(clock);
        }

        /// <summary>
        /// Fired when the loop is first started up, if the loop is started and stopped multiple
        /// times during execution this will fire every time the loop is started.
        /// </summary>
        public void loopStarting()
        {
            
        }

        /// <summary>
        /// Fired when the max delta time is exceeded.  This is likely because of a very long
        /// pause so any timing smoothing must be reset.
        /// </summary>
        public void exceededMaxDelta()
        {
            
        }
    }
}

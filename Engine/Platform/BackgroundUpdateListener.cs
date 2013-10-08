using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    public interface BackgroundUpdateListener
    {
        /// <summary>
        /// Start the background thread to do its work for this tick. The work done should not modify any other subsystems that
        /// might be running in parallell.
        /// </summary>
        void doBackgroundWork(Clock clock);

        /// <summary>
        /// Synchronize the results from the task. This will be run on the main thread and it will be safe to update all subsystems.
        /// </summary>
        void synchronizeResults();

        /// <summary>
        /// Fired when the loop is first started up, if the loop is started and stopped multiple
        /// times during execution this will fire every time the loop is started.
        /// </summary>
        void loopStarting();

        /// <summary>
        /// Fired when the max delta time is exceeded.  This is likely because of a very long
        /// pause so any timing smoothing must be reset.
        /// </summary>
        void exceededMaxDelta();
    }
}

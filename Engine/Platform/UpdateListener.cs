using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    public interface UpdateListener
    {
        /// <summary>
	    /// Called when the loop ticks.
	    /// </summary>
	    /// <param name="clock">The Clock containing the time since the last update.</param>
	    void sendUpdate(Clock clock);

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

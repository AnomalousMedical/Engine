using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    /// <summary>
    /// This class subscribes to the timer to do event updates.
    /// </summary>
    public class EventUpdateListener : UpdateListener
    {
        private EventManager eventManager;

        /// <summary>
        /// Constructor, takes the event manager to call.
        /// </summary>
        /// <param name="eventManager">The event manager to update.</param>
        public EventUpdateListener(EventManager eventManager)
        {
            this.eventManager = eventManager;
        }

        /// <summary>
        /// Loop starting function, does nothing.
        /// </summary>
        public void loopStarting()
        {
            
        }

        /// <summary>
        /// Update function.
        /// </summary>
        /// <param name="clock">The clock.</param>
        public void sendUpdate(Clock clock)
        {
            eventManager.updateEvents(clock.Seconds);
        }

        /// <summary>
        /// Called when the max delta was exceeded.
        /// </summary>
        public void exceededMaxDelta()
        {
            
        }
    }
}

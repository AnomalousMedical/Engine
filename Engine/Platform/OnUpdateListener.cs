using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Platform
{
    /// <summary>
    /// This update listener makes it easy to use events to listen to the timer.
    /// </summary>
    public class OnUpdateListener : UpdateListener
    {
        /// <summary>
        /// This is fired when sendUpdate is called.
        /// </summary>
        public event Action<Clock> OnUpdate;

        public void exceededMaxDelta()
        {
            
        }

        public void loopStarting()
        {
            
        }

        public void sendUpdate(Clock clock)
        {
            if (OnUpdate != null)
            {
                OnUpdate.Invoke(clock);
            }
        }
    }
}

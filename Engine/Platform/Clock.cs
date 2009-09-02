using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    /// <summary>
    /// The clock provides how long since the last update for UpdateListeners.
    /// </summary>
    public class Clock
    {
        protected Int64 timeMs;

        /// <summary>
        /// Get the amount of time on this clock in seconds.
        /// </summary>
        public double Seconds
        {
            get
            {
                return timeMs / 1000.0f;
            }
        }

        /// <summary>
        /// Set the time on this clock in milliseconds.
        /// </summary>
        /// <param name="ms">The number of milliseconds to set.</param>
        internal void setTimeMilliseconds(Int64 ms)
        {
            timeMs = ms;
        }
    }
}

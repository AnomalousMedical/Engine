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
        protected Int64 timeMicro;

        /// <summary>
        /// Get the amount of time on this clock in seconds.
        /// </summary>
        public double Seconds
        {
            get
            {
                return timeMicro / 1000000.0;
            }
        }

        /// <summary>
        /// Set the time on this clock in microseconds.
        /// </summary>
        /// <param name="micro">The number of microseconds to set.</param>
        internal void setTimeMicroseconds(Int64 micro)
        {
            timeMicro = micro;
        }
    }
}

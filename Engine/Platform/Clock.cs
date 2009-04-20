﻿using System;
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
        double timeSeconds;

        /// <summary>
        /// Set the time on this clock in seconds.
        /// </summary>
        /// <param name="seconds">The number of seconds to set.</param>
        public void setTimeSeconds(double seconds)
        {
            timeSeconds = seconds;
        }

        /// <summary>
        /// Get the amount of time on this clock in seconds.
        /// </summary>
        public double Seconds
        {
            get
            {
                return timeSeconds;
            }
        }
    }
}

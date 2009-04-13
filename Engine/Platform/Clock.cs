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
        double timeSeconds;

        public void setTimeSeconds(double seconds)
        {
            timeSeconds = seconds;
        }

        public double Seconds
        {
            get
            {
                return timeSeconds;
            }
        }
    }
}

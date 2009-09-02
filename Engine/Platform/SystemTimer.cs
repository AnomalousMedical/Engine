using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    public interface SystemTimer
    {
        /// <summary>
        /// Setup the timer. This should be called on the thread that runs the timer.
        /// </summary>
        /// <returns>True if the timer can be used.</returns>
        bool initialize();

        /// <summary>
        /// Get the current time in microseconds since the timer was initialized..
        /// </summary>
        /// <returns>The current time in microseconds since the timer was initialized.</returns>
        Int64 getCurrentTime();
    }
}

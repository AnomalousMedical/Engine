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

        /// <summary>
        /// Setting this to true will try to make the OS have more accurate timing to make
        /// Thread.Sleep sleep for the correct amount of time. Depending on your os this may
        /// not actually do anything.
        /// 
        /// For example on windows this will probably set timeBeginPeriod(1) to make windows
        /// run at millisecond timing.
        /// 
        /// If you are trying to cap the framerate with your update timer, it is a good idea
        /// to call this function so you can rely on sleep working correctly.
        /// </summary>
        bool Accurate { get; set; }
    }
}

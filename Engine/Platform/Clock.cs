using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    /// <summary>
    /// The clock provides how long since the last update for UpdateListeners as well as the current update time.
    /// </summary>
    public sealed class Clock
    {
        /// <summary>
        /// Convert floating point seconds to Int64 microseconds.
        /// </summary>
        /// <param name="seconds">The number of seconds as a float.</param>
        /// <returns>An Int64 version of the time in microseconds.</returns>
        public static Int64 SecondsToMicroseconds(float seconds)
        {
            return (Int64)(seconds * 1000000.0f);
        }

        /// <summary>
        /// Convert Int64 microseconds to float seconds.
        /// </summary>
        /// <param name="microseconds">The microseconds.</param>
        /// <returns>Floating point seconds.</returns>
        public static float MicrosecondsToSeconds(Int64 microseconds)
        {
            return microseconds / 1000000.0f;
        }

        /// <summary>
        /// Convert milliseconds to microseconds.
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static Int64 MillisecondsToMicroseconds(Int64 milliseconds)
        {
            return milliseconds * 1000;
        }

        /// <summary>
        /// Convert microseconds to milliseconds. Stays as an int.
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static Int64 MicrosecondsToMilliseconds(Int64 microseconds)
        {
            return microseconds / 1000;
        }

        private Int64 deltaMicro;
        private Int64 currentTimeMicro;
        private float deltaSeconds;

        /// <summary>
        /// The amount of time that has passed since the last update as floating point seconds.
        /// This will be rounded to the closest floating point number.
        /// </summary>
        public float DeltaSeconds
        {
            get
            {
                return deltaSeconds;
            }
        }

        /// <summary>
        /// The amount of time that has passed in microseconds as an Int64, no rounding has been applied to
        /// this number.
        /// </summary>
        public Int64 DeltaTimeMicro
        {
            get
            {
                return deltaMicro;
            }
        }

        /// <summary>
        /// The current time since the program started (0) in microseconds as an Int64, no rounding has been 
        /// applied to this number.
        /// </summary>
        public Int64 CurrentTimeMicro
        {
            get
            {
                return currentTimeMicro;
            }
        }

        /// <summary>
        /// Set the time on this clock in microseconds.
        /// </summary>
        /// <param name="deltaMicro">The number of microseconds to set.</param>
        internal void setTimeMicroseconds(Int64 currentTimeMicro, Int64 deltaMicro)
        {
            this.currentTimeMicro = currentTimeMicro;
            this.deltaMicro = deltaMicro;
            this.deltaSeconds = (float)(deltaMicro / 1000000.0);
        }
    }
}

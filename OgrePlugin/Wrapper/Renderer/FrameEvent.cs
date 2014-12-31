using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    public class FrameEvent
    {
        /// <summary>
        /// Elapsed time in seconds since the last event. This gives you time
        /// between frame start & frame end, and between frame end and next
        /// frame start.
        /// </summary>
        /// <remarks>
        /// This may not be the elapsed time but the average elapsed time
        /// between recently fired events.
        /// </remarks>
        public float timeSinceLastEvent;

        /// <summary>
        /// Elapsed time in seconds since the last event of the same type, i.e.
        /// time for a complete frame.
        /// </summary>
        /// <remarks>
        /// This may not be the elapsed time but the average elapsed time
        /// between recently fired events of the same type.
        /// </remarks>
        public float timeSinceLastFrame;
    }
}

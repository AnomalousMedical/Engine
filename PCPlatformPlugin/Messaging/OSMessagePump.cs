using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCPlatform
{
    /// <summary>
    /// This interface provides an abstract way to handle messages from the
    /// operating system on a PC. This can be used with the PCUpdateTimer to
    /// check messages every frame.
    /// </summary>
    public interface OSMessagePump
    {
        /// <summary>
        /// The loop is starting when this function is called. This is only
        /// called once per loop execution.
        /// </summary>
        void loopStarting();

        /// <summary>
        /// Process messages. Return true to skip the actual frame processing
        /// for this iteration of the loop.
        /// </summary>
        /// <returns>True to skip the rest of the frame processing.</returns>
        bool processMessages();

        /// <summary>
        /// This method is called when the loop is stopping. Any cleanup can
        /// happen here. This is only called once per loop execution.
        /// </summary>
        void loopCompleted();
    }
}

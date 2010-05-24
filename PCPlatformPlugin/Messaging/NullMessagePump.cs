using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCPlatform
{
    /// <summary>
    /// A null message pump that will be used by default.
    /// </summary>
    class NullMessagePump : OSMessagePump
    {
        public void loopStarting()
        {
            
        }

        public bool processMessages()
        {
            return false;
        }

        public void loopCompleted()
        {
            
        }
    }
}

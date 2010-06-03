using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PCPlatform
{
    public class AgnosticMessagePump : OSMessagePump
    {
        public void loopStarting()
        {
            AgnosticMessagePump_primeMessages();
        }

        public bool processMessages()
        {
            return AgnosticMessagePump_processMessage();
        }

        public void loopCompleted()
        {
            AgnosticMessagePump_finishMessages();
        }

#region PInvoke

        [DllImport("PCPlatform")]
        private static extern void AgnosticMessagePump_primeMessages();

        [DllImport("PCPlatform")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool AgnosticMessagePump_processMessage();

        [DllImport("PCPlatform")]
        private static extern void AgnosticMessagePump_finishMessages();

#endregion
    }
}

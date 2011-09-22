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

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern void AgnosticMessagePump_primeMessages();

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool AgnosticMessagePump_processMessage();

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern void AgnosticMessagePump_finishMessages();

#endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    enum LogLevel
    {
        Info,
        Warning,
        Error,
        Critical,
        EndLogLevel
    };

    class ManagedMyGUILogListener : IDisposable
    {
        private MessageLoggedDelegate messageLoggedDelegate;
        private IntPtr nativePtr;

        public ManagedMyGUILogListener()
        {
            messageLoggedDelegate = new MessageLoggedDelegate(messageLogged);
            nativePtr = ManagedMyGUILogListener_Create(messageLoggedDelegate);
        }

        public void Dispose()
        {
            ManagedMyGUILogListener_Delete(nativePtr);
        }

        private void messageLogged(String section, LogLevel lml, String message)
        {
            Log.Info(message);
        }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void MessageLoggedDelegate(String section, LogLevel lml, String message);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr ManagedMyGUILogListener_Create(MessageLoggedDelegate messageLoggedCallback);

        [DllImport("MyGUIWrapper")]
        private static extern void ManagedMyGUILogListener_Delete(IntPtr logListener);

        #endregion
    }
}

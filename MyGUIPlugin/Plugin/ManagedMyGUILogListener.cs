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

        private Logging.LogLevel[] levelMap = { Logging.LogLevel.Info, Logging.LogLevel.Warning, Logging.LogLevel.Error, Logging.LogLevel.ImportantInfo, Logging.LogLevel.Debug };

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
            Log.Default.sendMessage(message, levelMap[(int)lml], "MyGUI_" + section);
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

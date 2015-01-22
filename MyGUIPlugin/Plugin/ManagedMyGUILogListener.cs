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
        private IntPtr nativePtr;
        private CallbackHandler callbackHandler;

        private Logging.LogLevel[] levelMap = { Logging.LogLevel.Info, Logging.LogLevel.Warning, Logging.LogLevel.Error, Logging.LogLevel.ImportantInfo, Logging.LogLevel.Debug };

        public ManagedMyGUILogListener()
        {
            callbackHandler = new CallbackHandler();
            nativePtr = callbackHandler.create(this);
        }

        public void Dispose()
        {
            ManagedMyGUILogListener_Delete(nativePtr);
            callbackHandler.Dispose();
        }

        private void messageLogged(String section, LogLevel lml, String message)
        {
            Log.Default.sendMessage(message, levelMap[(int)lml], "MyGUI_" + section);
        }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void MessageLoggedDelegate(String section, LogLevel lml, String message
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr ManagedMyGUILogListener_Create(MessageLoggedDelegate messageLoggedCallback
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManagedMyGUILogListener_Delete(IntPtr logListener);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static MessageLoggedDelegate messageLoggedDelegate;

            static CallbackHandler()
            {
                messageLoggedDelegate = new MessageLoggedDelegate(messageLogged);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(MessageLoggedDelegate))]
            private static void messageLogged(String section, LogLevel lml, String message, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ManagedMyGUILogListener).messageLogged(section, lml, message);
            }

            private GCHandle handle;

            public IntPtr create(ManagedMyGUILogListener obj)
            {
                handle = GCHandle.Alloc(obj);
                return ManagedMyGUILogListener_Create(messageLoggedDelegate, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else 
        class CallbackHandler : IDisposable
        {
            private MessageLoggedDelegate messageLoggedDelegate;

            public IntPtr create(ManagedMyGUILogListener obj)
            {
                messageLoggedDelegate = new MessageLoggedDelegate(obj.messageLogged);
                return ManagedMyGUILogListener_Create(messageLoggedDelegate);
            }

            public void Dispose()
            {

            }
        }
#endif

        #endregion
    }
}

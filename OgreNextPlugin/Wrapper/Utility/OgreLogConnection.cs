using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using System.Runtime.InteropServices;

namespace OgreNextPlugin
{
    enum LogMessageLevel
    {
        LML_TRIVIAL = 1,
        LML_NORMAL = 2,
        LML_CRITICAL = 3
    };

    class OgreLogConnection : IDisposable
    {
        IntPtr nativeLogListener;
        CallbackHandler callbackHandler;

        public OgreLogConnection()
        {
            callbackHandler = new CallbackHandler();
            nativeLogListener = callbackHandler.createInstance(this);
        }

        public void Dispose()
        {
            OgreLogListener_Delete(nativeLogListener);
            callbackHandler.Dispose();
        }

        public void subscribe()
        {
            OgreLogListener_subscribe(nativeLogListener);
        }

        private void messageLogged(String message, LogMessageLevel lml)
        {
            LogLevel level = LogLevel.Info;
            switch (lml)
            {
                case LogMessageLevel.LML_CRITICAL:
                    level = LogLevel.Error;
                    break;
                case LogMessageLevel.LML_NORMAL:
                    level = LogLevel.ImportantInfo;
                    break;
                case LogMessageLevel.LML_TRIVIAL:
                    level = LogLevel.Info;
                    break;
            }
            Log.Default.sendMessage(message, level, "Ogre");
        }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void MessageLoggedDelegate(String message, LogMessageLevel lml
#if FULL_AOT_COMPILE
    , IntPtr instanceHandle
#endif
);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OgreLogListener_Create(MessageLoggedDelegate messageLoggedCallback
#if FULL_AOT_COMPILE
    , IntPtr instanceHandle
#endif
);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgreLogListener_Delete(IntPtr logListener);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OgreLogListener_subscribe(IntPtr logListener);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static MessageLoggedDelegate messageLoggedCallback;

            static CallbackHandler()
            {
                messageLoggedCallback = new MessageLoggedDelegate(messageLogged);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(MessageLoggedDelegate))]
            private static void messageLogged(String message, LogMessageLevel lml, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as OgreLogConnection).messageLogged(message, lml);
            }

            private GCHandle gcHandle;

            public IntPtr createInstance(OgreLogConnection obj)
            {
                gcHandle = GCHandle.Alloc(obj);
                return OgreLogListener_Create(messageLoggedCallback, GCHandle.ToIntPtr(gcHandle));
            }

            public void Dispose()
            {
                gcHandle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            private MessageLoggedDelegate messageLoggedCallback;

            public IntPtr createInstance(OgreLogConnection obj)
            {
                messageLoggedCallback = new MessageLoggedDelegate(obj.messageLogged);
                return OgreLogListener_Create(messageLoggedCallback);
            }

            public void Dispose()
            {

            }
        }
#endif

        #endregion
    }
}

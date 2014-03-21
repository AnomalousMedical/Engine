using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    enum LogMessageLevel
    {
        LML_TRIVIAL = 1,
        LML_NORMAL = 2,
        LML_CRITICAL = 3
    };

    class OgreLogConnection : IDisposable
    {
        private MessageLoggedDelegate messageLoggedCallback;

        IntPtr nativeLogListener;

        public OgreLogConnection()
        {
            messageLoggedCallback = new MessageLoggedDelegate(messageLogged);
            nativeLogListener = OgreLogListener_Create(messageLoggedCallback);
        }

        public void Dispose()
        {
            OgreLogListener_Delete(nativeLogListener);
            messageLoggedCallback = null;
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
        private delegate void MessageLoggedDelegate(String message, LogMessageLevel lml);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OgreLogListener_Create(MessageLoggedDelegate messageLoggedCallback);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgreLogListener_Delete(IntPtr logListener);

        [DllImport("OgreCWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void OgreLogListener_subscribe(IntPtr logListener);

        #endregion
    }
}

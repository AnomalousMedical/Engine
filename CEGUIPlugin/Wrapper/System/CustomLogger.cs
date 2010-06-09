using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace CEGUIPlugin
{
    public enum LoggingLevel
    {
	    Errors,			//!< Only actual error conditions will be logged.
        Warnings,       //!< Warnings will be logged as well.
	    Standard,		//!< Basic events will be logged (default level).
	    Informative,	//!< Useful tracing (object creations etc) information will be logged.
	    Insane			//!< Mostly everything gets logged (use for heavy tracing only, log WILL be big).
    };

    class CustomLogger : IDisposable
    {
        private LogEventDelegate logEventCallback;
        private IntPtr log;

        public CustomLogger()
        {
            logEventCallback = new LogEventDelegate(logEvent);
            log = CustomLogger_create(logEventCallback);
        }

        public void Dispose()
        {
            CustomLogger_delete(log);
            logEventCallback = null;
        }

        void logEvent(String message, LoggingLevel level)
        {
            switch(level)
            {
                case LoggingLevel.Errors:
                    Log.Error(message);
                    break;
                case LoggingLevel.Warnings:
                    Log.Warning(message);
                    break;
                case LoggingLevel.Standard:
                    Log.Info(message);
                    break;
                case LoggingLevel.Informative:
                    Log.Info(message);
                    break;
                case LoggingLevel.Insane:
                    Log.Info(message);
                    break;
            }
        }

        public void setLogLevel(LoggingLevel level)
        {
            CustomLogger_setLoggingLevel(log, level);
        }

#region PInvoke
#endregion
        private delegate void LogEventDelegate(String message, LoggingLevel level);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr CustomLogger_create(LogEventDelegate logEventCallback);

        [DllImport("CEGUIWrapper")]
        private static extern void CustomLogger_delete(IntPtr log);

        [DllImport("CEGUIWrapper")]
        private static extern void CustomLogger_setLoggingLevel(IntPtr log, LoggingLevel level);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Logging
{
    /// <summary>
    /// The severity of the information logged.
    /// </summary>
    public enum LogLevel
    {
        Info = 1 << 0,
        ImportantInfo = 1 << 1,
        Warning = 1 << 2,
        Error = 1 << 3,
        Debug = 1 << 4,
        All = Info | ImportantInfo | Warning | Error | Debug,
    }

    /// <summary>
    /// Provides logging functionality.  This log does not write directly to a file
    /// or any other location.  In order to write somewhere you must extend the 
    /// LogListener interface and do your work there. The log is thread safe as long
    /// as its listeners are all thread safe, so you should make sure any custom listeners
    /// created are thread safe.
    /// </summary>
    /// <remarks>
    /// Messages can be split up based on 4 levels of importance: 
    /// Info for verbose information output.
    /// ImportantInfo for less verbose higher priority information.
    /// Warning for a problem that has a recoverable default that will not stop 
    /// execution but will likely cause undesirable results. 
    /// Error for a problem that will cause execution to stop or not start.
    /// These constants are provided by the LogLevel enum.
    /// </remarks>
    public class Log
    {
        private LinkedList<LogListener> logListeners = new LinkedList<LogListener>();
        private LogLevel messageTypes = LogLevel.All;

	    private static Log defaultLog = new Log();

        /// <summary>
        /// Static log that exists for the duration of the program for easy access.
        /// </summary>
        public static Log Default 
        { 
            get
            {
                return defaultLog;
            }
        }

	    /// <summary>
	    /// Constructor
	    /// </summary>
        public Log()
        {

        }

	    /// <summary>
	    /// Dispatch a message to the log.
	    /// </summary>
	    /// <param name="message">The log message.</param>
	    /// <param name="logLevel">The LogLevel of the message for filtering.</param>
	    /// <param name="subsystem">The subsystem the message originated from.</param>
        public void sendMessage(String message, LogLevel logLevel, String subsystem)
        {
            if((logLevel & messageTypes) != 0)
            {
		        LinkedListNode<LogListener> currentNode = logListeners.First;
		        while( currentNode != null )
		        {
			        currentNode.Value.sendMessage( message, logLevel, subsystem );
			        currentNode = currentNode.Next;
		        }
	        }
        }

	    /// <summary>
	    /// Dispatch a message to the log.  This version supports formatted strings.
	    /// </summary>
	    /// <param name="message">The log message.</param>
	    /// <param name="logLevel">The LogLevel of the message for filtering.</param>
	    /// <param name="subsystem">The subsystem the message originated from.</param>
	    /// <param name="args">Additional objects to go into the formatted message string.</param>
        public void sendMessage(String message, LogLevel logLevel, String subsystem, params object[] args)
        {
            sendMessage(String.Format(message, args), logLevel, subsystem);
        }

        /// <summary>
        /// Print an exception out to the log including all inner exceptions.
        /// </summary>
        /// <param name="e">The Exception to print.</param>
        public void printException(Exception e)
        {
            sendMessage("Exception: {0}.\n{1}\n{2}.", LogLevel.Error, "Anomaly", e.GetType().Name, e.Message, e.StackTrace);
            while (e.InnerException != null)
            {
                e = e.InnerException;
                sendMessage("--Inner exception: {0}.\n{1}\n{2}.", LogLevel.Error, "Anomaly", e.GetType().Name, e.Message, e.StackTrace);
            }
        }

        /// <summary>
        /// Shortcut to send a debug message.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void debug(String message)
        {
            sendMessage(message, LogLevel.Debug, "Debug");
        }

        /// <summary>
        /// Shortcut to send a deubg message. This version formats the message with string.format.
        /// </summary>
        /// <param name="message">A message will be formatted.</param>
        /// <param name="args">Additional objects to go into the formatted message string.</param>
        public void debug(String message, params object[] args)
        {
            sendMessage(String.Format(message, args), LogLevel.Debug, "Debug");
        }

	    /// <summary>
	    /// Adds a listener to the log
	    /// </summary>
	    /// <param name="listener">The LogListener to add.</param>
        public void addLogListener(LogListener listener)
        {
            logListeners.AddLast(listener);
        }

	    /// <summary>
	    /// Removes a listener from the log
	    /// </summary>
	    /// <param name="listener">The LogListener to remove.</param>
        public void removeLogListener(LogListener listener)
        {
            logListeners.Remove(listener);
        }

	    /// <summary>
	    /// Sets what message types are active.
	    /// </summary>
	    /// <param name="types">An or'd enum containing all the information levels to process.  The default is everything.</param>
        public void setActiveMessageTypes(LogLevel types)
        {
            messageTypes = types;
        }

        /// <summary>
        /// Helper function to get the log process output into a common format.
        /// </summary>
        /// <param name="message">The log message to process.</param>
        /// <param name="subsystem">The subsystem the message originated from.</param>
        /// <returns>A String formatted as "Timestamp [subsystem]: message"</returns>
        public static String formatMessage(String message, String subsystem)
        {
            return String.Format("{0} [{1}]: {2}", DateTime.Now.ToString(), subsystem, message);
        }

        /// <summary>
        /// Shortcut to send a debug message to the default log.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public static void Debug(String message)
        {
            defaultLog.sendMessage(message, LogLevel.Debug, AssemblyShim.CurrentAssemblyName);
        }

        /// <summary>
        /// Shortcut to send a deubg message to the default log. This version
        /// formats the message with string.format.
        /// </summary>
        /// <param name="message">A message will be formatted.</param>
        /// <param name="args">Additional objects to go into the formatted message string.</param>
        public static void Debug(String message, params object[] args)
        {
            defaultLog.sendMessage(String.Format(message, args), LogLevel.Debug, AssemblyShim.CurrentAssemblyName);
        }

        /// <summary>
        /// Shortcut to send an error message to the default log.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public static void Error(String message)
        {
            defaultLog.sendMessage(message, LogLevel.Error, AssemblyShim.CurrentAssemblyName);
        }

        /// <summary>
        /// Shortcut to send a error message to the default log. This version
        /// formats the message with string.format.
        /// </summary>
        /// <param name="message">A message will be formatted.</param>
        /// <param name="args">Additional objects to go into the formatted message string.</param>
        public static void Error(String message, params object[] args)
        {
            defaultLog.sendMessage(String.Format(message, args), LogLevel.Error, AssemblyShim.CurrentAssemblyName);
        }

        /// <summary>
        /// Shortcut to send a warning message to the default log.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public static void Warning(String message)
        {
            defaultLog.sendMessage(message, LogLevel.Warning, AssemblyShim.CurrentAssemblyName);
        }

        /// <summary>
        /// Shortcut to send a warning message to the default log. This version
        /// formats the message with string.format.
        /// </summary>
        /// <param name="message">A message will be formatted.</param>
        /// <param name="args">Additional objects to go into the formatted message string.</param>
        public static void Warning(String message, params object[] args)
        {
            defaultLog.sendMessage(String.Format(message, args), LogLevel.Warning, AssemblyShim.CurrentAssemblyName);
        }

        /// <summary>
        /// Shortcut to send an Info message to the default log.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public static void Info(String message)
        {
            defaultLog.sendMessage(message, LogLevel.Info, AssemblyShim.CurrentAssemblyName);
        }

        /// <summary>
        /// Shortcut to send an Info message to the default log. This version
        /// formats the message with string.format.
        /// </summary>
        /// <param name="message">A message will be formatted.</param>
        /// <param name="args">Additional objects to go into the formatted message string.</param>
        public static void Info(String message, params object[] args)
        {
            defaultLog.sendMessage(String.Format(message, args), LogLevel.Info, AssemblyShim.CurrentAssemblyName);
        }

        /// <summary>
        /// Shortcut to send an ImportantInfo message to the default log.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public static void ImportantInfo(String message)
        {
            defaultLog.sendMessage(message, LogLevel.ImportantInfo, AssemblyShim.CurrentAssemblyName);
        }

        /// <summary>
        /// Shortcut to send an ImportantInfo message to the default log. This version
        /// formats the message with string.format.
        /// </summary>
        /// <param name="message">A message will be formatted.</param>
        /// <param name="args">Additional objects to go into the formatted message string.</param>
        public static void ImportantInfo(String message, params object[] args)
        {
            defaultLog.sendMessage(String.Format(message, args), LogLevel.ImportantInfo, AssemblyShim.CurrentAssemblyName);
        }

        /// <summary>
        /// Print a stack trace out to debug.
        /// </summary>
        public static void PrintStackTrace()
        {
#if !FIXLATER_DISABLED
            StackTrace stackTrace = new StackTrace(1, true);
            StringBuilder sb = new StringBuilder(512);
            sb.AppendLine("Stack Trace");
            sb.AppendLine("----------------------");
            foreach (StackFrame frame in stackTrace.GetFrames())
            {
                MethodBase method = frame.GetMethod();
                sb.AppendLine(String.Format("{0}::{1}", method.ReflectedType != null ? method.ReflectedType.Name : string.Empty, method.Name));
            }
            sb.Append("----------------------");
            Log.Debug(sb.ToString());
#endif
        }
    }
}

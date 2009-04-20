using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        All = Info | ImportantInfo | Warning | Error,
    }

    /// <summary>
    /// Provides logging functionality.  This log does not write directly to a file
    /// or any other location.  In order to write somewhere you must extend the 
    /// LogListener interface and do your work there.
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
            if((logLevel | messageTypes) != 0)
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
    }
}

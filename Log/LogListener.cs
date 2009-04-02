using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging
{
    /// <summary>
    /// Interface for LogListeners. The subclasses can handle log messages and
    /// output them to a file or write them to the console or the screen.
    /// </summary>
    public interface LogListener
    {
        /// <summary>
        /// Dispatch a message to the listener
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="logLevel">The LogLevel of the message for filtering.</param>
        /// <param name="subsystem">The subsystem the message originated from.</param>
        void sendMessage(String message, LogLevel logLevel, String subsystem);
    }
}

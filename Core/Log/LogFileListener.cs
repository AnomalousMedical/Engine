using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Logging
{
    /// <summary>
    /// This class will write the log messages to a file.
    /// </summary>
    public class LogFileListener : LogListener, IDisposable
    {
        private StreamWriter fileWriter;
        private bool closed;

        public LogFileListener()
        {

        }

        public void Dispose()
        {
            closeLogFile();
        }

        /// <summary>
        /// Gets a message from the log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logLevel">The importance of the message.</param>
        /// <param name="subsystem">The subsystem the message originated from.</param>
        public void sendMessage(string message, LogLevel logLevel, string subsystem)
        {
            if (!closed)
            {
                fileWriter.WriteLine(Log.formatMessage(message, subsystem));
                fileWriter.Flush();
            }
        }

        /// <summary>
        /// Opens a file to write the log messages to.  Should be opened before any log
        /// messages are sent.
        /// </summary>
        /// <param name="fileName"></param>
        public void openLogFile(String fileName)
        {
            try
            {
                fileWriter = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write));
                closed = false;
            }
            catch
            {
                closed = true;
            }
        }

        /// <summary>
        /// Closes the log file.
        /// </summary>
        public void closeLogFile()
        {
            if (!closed)
            {
                closed = true;
                fileWriter.Close();
            }
        }
    }
}


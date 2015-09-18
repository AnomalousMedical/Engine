using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;

namespace Logging
{
    /// <summary>
    /// This class will write the log messages to a file.
    /// </summary>
    public class LogFileListener : LogListener, IDisposable
    {
        private StreamWriter fileWriter;
        private bool closed;
        private String logFileName;
        private ConcurrentQueue<String> messageQueue = new ConcurrentQueue<string>();
        private Object streamLock = new object();
        private ManualResetEventSlim messageEvent = new ManualResetEventSlim(true);
        private bool runPrintThread = true;

        class LogEntry
        {
            public string message;
            public LogLevel logLevel;
            public string subsystem;
        }

        public LogFileListener()
        {
            Thread t = new Thread(() =>
            {
                while (runPrintThread)
                {
                    String message;
                    if (messageQueue.TryDequeue(out message))
                    {
                        lock (streamLock)
                        {
                            if (!closed)
                            {
                                fileWriter.WriteLine(message);
                                fileWriter.Flush();
                            }
                        }
                    }
                    else
                    {
                        messageEvent.Reset(); //Wait until there are more messages
                    }
                    messageEvent.Wait();
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void Dispose()
        {
            lock(streamLock)
            {
                runPrintThread = false;
                messageEvent.Dispose();
                if (!closed)
                {
                    Log.Info("Closed log {0}", logFileName);
                    closed = true;
                    fileWriter.Dispose();
                }
            }
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
                messageQueue.Enqueue(Log.formatMessage(message, subsystem));
                messageEvent.Set(); //We have a message alert the printing thread
            }
        }

        /// <summary>
        /// Opens a file to write the log messages to.  Should be opened before any log
        /// messages are sent.
        /// </summary>
        /// <param name="fileName"></param>
        public void openLogFile(String fileName)
        {
            lock (streamLock)
            {
                try
                {
                    logFileName = fileName;
                    fileWriter = new StreamWriter(File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read));
                    closed = false;
                }
                catch
                {
                    closed = true;
                }
            }
        }

        public void saveCrashLog(String crashFile)
        {
            lock (streamLock)
            {
                try
                {
                    String crashDir = Path.GetDirectoryName(crashFile);
                    if (!Directory.Exists(crashDir))
                    {
                        Directory.CreateDirectory(crashDir);
                    }
                    
                    fileWriter.Flush();
                    DateTime now = DateTime.Now;
                    File.Copy(logFileName, crashFile, true);
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}


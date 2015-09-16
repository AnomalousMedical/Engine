using Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// This class makes it easy to use a stopwatch that writes its time to the log with a given format string.
    /// The idea is to use it in a using block so you don't have to modify functions to test too much.
    /// </summary>
    public class LogPerformanceBlock : IDisposable
    {
        private String logFormat;
        private Stopwatch sw = new Stopwatch();
        private LogLevel logLevel;
        private String subsystem;

        /// <summary>
        /// Create a new LogPerformanceBlock, pass in the format of the string you want to use. Put a {0} where you want the time in milliseconds.
        /// </summary>
        /// <param name="logFormat"></param>
        public LogPerformanceBlock(String logFormat, LogLevel logLevel = LogLevel.Debug, String subsystem = "Performance")
        {
            this.logFormat = logFormat;
            this.logLevel = logLevel;
            this.subsystem = subsystem;
            sw.Start();
        }

        public void Dispose()
        {
            sw.Stop();
            Log.Default.sendMessage(String.Format(logFormat, sw.ElapsedMilliseconds), logLevel, subsystem);
        }
    }
}

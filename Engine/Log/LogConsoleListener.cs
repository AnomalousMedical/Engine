using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging
{
    public class LogConsoleListener : LogListener
    {
        public void sendMessage(string message, LogLevel logLevel, string subsystem)
        {
            Console.WriteLine(Log.formatMessage(message, subsystem));
        }
    }
}

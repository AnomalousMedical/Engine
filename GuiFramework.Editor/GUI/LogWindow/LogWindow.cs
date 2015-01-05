using Engine.Threads;
using Logging;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework.Editor
{
    public class LogWindow : MDIDialog, LogListener
    {
        private EditBox log;
        private StringBuilder sb;
        private int maxTextLength;

        public LogWindow(int maxTextLength = 10000)
            : base("Anomalous.GuiFramework.Editor.GUI.LogWindow.LogWindow.layout")
        {
            this.maxTextLength = maxTextLength;
            log = window.findWidget("Log") as EditBox;
            log.MaxTextLength = (uint)maxTextLength;
            sb = new StringBuilder(maxTextLength);
        }

        public void sendMessage(string message, LogLevel logLevel, string subsystem)
        {
            ThreadManager.invoke(new Action(() =>
                {
                    if (!window.IsDisposed)
                    {
                        String colorCode = "#000000";
                        switch (logLevel)
                        {
                            case LogLevel.Error:
                                colorCode = "#ff0000";
                                break;
                            case LogLevel.Warning:
                                colorCode = "#cd810a";
                                break;
                            case LogLevel.Info:
                                colorCode = "#000000";
                                break;
                            case LogLevel.ImportantInfo:
                                colorCode = "#000000";
                                break;
                            case LogLevel.Debug:
                                colorCode = "#2cae0a";
                                break;
                        }
                        String line = colorCode + Log.formatMessage(message, subsystem); //Yea this sucks
                        int toRemove = sb.Length + line.Length - maxTextLength;
                        if(toRemove > 0)
                        {
                            sb.Remove(0, toRemove);
                        }
                        sb.AppendLine(line);
                        log.Caption = sb.ToString();
                    }
                }));
        }
    }
}

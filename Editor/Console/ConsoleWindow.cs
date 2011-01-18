using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Logging;
using System.Runtime.InteropServices;

namespace Editor
{
    public partial class ConsoleWindow : DockContent, LogListener
    {
        const int SB_VERT = 1;
        const int EM_SETSCROLLPOS = 0x0400 + 222;

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, POINT lParam);

        [StructLayout(LayoutKind.Sequential)]
        private class POINT
        {
            public int x;
            public int y;

            public POINT()
            {
            }

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        } 

        public ConsoleWindow()
        {
            InitializeComponent();
        }

        #region LogListener Members

        public void sendMessage(string message, LogLevel logLevel, string subsystem)
        {
            if (!consoleText.IsDisposed)
            {
                switch (logLevel)
                {
                    case LogLevel.Error:
                        consoleText.SelectionColor = Color.Red;
                        break;
                    case LogLevel.Warning:
                        consoleText.SelectionColor = Color.Orange;
                        break;
                    case LogLevel.Info:
                        consoleText.SelectionColor = Color.Black;
                        break;
                    case LogLevel.ImportantInfo:
                        consoleText.SelectionColor = Color.Black;
                        break;
                    case LogLevel.Debug:
                        consoleText.SelectionColor = Color.DarkGreen;
                        break;
                }
                consoleText.SelectedText += Log.formatMessage(message, subsystem) + "\n";

                int min, max;
                GetScrollRange(consoleText.Handle, SB_VERT, out min, out max);
                SendMessage(consoleText.Handle, EM_SETSCROLLPOS, 0, new POINT(0, max - consoleText.Height)); 
            }
        }

        #endregion
    }
}

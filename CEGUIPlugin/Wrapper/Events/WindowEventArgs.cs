using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEGUIPlugin
{
    public class WindowEventArgs : EventArgs
    {
        public WindowEventArgs()
        {

        }

        public WindowEventArgs(Window window)
        {
            this.Window = window;
        }

        public Window Window { get; internal set; }
    }
}

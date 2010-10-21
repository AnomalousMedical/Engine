using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class CanvasEventArgs : EventArgs
    {
        public int Left { get; internal set; }

        public int Top { get; internal set; }
    }
}

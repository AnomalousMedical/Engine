using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class ScrollGestureEventArgs : EventArgs
    {
        public int AbsX { get; internal set; }
        
        public int AbsY { get; internal set; }

        public int DeltaX { get; internal set; }
        
        public int DeltaY { get; internal set; }
    }
}

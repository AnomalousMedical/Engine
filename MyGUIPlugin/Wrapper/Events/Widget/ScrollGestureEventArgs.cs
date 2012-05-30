using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class ScrollGestureEventArgs : EventArgs
    {
        public ScrollGestureEventArgs()
        {

        }

        public ScrollGestureEventArgs(int absx, int absy, int deltax, int deltay)
        {
            this.AbsX = absx;
            this.AbsY = absy;
            this.DeltaX = deltax;
            this.DeltaY = deltay;
        }

        public int AbsX { get; internal set; }
        
        public int AbsY { get; internal set; }

        public int DeltaX { get; internal set; }
        
        public int DeltaY { get; internal set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;

namespace MyGUIPlugin
{
    public class MouseEventArgs : EventArgs
    {
        public Vector2 Position { get; internal set; }

        public MouseButtonCode Button { get; internal set; }

        public int RelativeWheelPosition { get; internal set; }
    }
}

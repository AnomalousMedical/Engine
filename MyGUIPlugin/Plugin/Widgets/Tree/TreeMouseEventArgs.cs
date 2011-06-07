using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;

namespace MyGUIPlugin
{
    public class TreeMouseEventArgs : TreeEventArgs
    {
        private MouseEventArgs me;

        public void setData(MouseEventArgs me)
        {
            this.me = me;
        }

        public IntVector2 MousePosition
        {
            get
            {
                return me.Position;
            }
        }

        public int MouseWheelRelative
        {
            get
            {
                return me.RelativeWheelPosition;
            }
        }

        public MouseButtonCode Button
        {
            get
            {
                return me.Button;
            }
        }
    }
}

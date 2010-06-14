using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace CEGUIPlugin
{
    public class MouseEventArgs : WindowEventArgs
    {
        public MouseEventArgs()
        {

        }

        public MouseEventArgs(Window window, Vector2 position, Vector2 moveDelta, MouseButton button, uint sysKeys, float wheelChange, uint clickCount)
            :base(window)
        {
            Position = position;
            MoveDelta = moveDelta;
            Button = button;
            SysKeys = sysKeys;
            WheelChange = wheelChange;
            ClickCount = clickCount;
        }

        public Vector2 Position { get; internal set; }

        public Vector2 MoveDelta { get; internal set; }

        public MouseButton Button { get; internal set; }

        public uint SysKeys { get; internal set; }

        public float WheelChange { get; internal set; }

        public uint ClickCount { get; internal set; }
    }
}

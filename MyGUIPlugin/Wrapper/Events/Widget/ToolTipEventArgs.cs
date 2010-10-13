using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public enum ToolTipType
    {
        Hide,
        Show
    };

    public class ToolTipEventArgs : EventArgs
    {
        public ToolTipType Type { get; internal set; }

        public uint Index { get; internal set; }

        public Vector2 Point { get; internal set; }
    }
}

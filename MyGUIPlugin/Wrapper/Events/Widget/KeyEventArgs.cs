using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace MyGUIPlugin
{
    public class KeyEventArgs : EventArgs
    {
        public KeyboardButtonCode Key { get; internal set; }

        public char Char { get; internal set; }
    }
}

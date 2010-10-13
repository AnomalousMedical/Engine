using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class RootFocusEventArgs : EventArgs
    {
        public bool Focus { get; internal set; }
    }
}

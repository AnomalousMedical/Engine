using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class ComboBoxEventArgs : EventArgs
    {
        public uint Index { get; internal set; }
    }
}

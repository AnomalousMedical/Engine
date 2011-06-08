using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class MenuCtrlAcceptEventArgs : EventArgs
    {
        public MenuItem Item { get; internal set; }
    }
}

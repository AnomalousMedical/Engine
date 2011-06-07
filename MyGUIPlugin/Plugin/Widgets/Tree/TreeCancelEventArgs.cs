using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class TreeCancelEventArgs : TreeEventArgs
    {
        public TreeCancelEventArgs()
        {
            reset();
        }

        public void reset()
        {
            Cancel = false;
        }

        public bool Cancel { get; set; }
    }
}

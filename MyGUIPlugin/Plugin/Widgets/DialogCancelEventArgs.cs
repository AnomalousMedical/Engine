using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class DialogCancelEventArgs : EventArgs
    {
        public DialogCancelEventArgs()
        {
            Cancel = false;
        }

        public bool Cancel { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class MultiListEventArgs : EventArgs
    {
        internal MultiListEventArgs()
        {

        }

        public uint Index { get; internal set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class CancelEventArgs : EventArgs
    {
        public CancelEventArgs()
        {
            reset();
        }

        public virtual void reset()
        {
            Cancel = false;
        }

        public bool Cancel { get; set; }
    }
}

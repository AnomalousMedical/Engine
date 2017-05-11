using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Shim
{
    /// <summary>
    /// Once netstandard 2.0 comes out we can get rid of this hack
    /// </summary>
    public class ElapsedEventArgs
    {
        public ElapsedEventArgs(DateTime signalTime)
        {
            this.SignalTime = signalTime;
        }

        public DateTime SignalTime { get; private set; }
    }
}

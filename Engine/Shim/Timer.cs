using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Shim
{
    /// <summary>
    /// Once netstandard 2.0 comes out we can get rid of this hack
    /// Shim for System.Timers.Timer
    /// </summary>
    public interface Timer : IDisposable
    {
        bool AutoReset { get; set; }

        event Action<object, ElapsedEventArgs> Elapsed;

        void Dispose();

        void Stop();

        void Start();
    }
}

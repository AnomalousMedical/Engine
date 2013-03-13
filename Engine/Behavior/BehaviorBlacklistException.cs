using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This exception is thrown by the behavior blacklist function. This will
    /// cause the function that calls blacklist to terminate immediatly.
    /// </summary>
    class BehaviorBlacklistException : Exception
    {
        public BehaviorBlacklistException(String message, Behavior behavior)
            :base(message)
        {
            this.Behavior = behavior;
        }

        public Behavior Behavior { get; private set; }
    }
}

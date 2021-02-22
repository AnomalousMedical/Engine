using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This interface can be injected if an object wishes to offer a destroy method for
    /// its users. This will schedule the object for Dispose on the next call to IObjectResolver.Flush.
    /// </summary>
    public interface IDestructionRequest
    {
        /// <summary>
        /// Request the owning object be destroyed on the next flush.
        /// </summary>
        void RequestDestruction();
    }
}

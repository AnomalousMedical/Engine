using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This interface defines the common functions for a class that maintains
    /// objects created by a subsystem.
    /// </summary>
    public interface SimComponentManager : IDisposable
    {
        /// <summary>
        /// Set this manager as the target object that constructed objects will
        /// be placed in by the factory.
        /// </summary>
        void setAsConstructionTarget();

        /// <summary>
        /// Make this manager no longer the target object for constructed
        /// objects.
        /// </summary>
        void unsetAsConstructionTarget();
    }
}

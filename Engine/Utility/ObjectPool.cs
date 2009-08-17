using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// Abstract base class for pools.
    /// </summary>
    public abstract class ObjectPool
    {
        /// <summary>
        /// Internal function to return finished objects.
        /// </summary>
        /// <param name="finished">The object to return to the pool.</param>
        internal abstract void returnObject(PooledObject finished);
    }
}

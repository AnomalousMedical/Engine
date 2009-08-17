using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// Abstract base class for pooled objects.
    /// </summary>
    public abstract class PooledObject
    {
        private ObjectPool pool;

        /// <summary>
        /// Call this function when finished with the object to return it to the pool.
        /// </summary>
        protected void returnToPool()
        {
            pool.returnObject(this);
        }

        /// <summary>
        /// Call the protected reset function.
        /// </summary>
        internal void callReset()
        {
            reset();
        }

        /// <summary>
        /// Reset function. Clear the subclass for reuse in the pool here.
        /// </summary>
        protected abstract void reset();

        /// <summary>
        /// The pool this object belongs to.
        /// </summary>
        internal ObjectPool Pool
        {
            get
            {
                return pool;
            }
            set
            {
                pool = value;
            }
        }
    }
}

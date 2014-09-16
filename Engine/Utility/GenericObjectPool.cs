using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// A pool of objects. This class is thread safe, so you can take objects out of it on multiple threads.
    /// </summary>
    /// <typeparam name="PooledType"></typeparam>
    public class GenericObjectPool<PooledType> : ObjectPool
        where PooledType : PooledObject, new()
    {
        ConcurrentStack<PooledType> pool = new ConcurrentStack<PooledType>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public GenericObjectPool()
        {

        }

        /// <summary>
        /// Retrieve an object from the pool.
        /// </summary>
        /// <returns>A pooled object.</returns>
        public PooledType getPooledObject()
        {
            PooledType ret;
            if (!pool.TryPop(out ret))
            {
                ret = new PooledType();
                ret.Pool = this;
            }
            return ret;
        }

        /// <summary>
        /// Internal function to return objects to the pool.
        /// </summary>
        /// <param name="finished">The object to be returned.</param>
        internal override void returnObject(PooledObject finished)
        {
            finished.callReset();
            pool.Push((PooledType)finished);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// A pool of objects.
    /// </summary>
    /// <typeparam name="PooledType"></typeparam>
    public class GenericObjectPool<PooledType> : ObjectPool
        where PooledType : PooledObject, new()
    {
        Stack<PooledType> pool = new Stack<PooledType>();

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
            if (pool.Count == 0)
            {
                PooledType ret = new PooledType();
                ret.Pool = this;
                return ret;
            }
            return pool.Pop();
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

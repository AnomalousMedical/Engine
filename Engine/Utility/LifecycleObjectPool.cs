using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class LifecycleObjectPool<PooledType> : ObjectPool, IDisposable
        where PooledType : PooledObject
    {
        Stack<PooledType> pool = new Stack<PooledType>();

        Func<PooledType> CreatePooledObject;
        Action<PooledType> DestroyPooledObject { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LifecycleObjectPool(Func<PooledType> createPooledObject, Action<PooledType> destroyPooledObject)
        {
            this.CreatePooledObject = createPooledObject;
            this.DestroyPooledObject = destroyPooledObject;
        }

        public void Dispose()
        {
            foreach(PooledType item in pool)
            {
                DestroyPooledObject(item);
            }
        }

        /// <summary>
        /// Retrieve an object from the pool.
        /// </summary>
        /// <returns>A pooled object.</returns>
        public PooledType getPooledObject()
        {
            if (pool.Count == 0)
            {
                PooledType ret = CreatePooledObject();
                ret.Pool = this;
                return ret;
            }
            return pool.Pop();
        }

        /// <summary>
        /// Set the max pool size. If objects are returned to the pool and it already has MaxPoolSize objects
        /// in it the additional objects will be cleaned up instead of pooled.
        /// </summary>
        public int? MaxPoolSize { get; set; }

        /// <summary>
        /// Internal function to return objects to the pool.
        /// </summary>
        /// <param name="finished">The object to be returned.</param>
        internal override void returnObject(PooledObject finished)
        {
            if (!MaxPoolSize.HasValue || pool.Count < MaxPoolSize)
            {
                finished.callReset();
                pool.Push((PooledType)finished); 
            }
            else //Too many objects for pool, cleanup object instead.
            {
                DestroyPooledObject((PooledType)finished);
            }
        }
    }
}

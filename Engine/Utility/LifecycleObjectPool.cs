using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// An object pool where the pooled objects need lifecycle functions called. This class is thread safe to take
    /// objects out of and return to on multiple threads. It is not predictable what thread the destroyPooledObject callbac
    /// will happen on.
    /// </summary>
    /// <typeparam name="PooledType"></typeparam>
    public class LifecycleObjectPool<PooledType> : ObjectPool, IDisposable
        where PooledType : PooledObject
    {
        ConcurrentStack<PooledType> pool = new ConcurrentStack<PooledType>();

        Func<PooledType> CreatePooledObject;
        Action<PooledType> DestroyPooledObject { get; set; }
        private bool stillAlive = true;

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
            stillAlive = false;
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
            PooledType ret;
            if(!pool.TryPop(out ret))
            {
                ret = CreatePooledObject();
                ret.Pool = this;
            }
            return ret;
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
            if (stillAlive && (!MaxPoolSize.HasValue || pool.Count < MaxPoolSize))
            {
                finished.callReset();
                pool.Push((PooledType)finished); 
            }
            else //Too many objects for pool or pool is disposed, cleanup object instead.
            {
                DestroyPooledObject((PooledType)finished);
            }
        }
    }
}

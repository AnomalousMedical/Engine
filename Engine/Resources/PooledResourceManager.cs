using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// This class makes it easier to create a pool of resources that must be disposed. Users
    /// can Checkout and Return the items they need and when all of the checked out items
    /// are returned the pooled instance will be disposed.
    /// </summary>
    /// <typeparam name="TPooled"></typeparam>
    public class PooledResourceManager<TKey, TPooled>
    {
        class Entry
        {
            public IDisposable disposable;
            public TPooled pooled;
            public int count;
            public TKey key;
            public Task<CreationResult> task;
        }

        public class CreationResult
        {
            public TPooled pooled;
            public IDisposable disposable;
        }

        private Dictionary<TPooled, Entry> pooledToEntries = new Dictionary<TPooled, Entry>();
        private Dictionary<TKey, Entry> keysToEntries = new Dictionary<TKey, Entry>();

        public PooledResourceManager()
        {

        }

        /// <summary>
        /// Convenience method to create a result from a pooled object and disposable.
        /// This is useful for things like AutoPtr that are wrapped by a disposable.
        /// You can return the Obj as the pooled item and the pointer as the disposable.
        /// Only the pooled item will be visible.
        /// </summary>
        /// <param name="pooled">The pooled item to return.</param>
        /// <param name="disposable">The disposable view of the item.</param>
        /// <returns></returns>
        public CreationResult CreateResult(TPooled pooled, IDisposable disposable)
        {
            return new CreationResult()
            {
                pooled = pooled,
                disposable = disposable,
            };
        }

        /// <summary>
        /// Convenience method to create a result from a pooled object that is disposable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pooled"></param>
        /// <returns></returns>
        public CreationResult CreateResult<T>(T pooled)
            where T : TPooled, IDisposable
        {
            return new CreationResult()
            {
                pooled = pooled,
                disposable = pooled,
            };
        }

        /// <summary>
        /// Checkout a pooled object. It is assumed that this will be happening on the main render thread not
        /// a background thread. The synchronization in this class is not thread safe.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<TPooled> Checkout(TKey key, Func<Task<CreationResult>> create)
        {
            Entry entry;
            if (!keysToEntries.TryGetValue(key, out entry))
            {
                entry = new Entry()
                {
                    key = key,
                    task = create()
                };
                keysToEntries[key] = entry;
                var created = await entry.task;
                entry.task = null; //Clear the task on the entry after finished
                entry.pooled = created.pooled;
                entry.disposable = created.disposable;
                pooledToEntries[entry.pooled] = entry;
            }
            else if(entry.task != null)
            {
                await entry.task;
            }
            ++entry.count;
            return entry.pooled;
        }

        /// <summary>
        /// Return a pooled object. This is assumed to be happening on the main thread. The synchronization
        /// will not work if this is run from a background thread.
        /// </summary>
        /// <param name="pooled"></param>
        public void Return(TPooled pooled)
        {
            if (pooledToEntries.TryGetValue(pooled, out var entry))
            {
                --entry.count;
                if (entry.count == 0)
                {
                    pooledToEntries.Remove(pooled);
                    keysToEntries.Remove(entry.key);
                    entry.disposable.Dispose();
                }
            }
            else
            {
                throw new InvalidOperationException($"A {nameof(TPooled)} was returned that did not have an entry. Do not return items you did not check out and do not return items more than once.");
            }
        }
    }
}

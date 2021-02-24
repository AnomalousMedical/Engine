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
    public abstract class PooledResourceManager<TPooled>
    {
        class Entry
        {
            public IDisposable disposable;
            public TPooled pooled;
            public int count;
            public string name;
            public Task<CreateResult> task;
        }

        protected class CreateResult
        {
            public TPooled pooled;
            public IDisposable disposable;
        }

        private Dictionary<TPooled, Entry> pooledToEntries = new Dictionary<TPooled, Entry>();
        private Dictionary<String, Entry> namesToEntries = new Dictionary<String, Entry>();

        private readonly ILogger<PooledResourceManager<TPooled>> logger;

        public PooledResourceManager(ILogger<PooledResourceManager<TPooled>> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Create a new pooled object. This will only be called once per baseName. This must
        /// set the pooled object and the disposable. These can be the same object, but are given
        /// separately in case of something like an AutoPtr that has a value. The value can be
        /// returned as pooled and the AutoPtr as disposable and the AutoPtr will be disposed
        /// at the correct time.
        /// </summary>
        /// <param name="key">The base name of the resource to load.</param>
        /// <param name="pooled">The pooled item created.</param>
        /// <param name="disposable">The disposable item created. Can be the same as pooled.</param>
        /// <returns></returns>
        protected abstract Task<CreateResult> Create(String key);

        /// <summary>
        /// If you want to modify the key in some way, you can do it here and this will be called
        /// before any processing is done.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual string ModifyKey(String key)
        {
            return key;
        }

        public async Task<TPooled> Checkout(String key)
        {
            key = ModifyKey(key);
            Entry entry;
            if (!namesToEntries.TryGetValue(key, out entry))
            {
                entry = new Entry()
                {
                    name = key,
                    task = Create(key)
                };
                namesToEntries[key] = entry;
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

        public void Return(TPooled pooled)
        {
            if (pooledToEntries.TryGetValue(pooled, out var entry))
            {
                --entry.count;
                if (entry.count == 0)
                {
                    pooledToEntries.Remove(pooled);
                    namesToEntries.Remove(entry.name);
                    entry.disposable.Dispose();
                }
            }
            else
            {
                logger.LogInformation($"A {nameof(TPooled)} was returned that did not have an entry. Do not return items you did not check out and do not return items more than once.");
            }
        }
    }
}

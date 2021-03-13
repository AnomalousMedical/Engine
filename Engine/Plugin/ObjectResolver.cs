using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    class ObjectResolver : IObjectResolver
    {
        private readonly ServiceProvider serviceProvider;
        private readonly ObjectResolverFactory factory;
        private List<IDisposable> resolvedObjects = new List<IDisposable>();
        private List<IDisposable> destructionQueue = new List<IDisposable>();

        public ObjectResolver(ServiceProvider serviceProvider, ObjectResolverFactory factory)
        {
            this.serviceProvider = serviceProvider;
            this.factory = factory;
        }

        public void Dispose()
        {
            var count = resolvedObjects.Count;
            while(count > 0)
            {
                resolvedObjects[--count].Dispose();
            }
            //Do not have to deal with destructionQueue here.
            //If those have not yet been flushed they will have been destroyed
            //as part of the resolved objects above. Items are only removed
            //from that collection when their scope is disposed.
            factory.AlertDestroyed(this);
        }

        internal void QueueDestroy(ResolvedObject resolvedObject)
        {
            destructionQueue.Add(resolvedObject);
        }

        public T Resolve<T>()
        {
            var scope = serviceProvider.CreateScope();
            var instance = scope.ServiceProvider.GetRequiredService<T>();
            var resolved = scope.ServiceProvider.GetRequiredService<ResolvedObject>();
            resolved.Scope = scope;
            resolved.ObjectResolver = this;
            resolvedObjects.Add(resolved);

            return instance;
        }

        public T Resolve<T, TConfig>(Action<TConfig> configure)
        {
            var scope = serviceProvider.CreateScope();
            var options = scope.ServiceProvider.GetRequiredService<TConfig>();
            configure(options);
            var instance = scope.ServiceProvider.GetRequiredService<T>();
            var resolved = scope.ServiceProvider.GetRequiredService<ResolvedObject>();
            resolved.Scope = scope;
            resolved.ObjectResolver = this;
            resolvedObjects.Add(resolved);

            return instance;
        }

        /// <summary>
        /// Flush out all destruction requested objects and actually destroy them.
        /// If this is never called the objects are not really destroyed until
        /// the program resolver is destroyed.
        /// </summary>
        internal void Flush()
        {
            foreach(var item in destructionQueue)
            {
                item.Dispose();
            }
            destructionQueue.Clear();
        }

        internal void Remove(IDisposable resolvedObject)
        {
            resolvedObjects.Remove(resolvedObject);
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    class ObjectResolver : IObjectResolver
    {
        private readonly ServiceProvider serviceProvider;
        private List<IDisposable> resolvedObjects = new List<IDisposable>();

        public ObjectResolver(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            var count = resolvedObjects.Count;
            while(count > 0)
            {
                resolvedObjects[--count].Dispose();
            }
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

        internal void Remove(IDisposable resolvedObject)
        {
            resolvedObjects.Remove(resolvedObject);
        }
    }
}

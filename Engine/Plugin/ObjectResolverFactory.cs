using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    class ObjectResolverFactory : IObjectResolverFactory
    {
        private readonly ServiceProvider serviceProvider;
        private List<ObjectResolver> resolvers = new List<ObjectResolver>();
        private int flushIndex = -1;

        public ObjectResolverFactory(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IObjectResolver Create()
        {
            var resolver = new ObjectResolver(serviceProvider, this);
            resolvers.Add(resolver);
            return resolver;
        }

        public void Flush()
        {
            for (flushIndex = 0; flushIndex < resolvers.Count; ++flushIndex)
            {
                resolvers[flushIndex].Flush();
            }
        }

        internal void AlertDestroyed(ObjectResolver objectResolver)
        {
            int index = resolvers.IndexOf(objectResolver);
            if (index != -1)
            {
                resolvers.RemoveAt(index);
                //Adjust the iteration index backwards if the element being removed is before or on the index.
                //This way nothing gets skipped.
                if (index <= flushIndex)
                {
                    --flushIndex;
                }
            }
        }
    }
}

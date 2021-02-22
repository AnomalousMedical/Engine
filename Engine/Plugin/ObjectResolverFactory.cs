using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    class ObjectResolverFactory : IObjectResolverFactory
    {
        private readonly ServiceProvider serviceProvider;

        public ObjectResolverFactory(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IObjectResolver Create()
        {
            return new ObjectResolver(serviceProvider);
        }
    }
}

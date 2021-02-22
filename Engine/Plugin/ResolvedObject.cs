using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    class ResolvedObject : IDisposable
    {
        public ResolvedObject(DestructionRequest destructionRequest)
        {
            destructionRequest.Destroy = Dispose;
        }

        public void Dispose()
        {
            ObjectResolver.Remove(this);
            Scope.Dispose();
        }

        internal IServiceScope Scope { get; set; }

        internal ObjectResolver ObjectResolver { get; set; }
    }
}

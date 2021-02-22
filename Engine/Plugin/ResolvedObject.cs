using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This class is what holds the binding between the resolved object
    /// and the scope. It isn't accessed by the client directly.
    /// </summary>
    class ResolvedObject : IDisposable
    {
        public ResolvedObject(DestructionRequest destructionRequest)
        {
            destructionRequest.Destroy = QueueDestroy;
        }

        public void Dispose()
        {
            ObjectResolver.Remove(this);
            Scope.Dispose();
            //If the resolved object implements dispose it will have been called
            //Dispose will also have been called for any scoped objects in the 
            //object scope.
        }

        private void QueueDestroy()
        {
            ObjectResolver.QueueDestroy(this);
        }

        internal IServiceScope Scope { get; set; }

        internal ObjectResolver ObjectResolver { get; set; }
    }
}

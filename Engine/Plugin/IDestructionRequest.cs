using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public interface IDestructionRequest
    {
        void RequestDestruction();
    }

    class DestructionRequest : IDestructionRequest
    {
        internal Action Destroy;

        void IDestructionRequest.RequestDestruction()
        {
            Destroy.Invoke();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public interface IDestructionRequest
    {
        void Destroy();
    }

    class DestructionRequest : IDestructionRequest
    {
        internal Action Destroy;

        void IDestructionRequest.Destroy()
        {
            Destroy.Invoke();
        }
    }
}

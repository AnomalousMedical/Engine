using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    class DestructionRequest : IDestructionRequest
    {
        internal Action Destroy;

        void IDestructionRequest.RequestDestruction()
        {
            Destroy.Invoke();
        }
    }
}

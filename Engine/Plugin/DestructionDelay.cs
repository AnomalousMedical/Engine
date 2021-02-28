using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    class DestructionDelay : IDisposable
    {
        private DestructionRequest heldRequest;

        public DestructionDelay(DestructionRequest heldRequest)
        {
            this.heldRequest = heldRequest;
        }

        public void Dispose()
        {
            heldRequest?.RemoveBlock();
            heldRequest = null; //Prevent duplicate returns
        }
    }
}

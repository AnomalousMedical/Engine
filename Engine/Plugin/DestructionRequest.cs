using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    class DestructionRequest : IDestructionRequest
    {
        internal Action Destroy;
        private int outstandingBlocks = 0;
        private bool destructionRequested = false;

        public void RequestDestruction()
        {
            destructionRequested = true;
            if (outstandingBlocks == 0)
            {
                Destroy.Invoke();
            }
        }

        public IDisposable BlockDestruction()
        {
            ++outstandingBlocks;
            return new DestructionDelay(this);
        }

        internal void RemoveBlock()
        {
            --outstandingBlocks;
            if (destructionRequested)
            {
                RequestDestruction();
            }
        }

        public bool DestructionRequested => destructionRequested;
    }
}

using DilligentEngine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DilligentEngineTest
{
    class SimpleUpdateListener : UpdateListener
    {
        private readonly GenericEngineFactory genericEngineFactory;
        private readonly ISwapChain swapChain;

        public SimpleUpdateListener(GenericEngineFactory genericEngineFactory, ISwapChain swapChain)
        {
            this.genericEngineFactory = genericEngineFactory;
            this.swapChain = swapChain;
        }

        public void exceededMaxDelta()
        {
            
        }

        public void loopStarting()
        {
            
        }

        public void sendUpdate(Clock clock)
        {
            genericEngineFactory.LazyRender();
            this.swapChain.Present();
        }
    }
}

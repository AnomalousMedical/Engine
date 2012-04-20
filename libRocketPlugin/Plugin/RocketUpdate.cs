using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace libRocketPlugin
{
    class RocketUpdate : UpdateListener
    {
        private RocketInterface rocketInterface;

        public RocketUpdate(RocketInterface rocketInterface)
        {
            this.rocketInterface = rocketInterface;
        }

        public void sendUpdate(Clock clock)
        {
            rocketInterface.render();
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }
    }
}

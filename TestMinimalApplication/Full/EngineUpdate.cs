using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace Anomalous.Minimus.Full
{
    class EngineUpdate : UpdateListener
    {
        private EngineController controller;

        public EngineUpdate(EngineController controller)
        {
            this.controller = controller;
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        public void sendUpdate(Clock clock)
        {
            controller._sendUpdate(clock);
        }
    }
}

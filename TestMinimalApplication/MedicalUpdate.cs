using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace Anomalous.Minimus
{
    class MedicalUpdate : UpdateListener
    {
        private EngineController controller;

        public MedicalUpdate(EngineController controller)
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

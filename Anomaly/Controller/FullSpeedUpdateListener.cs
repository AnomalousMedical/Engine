using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace Anomaly
{
    class FullSpeedUpdateListener : UpdateListener
    {
        private SceneController sceneController;

        public FullSpeedUpdateListener(SceneController sceneController)
        {
            this.sceneController = sceneController;
        }

        public void sendUpdate(Clock clock)
        {
            sceneController.drawDebugInformation();
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }
    }
}

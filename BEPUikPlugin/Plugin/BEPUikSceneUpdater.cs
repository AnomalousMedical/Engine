using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    class BEPUikSceneUpdater : UpdateListener
    {
        BEPUikScene scene;

        public BEPUikSceneUpdater(BEPUikScene scene)
        {
            this.scene = scene;
        }

        public void sendUpdate(Clock clock)
        {
            scene.update();
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }
    }
}

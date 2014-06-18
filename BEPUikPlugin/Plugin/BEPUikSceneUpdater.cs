using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    class BEPUikSceneUpdater : BackgroundUpdateListener
    {
        BEPUikScene scene;

        public BEPUikSceneUpdater(BEPUikScene scene)
        {
            this.scene = scene;
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }

        public void doBackgroundWork(Clock clock)
        {
            scene.backgroundUpdate();
        }

        public void synchronizeResults()
        {
            scene.sync();
        }
    }
}

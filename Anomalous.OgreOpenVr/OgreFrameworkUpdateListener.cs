using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anomalous.OgreOpenVr
{
    class OgreFrameworkUpdateListener : UpdateListener
    {
        private OgreFramework ogreFramework;

        public OgreFrameworkUpdateListener(OgreFramework ogreFramework)
        {
            this.ogreFramework = ogreFramework;
        }

        public void exceededMaxDelta()
        {
            
        }

        public void loopStarting()
        {
            
        }

        public void sendUpdate(Clock clock)
        {
            ogreFramework.Update(clock.DeltaSeconds);
        }
    }
}

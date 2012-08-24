using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using OgreWrapper;
using Engine;

namespace OgrePlugin
{
    /// <summary>
    /// A Timer Update class for Ogre.
    /// </summary>
    class OgreUpdate : UpdateListener
    {
        private Root ogreRoot;

        public OgreUpdate(Root ogreRoot)
        {
            this.ogreRoot = ogreRoot;
        }

        public void sendUpdate(Clock clock)
        {
            PerformanceMonitor.start("OgreRender");
            float time = (float)clock.Seconds;
            ogreRoot._fireFrameStarted(time, time);
            ogreRoot._updateAllRenderTargets();
            ogreRoot._fireFrameEnded(time, time);
            PerformanceMonitor.stop("OgreRender");
        }

        public void loopStarting()
        {
            ogreRoot.clearEventTimes();
            ogreRoot.getRenderSystem()._initRenderTargets();
        }

        public void exceededMaxDelta()
        {
            ogreRoot.clearEventTimes();
        }
    }
}

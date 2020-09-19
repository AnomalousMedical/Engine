using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;

namespace OgreNextPlugin
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
            ogreRoot.renderOneFrame(clock.DeltaSeconds);
            PerformanceMonitor.stop("OgreRender");
        }

        public void loopStarting()
        {
            ogreRoot.clearEventTimes();
        }

        public void exceededMaxDelta()
        {
            ogreRoot.clearEventTimes();
        }
    }
}

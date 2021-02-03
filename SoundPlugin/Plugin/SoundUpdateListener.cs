using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;

namespace SoundPlugin
{
    class SoundUpdateListener : UpdateListener
    {
        private OpenALManager openALManager;

        public SoundUpdateListener(OpenALManager openALManager)
        {
            this.openALManager = openALManager;
        }

        public void sendUpdate(Clock clock)
        {
            PerformanceMonitor.start("Sound");
            openALManager.Update();
            PerformanceMonitor.stop("Sound");
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }
    }
}

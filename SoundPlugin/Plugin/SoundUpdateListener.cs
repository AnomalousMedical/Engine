using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

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
            openALManager.update();
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }
    }
}

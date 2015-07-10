using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Threads
{
    class ThreadManagerUpdate : UpdateListener
    {
        public void sendUpdate(Clock clock)
        {
            ThreadManager._doInvoke();
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }
    }
}

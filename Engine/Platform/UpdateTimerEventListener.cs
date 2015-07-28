using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    public class UpdateTimerEventListener : UpdateListener
    {
        public event Action<Clock> OnUpdate;

        public void sendUpdate(Clock clock)
        {
            if(OnUpdate != null)
            {
                OnUpdate.Invoke(clock);
            }
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }
    }
}

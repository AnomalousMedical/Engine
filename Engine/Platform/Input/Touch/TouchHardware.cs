using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    public abstract class TouchHardware
    {
        private Touches touches;

        public TouchHardware(Touches touches)
        {
            this.touches = touches;
        }

        protected void fireTouchStarted(TouchInfo touchInfo)
        {
            touches.fireTouchStarted(touchInfo);
        }

        protected void fireTouchEnded(TouchInfo touchInfo)
        {
            touches.fireTouchEnded(touchInfo);
        }

        protected void fireTouchMoved(TouchInfo touchInfo)
        {
            touches.fireTouchMoved(touchInfo);
        }

        protected void fireAllTouchesCanceled()
        {
            touches.fireAllTouchesCanceled();
        }
    }
}

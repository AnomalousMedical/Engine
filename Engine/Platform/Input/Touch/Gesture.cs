using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    public abstract class Gesture : MessageEvent
    {
        public Gesture(Object eventLayerKey)
            :base(eventLayerKey)
        {

        }

        protected internal override void update(EventLayer eventLayer, bool allowProcessing, Clock clock)
        {
            var touches = eventLayer.EventManager.Touches;
            processFingers(eventLayer, touches);
            additionalProcessing(eventLayer, clock);
        }

        protected abstract bool processFingers(EventLayer eventLayer, Touches touches);

        protected abstract void additionalProcessing(EventLayer eventLayer, Clock clock);
    }
}

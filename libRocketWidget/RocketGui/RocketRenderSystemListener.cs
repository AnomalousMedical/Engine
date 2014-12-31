using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgrePlugin;

namespace Anomalous.libRocketWidget
{
    class RocketRenderSystemListener : RenderSystemListener
    {
        public RocketRenderSystemListener()
        {
            
        }

        public override void eventOccured()
        {
            switch (EventType)
            {
                case KnownRenderSystemEvents.DeviceLost:
                    
                    break;
                case KnownRenderSystemEvents.DeviceRestored:
                    RocketWidgetManager.deviceRestored();
                    break;
            }
        }
    }
}

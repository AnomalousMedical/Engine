using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    /// <summary>
    /// This class listens for device events and fires them to classes that are interested throughout the plugin.
    /// </summary>
    class DeviceLostListener : RenderSystemListener
    {
        public override void eventOccured()
        {
            switch(this.EventType)
            {
                case KnownRenderSystemEvents.DeviceRestored:
                    ManualObject.fireRedrawRequired();
                    break;
            }
        }
    }
}

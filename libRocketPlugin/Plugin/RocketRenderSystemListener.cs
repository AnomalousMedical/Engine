using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libRocketPlugin
{
    class RocketRenderSystemListener : RenderSystemListener
    {
        public override void eventOccured()
        {
            switch (this.EventType)
            {
                case OgrePlugin.KnownRenderSystemEvents.DeviceRestored:
                    GeometryDatabase.ReleaseGeometries();
                    break;
            }
        }
    }
}

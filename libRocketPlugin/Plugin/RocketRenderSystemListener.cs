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
                    Core.ReleaseCompiledGeometries();
                    Core.ReleaseTextures();
                    foreach (var context in Core.Contexts)
                    {
                        foreach (var document in context.Documents)
                        {
                            document.MakeDirtyForScaleChange();
                        }
                    }
                    break;
            }
        }
    }
}

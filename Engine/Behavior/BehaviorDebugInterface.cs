using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Renderer;
using Engine.ObjectManagement;

namespace Engine
{
    class BehaviorDebugInterface : DebugInterface
    {
        private List<DebugEntry> entries = new List<DebugEntry>();
        private bool enabled = false;

        public IEnumerable<DebugEntry> getEntries()
        {
            return entries;
        }

        public void renderDebug(DebugDrawingSurface drawingSurface, SimSubScene subScene)
        {
            if (enabled)
            {
                BehaviorManager manager = subScene.getSimElementManager<BehaviorManager>();
                if (manager != null)
                {
                    manager.renderDebugInfo(drawingSurface);
                }
            }
        }

        public void setEnabled(bool enabled)
        {
            this.enabled = enabled;
        }

        public string Name
        {
            get
            {
                return "Behavior Debug";
            }
        }
    }
}

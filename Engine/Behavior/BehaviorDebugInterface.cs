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
        private bool clearSurface = false;
        private DebugDrawingSurface drawingSurface;
        private bool depthTesting = false;

        public IEnumerable<DebugEntry> getEntries()
        {
            return entries;
        }

        public void renderDebug(SimSubScene subScene)
        {
            if (enabled)
            {
                BehaviorManager manager = subScene.getSimElementManager<BehaviorManager>();
                if (manager != null)
                {
                    manager.renderDebugInfo(drawingSurface);
                }
            }
            else if (clearSurface)
            {
                clearSurface = false;
                drawingSurface.clearAll();
            }
        }

        public void createDebugInterface(RendererPlugin rendererPlugin, SimSubScene subScene)
        {
            drawingSurface = rendererPlugin.createDebugDrawingSurface("BehaviorSurface", subScene);
            drawingSurface.setDepthTesting(depthTesting);
        }

        public void destroyDebugInterface(RendererPlugin rendererPlugin, SimSubScene subScene)
        {
            if (drawingSurface != null)
            {
                rendererPlugin.destroyDebugDrawingSurface(drawingSurface);
                drawingSurface = null;
            }
        }

        public void setDepthTesting(bool depthCheckEnabled)
        {
            depthTesting = depthCheckEnabled;
            if (drawingSurface != null)
            {
                drawingSurface.setDepthTesting(depthTesting);
            }
        }

        public bool isDepthTestingEnabled()
        {
            return depthTesting;
        }

        public void setEnabled(bool enabled)
        {
            this.enabled = enabled;
            if (!enabled)
            {
                clearSurface = true;
            }
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

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
            if(drawingSurface != null)
            {
                drawingSurface.setDepthTesting(depthTesting);
            }
        }

        public void destroyDebugInterface(RendererPlugin rendererPlugin, SimSubScene subScene)
        {
            if (drawingSurface != null)
            {
                rendererPlugin.destroyDebugDrawingSurface(drawingSurface);
                drawingSurface = null;
            }
        }

        public bool DepthTesting
        {
            get
            {
                return depthTesting;
            }
            set
            {
                depthTesting = value;
                if (drawingSurface != null)
                {
                    drawingSurface.setDepthTesting(depthTesting);
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                if(enabled != value)
                {
                    this.enabled = value;
                    if (!enabled)
                    {
                        clearSurface = true;
                    }
                }
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

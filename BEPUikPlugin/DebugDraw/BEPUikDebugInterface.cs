using Engine;
using Engine.ObjectManagement;
using Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    class BEPUikDebugInterface : DebugInterface
    {
        private LinkedList<DebugEntry> entries = new LinkedList<DebugEntry>();
        private bool enabled = false;
        private bool depthTesting = false;
        private DebugDrawingSurface drawingSurface = null;
        bool clearDebugSurface = false;

        public BEPUikDebugInterface()
        {

        }

        public IEnumerable<DebugEntry> Entries
        {
            get
            {
                return entries;
            }
        }

        public void renderDebug(SimSubScene subScene)
        {
            if (enabled)
            {
                BEPUikScene sceneManager = subScene.getSimElementManager<BEPUikScene>();
                if (sceneManager != null)
                {
                    sceneManager.drawDebug(drawingSurface);
                }
            }
            else if (clearDebugSurface)
            {
                drawingSurface.clearAll();
                clearDebugSurface = false;
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
                    enabled = value;
                    if(!enabled)
                    {
                        clearDebugSurface = true;
                    }
                }
            }
        }

        public void createDebugInterface(RendererPlugin rendererPlugin, SimSubScene subScene)
        {
            drawingSurface = rendererPlugin.createDebugDrawingSurface("BEPUikDebugSurface", subScene);
            if (drawingSurface != null)
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
                if (depthTesting != value)
                {
                    depthTesting = value;
                    if (drawingSurface != null)
                    {
                        drawingSurface.setDepthTesting(depthTesting);
                    }
                }
            }
        }

        public string Name
        {
            get
            {
                return "BEPUik Debug";
            }
        }
    }
}

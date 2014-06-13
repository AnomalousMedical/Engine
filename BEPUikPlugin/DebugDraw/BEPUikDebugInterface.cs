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
    public enum DebugDrawMode : int
    {
        NoDebug = 0,
        Bones = 1,
        AngularJoints = 2,
        BallSocketJoints = 4,
        DistanceJoints = 8,
        DistanceLimits = 16,
        RevoluteJoints = 32,
        SwingLimits = 64,
        SwivelHingeJoints = 128,
        TwistJoints = 256,
        TwistLimits = 512,
        MAX_DEBUG_DRAW_MODE
    }

    class BEPUikDebugInterface : DebugInterface
    {
        private LinkedList<DebugEntry> entries = new LinkedList<DebugEntry>();
        private bool enabled = false;
        private bool depthTesting = false;
        private DebugDrawingSurface drawingSurface = null;
        bool clearDebugSurface = false;
        private DebugDrawMode drawMode = DebugDrawMode.NoDebug;

        public BEPUikDebugInterface()
        {
            entries.AddLast(new BEPUikDebugEntry("Draw Bones", DebugDrawMode.Bones, this));
            entries.AddLast(new BEPUikDebugEntry("Draw Ball Socket Joints", DebugDrawMode.BallSocketJoints, this));
            entries.AddLast(new BEPUikDebugEntry("Draw Swivel Hinge Joints", DebugDrawMode.SwivelHingeJoints, this));
            entries.AddLast(new BEPUikDebugEntry("Draw Twist Joints", DebugDrawMode.TwistJoints, this));
            entries.AddLast(new BEPUikDebugEntry("Draw Swing Limits", DebugDrawMode.SwingLimits, this));
            entries.AddLast(new BEPUikDebugEntry("Draw Twist Limits", DebugDrawMode.TwistLimits, this));
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
                    sceneManager.drawDebug(drawingSurface, drawMode);
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

        internal void enableGlobalDebugMode(DebugDrawMode mode)
        {
            drawMode |= mode;
        }

        internal void disableGlobalDebugMode(DebugDrawMode mode)
        {
            drawMode &= (~mode);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Renderer;
using Engine.ObjectManagement;

namespace BulletPlugin
{
    public class BulletDebugInterface : DebugInterface
    {
        List<DebugEntry> debugEntries = new List<DebugEntry>();
	    bool enabled = false;
	    bool firstFrameDisabled = false;
	    DebugDrawingSurface drawingSurface = null;
	    bool depthTesting = false;

        public BulletDebugInterface()
        {
            debugEntries.Add(new BulletDebugEntry("Draw Wireframe", DebugDrawModes.DBG_DrawWireframe));
	        debugEntries.Add(new BulletDebugEntry("Draw AABB", DebugDrawModes.DBG_DrawAabb));
	        debugEntries.Add(new BulletDebugEntry("Draw Features Text", DebugDrawModes.DBG_DrawFeaturesText));
	        debugEntries.Add(new BulletDebugEntry("Draw Contact Points", DebugDrawModes.DBG_DrawContactPoints));
	        debugEntries.Add(new BulletDebugEntry("No Deactivation", DebugDrawModes.DBG_NoDeactivation));
	        debugEntries.Add(new BulletDebugEntry("No Help Text", DebugDrawModes.DBG_NoHelpText));
	        debugEntries.Add(new BulletDebugEntry("Draw Text", DebugDrawModes.DBG_DrawText));
	        debugEntries.Add(new BulletDebugEntry("Profile Timings", DebugDrawModes.DBG_ProfileTimings));
	        debugEntries.Add(new BulletDebugEntry("Enable Sat Comparison", DebugDrawModes.DBG_EnableSatComparison));
	        debugEntries.Add(new BulletDebugEntry("Disable Bullet LCP", DebugDrawModes.DBG_DisableBulletLCP));
	        debugEntries.Add(new BulletDebugEntry("Enable CCD", DebugDrawModes.DBG_EnableCCD));
	        debugEntries.Add(new BulletDebugEntry("Draw Constraints", DebugDrawModes.DBG_DrawConstraints));
	        debugEntries.Add(new BulletDebugEntry("Draw Constraint Limits", DebugDrawModes.DBG_DrawConstraintLimits));
        }

        public IEnumerable<DebugEntry> getEntries()
        {
	        return debugEntries;
        }

        public void renderDebug(SimSubScene subScene)
        {
	        if(enabled)
	        {
		        BulletScene sceneManager = subScene.getSimElementManager<BulletScene>();
		        if(sceneManager != null)
		        {
			        sceneManager.drawDebug(drawingSurface);
		        }
	        }
	        else if(firstFrameDisabled)
	        {
		        BulletScene sceneManager = subScene.getSimElementManager<BulletScene>();
		        if(sceneManager != null)
		        {
			        sceneManager.clearDebug(drawingSurface);
		        }
		        firstFrameDisabled = false;
	        }
        }

        public void createDebugInterface(RendererPlugin rendererPlugin, SimSubScene subScene)
        {
            drawingSurface = rendererPlugin.createDebugDrawingSurface("BulletDebugSurface", subScene);
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

        public void setDepthTesting(bool depthCheckEnabled)
        {
            depthTesting = depthCheckEnabled;
	        if(drawingSurface != null)
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
	        if(this.enabled && !enabled)
	        {
		        firstFrameDisabled = true;
	        }
	        this.enabled = enabled;
        }

        public String Name
        {
            get
            {
                return "Bullet Debug";
            }
        }
    }
}

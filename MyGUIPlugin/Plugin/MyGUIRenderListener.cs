using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgrePlugin;

namespace MyGUIPlugin
{
    class MyGUIRenderListener : SceneListener, RenderQueueListener
    {
        public event EventHandler RenderStarted;
        public event EventHandler RenderEnded;

        private Viewport viewport;
        private SceneManager sceneManager;
        private OgreRenderManager renderManager;

        private int lastWidth;
        private int lastHeight;

        public MyGUIRenderListener(Viewport viewport, SceneManager sceneManager, OgreRenderManager renderManager)
        {
            this.viewport = viewport;
            this.sceneManager = sceneManager;
            this.renderManager = renderManager;
            sceneManager.addRenderQueueListener(this);
            sceneManager.addSceneListener(this);
        }

        public void preFindVisibleObjects(SceneManager sceneManager, SceneManager.IlluminationRenderStage irs, Viewport viewport)
        {
            if (this.viewport == viewport)
            {
                if (RenderStarted != null)
                {
                    RenderStarted.Invoke(this, EventArgs.Empty);
                }

                resized();

                renderManager.update();
            }
        }

        public void postFindVisibleObjects(SceneManager sceneManager, SceneManager.IlluminationRenderStage irs, Viewport viewport)
        {
            
        }

        public void preRenderQueues()
        {
            
        }

        public void postRenderQueues()
        {
            if (sceneManager.getCurrentViewport() == viewport)
            {
                if (RenderEnded != null)
                {
                    RenderEnded.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void renderQueueStarted(byte queueGroupId, string invocation, ref bool skipThisInvocation)
        {
            
        }

        public void renderQueueEnded(byte queueGroupId, string invocation, ref bool repeatThisInvocation)
        {
            
        }

        internal void resized()
        {
            int width = viewport.getActualWidth();
            int height = viewport.getActualHeight();
            if (width != lastWidth || height != lastHeight)
            {
                renderManager.windowResized(width, height);
                lastWidth = width;
                lastHeight = height;
            }
        }
    }
}

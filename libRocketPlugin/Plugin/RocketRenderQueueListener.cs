using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;

namespace libRocketPlugin
{
    public class RocketRenderQueueListener : RenderQueueListener
    {
        private Context context;
        private RenderInterfaceOgre3D renderInterface;

        public RocketRenderQueueListener(Context context, RenderInterfaceOgre3D renderInterface)
        {
            this.context = context;
            this.renderInterface = renderInterface;
        }

        public void preRenderQueues()
        {

        }

        public void postRenderQueues()
        {

        }

        public void renderQueueStarted(byte queueGroupId, string invocation, ref bool skipThisInvocation)
        {
            if (queueGroupId == 100)
            {
                context.Update();

                Vector2i dimensions = context.Dimensions;
                renderInterface.ConfigureRenderSystem(dimensions.X, dimensions.Y);
                context.Render();
            }
        }

        public void renderQueueEnded(byte queueGroupId, string invocation, ref bool repeatThisInvocation)
        {

        }
    }
}

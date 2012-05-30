using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;
using Engine;

namespace libRocketPlugin
{
    public class RocketRenderQueueListener : RenderQueueListener
    {
        private Context context;
        private RenderInterfaceOgre3D renderInterface;
        private IntSize2 renderDimensions;

        public event Action FrameCompleted;

        public RocketRenderQueueListener(Context context, RenderInterfaceOgre3D renderInterface)
        {
            this.context = context;
            this.renderInterface = renderInterface;
            Vector2i dimensions = context.Dimensions;
            renderDimensions = new IntSize2(dimensions.X, dimensions.Y);
        }

        public void preRenderQueues()
        {

        }

        public void postRenderQueues()
        {
            if (FrameCompleted != null)
            {
                FrameCompleted.Invoke();
            }
        }

        public void renderQueueStarted(byte queueGroupId, string invocation, ref bool skipThisInvocation)
        {
            if (queueGroupId == 100)
            {
                context.Update();
                renderInterface.ConfigureRenderSystem(renderDimensions.Width, renderDimensions.Height);
                context.Render();
            }
        }

        public void renderQueueEnded(byte queueGroupId, string invocation, ref bool repeatThisInvocation)
        {

        }

        public IntSize2 RenderDimensions
        {
            get
            {
                return renderDimensions;
            }
            set
            {
                renderDimensions = value;
            }
        }
    }
}

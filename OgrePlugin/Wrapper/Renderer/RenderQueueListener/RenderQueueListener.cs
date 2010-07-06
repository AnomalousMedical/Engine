using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public interface RenderQueueListener
    {
        void preRenderQueues();

        void postRenderQueues();

        void renderQueueStarted(byte queueGroupId, String invocation, ref bool skipThisInvocation);

        void renderQueueEnded(byte queueGroupId, String invocation, ref bool repeatThisInvocation);
    }
}

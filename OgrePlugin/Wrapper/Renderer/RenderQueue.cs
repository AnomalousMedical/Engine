using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    public class RenderQueue : IDisposable
    {
        private IntPtr renderQueue;

        internal RenderQueue(IntPtr renderQueue)
        {
            this.renderQueue = renderQueue;
        }

        public void Dispose()
        {
            renderQueue = IntPtr.Zero;
        }

	    byte getDefaultQueueGroup()
        {
            return RenderQueue_getDefaultQueueGroup(renderQueue);
        }

        void setDefaultRenderablePriority(ushort priority)
        {
            RenderQueue_setDefaultRenderablePriority(renderQueue, priority);
        }

        ushort getDefaultRenderablePriority()
        {
            return RenderQueue_getDefaultRenderablePriority(renderQueue);
        }

        void setDefaultQueueGroup(byte grp)
        {
            RenderQueue_setDefaultQueueGroup(renderQueue, grp);
        }

        void setSplitPassesByLightingType(bool split)
        {
            RenderQueue_setSplitPassesByLightingType(renderQueue, split);
        }

        void setSplitNoShadowPasses(bool split)
        {
            RenderQueue_setSplitNoShadowPasses(renderQueue, split);
        }

        void setShadowCastersCannotBeReceivers(bool ind)
        {
            RenderQueue_setShadowCastersCannotBeReceivers(renderQueue, ind);
        }

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern byte RenderQueue_getDefaultQueueGroup(IntPtr renderQueue);

        [DllImport("OgreCWrapper")]
        private static extern void RenderQueue_setDefaultRenderablePriority(IntPtr renderQueue, ushort priority);

        [DllImport("OgreCWrapper")]
        private static extern ushort RenderQueue_getDefaultRenderablePriority(IntPtr renderQueue);

        [DllImport("OgreCWrapper")]
        private static extern void RenderQueue_setDefaultQueueGroup(IntPtr renderQueue, byte grp);

        [DllImport("OgreCWrapper")]
        private static extern void RenderQueue_setSplitPassesByLightingType(IntPtr renderQueue, bool split);

        [DllImport("OgreCWrapper")]
        private static extern void RenderQueue_setSplitNoShadowPasses(IntPtr renderQueue, bool split);

        [DllImport("OgreCWrapper")]
        private static extern void RenderQueue_setShadowCastersCannotBeReceivers(IntPtr renderQueue, bool ind);
#endregion
    }
}

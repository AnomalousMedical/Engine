using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RenderTargetEventDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RenderTargetViewportEventDelegate();

    class RenderTargetListener : IDisposable
    {        
        private IntPtr ptr;
        private RenderTargetEventDelegate preRenderTargetUpdateCb;
        private RenderTargetEventDelegate postRenderTargetUpdateCb;
        private RenderTargetViewportEventDelegate preViewportUpdateCb;
        private RenderTargetViewportEventDelegate postViewportUpdateCb;
        private RenderTargetViewportEventDelegate viewportAddedCb;
        private RenderTargetViewportEventDelegate viewportRemovedCb;

        public event RenderTargetEventDelegate PreRenderTargetUpdate;
        public event RenderTargetEventDelegate PostRenderTargetUpdate;
        public event RenderTargetViewportEventDelegate PreViewportUpdate;
        public event RenderTargetViewportEventDelegate PostViewportUpdate;
        public event RenderTargetViewportEventDelegate ViewportAdded;
        public event RenderTargetViewportEventDelegate ViewportRemoved;

        public RenderTargetListener()
        {
            preRenderTargetUpdateCb = preRenderTargetUpdate;
            postRenderTargetUpdateCb = postRenderTargetUpdate;
            preViewportUpdateCb = preViewportUpdate;
            postViewportUpdateCb = postViewportUpdate;
            viewportAddedCb = viewportAdded;
            viewportRemovedCb = viewportRemoved;
            ptr = ManagedRenderTargetListener_Create(preRenderTargetUpdateCb, postRenderTargetUpdateCb, preViewportUpdateCb, postViewportUpdateCb, viewportAddedCb, viewportRemovedCb);
        }

        public void Dispose()
        {
            ManagedRenderTargetListener_Delete(ptr);
        }

        public void preRenderTargetUpdate()
	    {
            if (PreRenderTargetUpdate != null)
            {
                PreRenderTargetUpdate.Invoke();
            }
	    }

        public void postRenderTargetUpdate()
	    {
            if (PostRenderTargetUpdate != null)
            {
                PostRenderTargetUpdate.Invoke();
            }
	    }

        public void preViewportUpdate()
	    {
            if (PreViewportUpdate != null)
            {
                PreViewportUpdate.Invoke();
            }
	    }

        public void postViewportUpdate()
	    {
            if (PostViewportUpdate != null)
            {
                PostViewportUpdate.Invoke();
            }
	    }

        public void viewportAdded()
	    {
            if (ViewportAdded != null)
            {
                ViewportAdded.Invoke();
            }
	    }

        public void viewportRemoved()
	    {
            if (ViewportRemoved != null)
            {
                ViewportRemoved.Invoke();
            }
	    }

        public bool HasSubscribers
        {
            get
            {
                return PreRenderTargetUpdate != null ||
                       PostRenderTargetUpdate != null ||
                       PreViewportUpdate != null ||
                       PostViewportUpdate != null ||
                       ViewportAdded != null ||
                       ViewportRemoved != null;
            }
        }

        internal IntPtr Ptr
        {
            get
            {
                return ptr;
            }
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ManagedRenderTargetListener_Create(RenderTargetEventDelegate preRenderTargetUpdateCb, RenderTargetEventDelegate postRenderTargetUpdateCb, RenderTargetViewportEventDelegate preViewportUpdateCb, RenderTargetViewportEventDelegate postViewportUpdateCb, RenderTargetViewportEventDelegate viewportAddedCb, RenderTargetViewportEventDelegate viewportRemovedCb);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedRenderTargetListener_Delete(IntPtr listener);

        #endregion
    }
}

using Anomalous.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    class RenderTargetListener : IDisposable
    {        
        private IntPtr ptr;
        private CallbackHandler callbackHandler;

        public event Action PreRenderTargetUpdate;
        public event Action PostRenderTargetUpdate;
        public event Action PreViewportUpdate;
        public event Action PostViewportUpdate;
        public event Action ViewportAdded;
        public event Action ViewportRemoved;

        public RenderTargetListener()
        {
            callbackHandler = new CallbackHandler();
            ptr = callbackHandler.create(this);
        }

        public void Dispose()
        {
            ManagedRenderTargetListener_Delete(ptr);
            callbackHandler.Dispose();
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
        private static extern IntPtr ManagedRenderTargetListener_Create(NativeAction preRenderTargetUpdateCb, NativeAction postRenderTargetUpdateCb, NativeAction preViewportUpdateCb, NativeAction postViewportUpdateCb, NativeAction viewportAddedCb, NativeAction viewportRemovedCb
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedRenderTargetListener_Delete(IntPtr listener);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static NativeAction preRenderTargetUpdateCb;
            private static NativeAction postRenderTargetUpdateCb;
            private static NativeAction preViewportUpdateCb;
            private static NativeAction postViewportUpdateCb;
            private static NativeAction viewportAddedCb;
            private static NativeAction viewportRemovedCb;

            static CallbackHandler()
            {
                preRenderTargetUpdateCb = preRenderTargetUpdate;
                postRenderTargetUpdateCb = postRenderTargetUpdate;
                preViewportUpdateCb = preViewportUpdate;
                postViewportUpdateCb = postViewportUpdate;
                viewportAddedCb = viewportAdded;
                viewportRemovedCb = viewportRemoved;
            }

            [MonoTouch.MonoPInvokeCallback(typeof(NativeAction))]
            private static void preRenderTargetUpdate(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as RenderTargetListener).preRenderTargetUpdate();
            }

            [MonoTouch.MonoPInvokeCallback(typeof(NativeAction))]
            private static void postRenderTargetUpdate(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as RenderTargetListener).postRenderTargetUpdate();
            }

            [MonoTouch.MonoPInvokeCallback(typeof(NativeAction))]
            private static void preViewportUpdate(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as RenderTargetListener).preViewportUpdate();
            }

            [MonoTouch.MonoPInvokeCallback(typeof(NativeAction))]
            private static void postViewportUpdate(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as RenderTargetListener).postViewportUpdate();
            }

            [MonoTouch.MonoPInvokeCallback(typeof(NativeAction))]
            private static void viewportAdded(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as RenderTargetListener).viewportAdded();
            }

            [MonoTouch.MonoPInvokeCallback(typeof(NativeAction))]
            private static void viewportRemoved(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as RenderTargetListener).viewportRemoved();
            }

            private GCHandle handle;

            public IntPtr create(RenderTargetListener obj)
            {
                handle = GCHandle.Alloc(obj);
                return ManagedRenderTargetListener_Create(preRenderTargetUpdateCb, postRenderTargetUpdateCb, preViewportUpdateCb, postViewportUpdateCb, viewportAddedCb, viewportRemovedCb, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            private NativeAction preRenderTargetUpdateCb;
            private NativeAction postRenderTargetUpdateCb;
            private NativeAction preViewportUpdateCb;
            private NativeAction postViewportUpdateCb;
            private NativeAction viewportAddedCb;
            private NativeAction viewportRemovedCb;

            public IntPtr create(RenderTargetListener obj)
            {
                preRenderTargetUpdateCb = obj.preRenderTargetUpdate;
                postRenderTargetUpdateCb = obj.postRenderTargetUpdate;
                preViewportUpdateCb = obj.preViewportUpdate;
                postViewportUpdateCb = obj.postViewportUpdate;
                viewportAddedCb = obj.viewportAdded;
                viewportRemovedCb = obj.viewportRemoved;
                return ManagedRenderTargetListener_Create(preRenderTargetUpdateCb, postRenderTargetUpdateCb, preViewportUpdateCb, postViewportUpdateCb, viewportAddedCb, viewportRemovedCb);
            }

            public void Dispose()
            {

            }
        }
#endif

        #endregion
    }
}

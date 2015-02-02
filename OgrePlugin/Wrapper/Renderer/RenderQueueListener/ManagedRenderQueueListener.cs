using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Anomalous.Interop;

namespace OgrePlugin
{
    class ManagedRenderQueueListener
    {
        List<RenderQueueListener> listeners = new List<RenderQueueListener>();

        IntPtr nativeRenderQueueListener;
        CallbackHandler callbackHandler;
        SceneManager sceneManager;

        public ManagedRenderQueueListener(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
            callbackHandler = new CallbackHandler();
            nativeRenderQueueListener = callbackHandler.create(this);
        }

        public void Dispose()
        {
            callbackHandler.Dispose();
            if (listeners.Count != 0)
            {
                NativeRenderQueue_RemoveListener(sceneManager.OgreSceneManager, nativeRenderQueueListener);
            }
            NativeRenderQueue_Delete(nativeRenderQueueListener);
        }

        public void addListener(RenderQueueListener listener)
        {
            if (listeners.Count == 0)
            {
                NativeRenderQueue_AddListener(sceneManager.OgreSceneManager, nativeRenderQueueListener);
            }
            listeners.Add(listener);
        }

        public void removeListener(RenderQueueListener listener)
        {
            listeners.Remove(listener);
            if (listeners.Count == 0)
            {
                NativeRenderQueue_RemoveListener(sceneManager.OgreSceneManager, nativeRenderQueueListener);
            }
        }

        public int getNumListeners()
        {
            return listeners.Count;
        }

        public IntPtr NativeSceneListener
        {
            get
            {
                return nativeRenderQueueListener;
            }
        }

        void preRenderQueues()
        {
            foreach (RenderQueueListener listener in listeners)
            {
                listener.preRenderQueues();
            }
        }

        void postRenderQueues()
        {
            foreach (RenderQueueListener listener in listeners)
            {
                listener.postRenderQueues();
            }
        }

        bool renderQueueStarted(byte queueGroupId, IntPtr invocation)
        {
            bool skip = false;
            String invoke = Marshal.PtrToStringAnsi(invocation);
            foreach (RenderQueueListener listener in listeners)
            {
                listener.renderQueueStarted(queueGroupId, invoke, ref skip);
            }
            return skip;
        }

        bool renderQueueEnded(byte queueGroupId, IntPtr invocation)
        {
            bool repeat = false;
            String invoke = Marshal.PtrToStringAnsi(invocation);
            foreach (RenderQueueListener listener in listeners)
            {
                listener.renderQueueEnded(queueGroupId, invoke, ref repeat);
            }
            return repeat;
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr NativeRenderQueue_Create(NativeAction preRender, NativeAction postRender, ByteStringBoolRenderQueueEvent renderStarted, ByteStringBoolRenderQueueEvent renderEnded
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void NativeRenderQueue_Delete(IntPtr nativeRenderQueueListener);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void NativeRenderQueue_AddListener(IntPtr sceneManager, IntPtr nativeRenderQueueListener);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void NativeRenderQueue_RemoveListener(IntPtr sceneManager, IntPtr nativeRenderQueueListener);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool ByteStringBoolRenderQueueEvent(byte queueGroupId, IntPtr invocation
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            static NativeAction preRenderQueuesCallback;
            static NativeAction postRenderQueuesCallback;
            static ByteStringBoolRenderQueueEvent renderQueueStartedCallback;
            static ByteStringBoolRenderQueueEvent renderQueueEndedCallback;

            static CallbackHandler()
            {
                preRenderQueuesCallback = new NativeAction(preRenderQueues);
                postRenderQueuesCallback = new NativeAction(postRenderQueues);
                renderQueueStartedCallback = new ByteStringBoolRenderQueueEvent(renderQueueStarted);
                renderQueueEndedCallback = new ByteStringBoolRenderQueueEvent(renderQueueEnded);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(NativeAction))]
            static void preRenderQueues(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ManagedRenderQueueListener).preRenderQueues();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(NativeAction))]
            static void postRenderQueues(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ManagedRenderQueueListener).postRenderQueues();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(ByteStringBoolRenderQueueEvent))]
            static bool renderQueueStarted(byte queueGroupId, IntPtr invocation, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as ManagedRenderQueueListener).renderQueueStarted(queueGroupId, invocation);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(ByteStringBoolRenderQueueEvent))]
            static bool renderQueueEnded(byte queueGroupId, IntPtr invocation, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as ManagedRenderQueueListener).renderQueueEnded(queueGroupId, invocation);
            }

            private GCHandle handle;

            public IntPtr create(ManagedRenderQueueListener obj)
            {
                handle = GCHandle.Alloc(obj);
                return NativeRenderQueue_Create(preRenderQueuesCallback, postRenderQueuesCallback, renderQueueStartedCallback, renderQueueEndedCallback, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            NativeAction preRenderQueuesCallback;
            NativeAction postRenderQueuesCallback;
            ByteStringBoolRenderQueueEvent renderQueueStartedCallback;
            ByteStringBoolRenderQueueEvent renderQueueEndedCallback;

            public IntPtr create(ManagedRenderQueueListener obj)
            {
                preRenderQueuesCallback = new NativeAction(obj.preRenderQueues);
                postRenderQueuesCallback = new NativeAction(obj.postRenderQueues);
                renderQueueStartedCallback = new ByteStringBoolRenderQueueEvent(obj.renderQueueStarted);
                renderQueueEndedCallback = new ByteStringBoolRenderQueueEvent(obj.renderQueueEnded);
                return NativeRenderQueue_Create(preRenderQueuesCallback, postRenderQueuesCallback, renderQueueStartedCallback, renderQueueEndedCallback);
            }

            public void Dispose()
            {

            }
        }
#endif

        #endregion
    }
}

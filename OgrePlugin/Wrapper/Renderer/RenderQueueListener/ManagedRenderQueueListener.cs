using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    class ManagedRenderQueueListener
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void EmptyRenderQueueEvent();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool ByteStringBoolRenderQueueEvent(byte queueGroupId, IntPtr invocation);

        List<RenderQueueListener> listeners = new List<RenderQueueListener>();

        IntPtr nativeRenderQueueListener;
        SceneManager sceneManager;
        EmptyRenderQueueEvent preRenderQueuesCallback;
        EmptyRenderQueueEvent postRenderQueuesCallback;
        ByteStringBoolRenderQueueEvent renderQueueStartedCallback;
        ByteStringBoolRenderQueueEvent renderQueueEndedCallback;

        public ManagedRenderQueueListener(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
            preRenderQueuesCallback = new EmptyRenderQueueEvent(preRenderQueues);
            postRenderQueuesCallback = new EmptyRenderQueueEvent(postRenderQueues);
            renderQueueStartedCallback = new ByteStringBoolRenderQueueEvent(renderQueueStarted);
            renderQueueEndedCallback = new ByteStringBoolRenderQueueEvent(renderQueueEnded);
            nativeRenderQueueListener = NativeRenderQueue_Create(preRenderQueuesCallback, postRenderQueuesCallback, renderQueueStartedCallback, renderQueueEndedCallback);
        }

        public void Dispose()
        {
            preRenderQueuesCallback = null;
            postRenderQueuesCallback = null;
            renderQueueStartedCallback = null;
            renderQueueEndedCallback = null;
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
        
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr NativeRenderQueue_Create(EmptyRenderQueueEvent preRender, EmptyRenderQueueEvent postRender, ByteStringBoolRenderQueueEvent renderStarted, ByteStringBoolRenderQueueEvent renderEnded);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeRenderQueue_Delete(IntPtr nativeRenderQueueListener);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeRenderQueue_AddListener(IntPtr sceneManager, IntPtr nativeRenderQueueListener);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeRenderQueue_RemoveListener(IntPtr sceneManager, IntPtr nativeRenderQueueListener);

#endregion
    }
}

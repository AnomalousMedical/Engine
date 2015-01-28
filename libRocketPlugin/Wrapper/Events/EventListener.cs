using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    /// <summary>
    /// Default EventListener superclass, this version will automatically dispose
    /// events when they are detached from their respective elements. Once detached
    /// they are not reusable.
    /// </summary>
    public abstract class EventListener : RocketNativeObject, IDisposable
    {
        private static Event pooledEvent = new Event();
        private GCHandle gcHandle;
        private CallbackHandler callbackHandler;

        public EventListener()
        {
            gcHandle = GCHandle.Alloc(this, GCHandleType.Normal);
            callbackHandler = new CallbackHandler();
            setPtr(callbackHandler.create(this));
        }

        public void Dispose()
        {
            ManagedEventListener_Delete(ptr);
            gcHandle.Free();
        }

        public abstract void ProcessEvent(Event evt);

        private void processEventCbImpl(IntPtr evt)
        {
            //Not thread safe, but prevents gc thrashing with lots of events
            pooledEvent.changePtr(evt);
            ProcessEvent(pooledEvent);
        }

        private void onAttachCbImpl(IntPtr element)
        {
            
        }

        private void onDetatchCbImpl(IntPtr element)
        {
            Dispose();
        }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ProcessEventCb(IntPtr evt
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void AttachDetatchCb(IntPtr element
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ManagedEventListener_Create(ProcessEventCb processEvent, AttachDetatchCb onAttach, AttachDetatchCb onDetatch
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedEventListener_Delete(IntPtr managedEventListener);

#if FULL_AOT_COMPILE
        /// <summary>
        /// This does not dispose since we have the GCHandle always for this class
        /// </summary>
        class CallbackHandler
        {
            private static ProcessEventCb processEventCb;
            private static AttachDetatchCb onAttachCb;
            private static AttachDetatchCb onDetatchCb;

            static CallbackHandler()
            {
                processEventCb = new ProcessEventCb(processEventCbImpl);
                onAttachCb = new AttachDetatchCb(onAttachCbImpl);
                onDetatchCb = new AttachDetatchCb(onDetatchCbImpl);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(ProcessEventCb))]
            private static void processEventCbImpl(IntPtr evt, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as EventListener).processEventCbImpl(evt);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(AttachDetatchCb))]
            private static void onAttachCbImpl(IntPtr element, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as EventListener).onAttachCbImpl(element);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(AttachDetatchCb))]
            private static void onDetatchCbImpl(IntPtr element, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as EventListener).onDetatchCbImpl(element);
            }

            public IntPtr create(EventListener obj)
            {
                return ManagedEventListener_Create(processEventCb, onAttachCb, onDetatchCb, GCHandle.ToIntPtr(obj.gcHandle));
            }
        }
#else
        /// <summary>
        /// This does not dispose since we have the GCHandle always for this class
        /// </summary>
        class CallbackHandler
        {
            private ProcessEventCb processEventCb;
            private AttachDetatchCb onAttachCb;
            private AttachDetatchCb onDetatchCb;

            public IntPtr create(EventListener obj)
            {
                processEventCb = new ProcessEventCb(obj.processEventCbImpl);
                onAttachCb = new AttachDetatchCb(obj.onAttachCbImpl);
                onDetatchCb = new AttachDetatchCb(obj.onDetatchCbImpl);

                return ManagedEventListener_Create(processEventCb, onAttachCb, onDetatchCb); ;
            }
        }
#endif

        #endregion
    }
}

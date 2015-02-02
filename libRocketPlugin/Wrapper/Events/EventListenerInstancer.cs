using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public abstract class EventListenerInstancer : ReferenceCountable
    {
        private CallbackHandler callbackHandler;

        public EventListenerInstancer()
        {
            callbackHandler = new CallbackHandler();
            setPtr(callbackHandler.create(this));
        }

        public override void Dispose()
        {
            base.Dispose();
            callbackHandler.Dispose();
        }

        public abstract EventListener InstanceEventListener(String name, Element element);

        public virtual void Release()
        {

        }

        private IntPtr InstanceEventListenerCbImpl(String name, IntPtr element)
        {
            EventListener evtListener = InstanceEventListener(name, ElementManager.getElement(element));
            if (evtListener != null)
            {
                return evtListener.Ptr;
            }
            return IntPtr.Zero;
        }

        private void ReleaseCbImpl()
        {
            Release();
            ManagedEventListenerInstancer_Delete(ptr);
        }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr InstanceEventListenerCb(String name, IntPtr element
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ReleaseCb(
#if FULL_AOT_COMPILE
        IntPtr instanceHandle
#endif
);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ManagedEventListenerInstancer_Create(InstanceEventListenerCb instanceEventListener, ReleaseCb release
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedEventListenerInstancer_Delete(IntPtr eventListenerInstancer);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static InstanceEventListenerCb instanceEventListenerCb;
            private static ReleaseCb releaseCb;

            static CallbackHandler()
            {
                instanceEventListenerCb = new InstanceEventListenerCb(InstanceEventListenerCbImpl);
                releaseCb = new ReleaseCb(ReleaseCbImpl);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(InstanceEventListenerCb))]
            private static IntPtr InstanceEventListenerCbImpl(String name, IntPtr element, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as EventListenerInstancer).InstanceEventListenerCbImpl(name, element);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(ReleaseCb))]
            private static void ReleaseCbImpl(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as EventListenerInstancer).ReleaseCbImpl();
            }

            private GCHandle handle;

            public IntPtr create(EventListenerInstancer obj)
            {
                handle = GCHandle.Alloc(obj);
                return ManagedEventListenerInstancer_Create(instanceEventListenerCb, releaseCb, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            InstanceEventListenerCb instanceEventListenerCb;
            ReleaseCb releaseCb;

            public IntPtr create(EventListenerInstancer obj)
            {
                instanceEventListenerCb = new InstanceEventListenerCb(obj.InstanceEventListenerCbImpl);
                releaseCb = new ReleaseCb(obj.ReleaseCbImpl);

                return ManagedEventListenerInstancer_Create(instanceEventListenerCb, releaseCb);
            }

            public void Dispose()
            {

            }
        }
#endif

        #endregion
    }
}

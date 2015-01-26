using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    /// <summary>
    /// This class creates actual events.
    /// 
    /// Not currently implemented past the wrapper.
    /// </summary>
    public class EventInstancer : ReferenceCountable
    {
        private CallbackHandler callbackHandler;

        public EventInstancer()
        {
            callbackHandler = new CallbackHandler();
            setPtr(callbackHandler.create(this));
        }

        public override void Dispose()
        {
            base.Dispose();
            callbackHandler.Dispose();
        }

        private IntPtr InstanceEventCbImpl(IntPtr elementTarget, String name, IntPtr parameters, bool interuptable)
        {
            throw new NotImplementedException();
        }

        private void ReleaseEventCbImpl(IntPtr evt)
        {
            throw new NotImplementedException();
        }

        private void ReleaseCbImpl()
        {
            throw new NotImplementedException();
        }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr InstanceEventCb(IntPtr elementTarget, String name, IntPtr parameters, [MarshalAs(UnmanagedType.I1)] bool interuptable
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ReleaseEventCb(IntPtr evt
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

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr ManagedEventInstancer_Create(InstanceEventCb instanceEvent, ReleaseEventCb releaseEvent, ReleaseCb release
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedEventInstancer_Delete(IntPtr eventInstancer);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static InstanceEventCb instanceEventCb;
            private static ReleaseEventCb releaseEventCb;
            private static ReleaseCb releaseCb;

            static CallbackHandler()
            {
                instanceEventCb = new InstanceEventCb(InstanceEventCbImpl);
                releaseEventCb = new ReleaseEventCb(ReleaseEventCbImpl);
                releaseCb = new ReleaseCb(ReleaseCbImpl);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(InstanceEventCb))]
            private static IntPtr InstanceEventCbImpl(IntPtr elementTarget, string name, IntPtr parameters, bool interuptable, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as EventInstancer).InstanceEventCbImpl(elementTarget, name, parameters, interuptable);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(ReleaseEventCb))]
            private static void ReleaseEventCbImpl(IntPtr evt, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as EventInstancer).ReleaseEventCbImpl(evt);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(ReleaseCb))]
            private static void ReleaseCbImpl(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as EventInstancer).ReleaseCbImpl();
            }

            private GCHandle handle;

            public IntPtr create(EventInstancer obj)
            {
                handle = GCHandle.Alloc(obj);
                return ManagedEventInstancer_Create(instanceEventCb, releaseEventCb, releaseCb, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            private InstanceEventCb instanceEventCb;
            private ReleaseEventCb releaseEventCb;
            private ReleaseCb releaseCb;

            public IntPtr create(EventInstancer obj)
            {
                instanceEventCb = new InstanceEventCb(obj.InstanceEventCbImpl);
                releaseEventCb = new ReleaseEventCb(obj.ReleaseEventCbImpl);
                releaseCb = new ReleaseCb(obj.ReleaseCbImpl);

                return ManagedEventInstancer_Create(instanceEventCb, releaseEventCb, releaseCb);
            }

            public void Dispose()
            {

            }
        }
#endif

        #endregion
    }
}

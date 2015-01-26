using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    public enum KnownRenderSystemEvents
    {
        RenderSystemCapabilitiesCreated,
        DeviceLost,
        DeviceRestored,
        Unknown
    };

    public abstract class RenderSystemListener : IDisposable
    {
        private CallbackHandler callbackHandler;
        private IntPtr ptr;

        public RenderSystemListener()
        {
            callbackHandler = new CallbackHandler();
            ptr = callbackHandler.create(this);
        }

        public void Dispose()
        {
            ManagedRenderSystemListener_Delete(ptr);
            callbackHandler.Dispose();
        }

        public abstract void eventOccured();

        public KnownRenderSystemEvents EventType
        {
            get
            {
                return ManagedRenderSystemListener_getEventType(ptr);
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
        private static extern IntPtr ManagedRenderSystemListener_Create(EventOccuredCallback eventOccuredCb
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedRenderSystemListener_Delete(IntPtr listener);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern KnownRenderSystemEvents ManagedRenderSystemListener_getEventType(IntPtr listener);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void EventOccuredCallback(
#if FULL_AOT_COMPILE
        IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static EventOccuredCallback eventOccuredCb;

            static CallbackHandler()
            {
                eventOccuredCb = new EventOccuredCallback(eventOccured);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(EventOccuredCallback))]
            private static void eventOccured(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as RenderSystemListener).eventOccured();
            }

            private GCHandle handle;

            public IntPtr create(RenderSystemListener obj)
            {
                handle = GCHandle.Alloc(obj);
                return ManagedRenderSystemListener_Create(eventOccuredCb, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            private EventOccuredCallback eventOccuredCb;

            public IntPtr create(RenderSystemListener obj)
            {
                eventOccuredCb = obj.eventOccured;
                return ManagedRenderSystemListener_Create(eventOccuredCb);
            }

            public void Dispose()
            {

            }
        }
#endif

        #endregion
    }
}

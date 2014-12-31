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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void EventOccuredCallback();

        private IntPtr ptr;
        private EventOccuredCallback eventOccuredCb;

        public RenderSystemListener()
        {
            eventOccuredCb = eventOccured;
            ptr = ManagedRenderSystemListener_Create(eventOccuredCb);
        }

        public void Dispose()
        {
            ManagedRenderSystemListener_Delete(ptr);
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
        private static extern IntPtr ManagedRenderSystemListener_Create(EventOccuredCallback eventOccuredCb);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedRenderSystemListener_Delete(IntPtr listener);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern KnownRenderSystemEvents ManagedRenderSystemListener_getEventType(IntPtr listener);

        #endregion
    }
}

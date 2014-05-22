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
        private InstanceEventCb instanceEventCb;
        private ReleaseEventCb releaseEventCb;
        private ReleaseCb releaseCb;

        public EventInstancer()
        {
            instanceEventCb = new InstanceEventCb(InstanceEventCbImpl);
            releaseEventCb = new ReleaseEventCb(ReleaseEventCbImpl);
            releaseCb = new ReleaseCb(ReleaseCbImpl);

            setPtr(ManagedEventInstancer_Create(instanceEventCb, releaseEventCb, releaseCb));
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
        delegate IntPtr InstanceEventCb(IntPtr elementTarget, String name, IntPtr parameters, [MarshalAs(UnmanagedType.I1)] bool interuptable);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ReleaseEventCb(IntPtr evt);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ReleaseCb();

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr ManagedEventInstancer_Create(InstanceEventCb instanceEvent, ReleaseEventCb releaseEvent, ReleaseCb release);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedEventInstancer_Delete(IntPtr eventInstancer);

        #endregion
    }
}

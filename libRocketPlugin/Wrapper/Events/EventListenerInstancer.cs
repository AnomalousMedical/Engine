using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public abstract class EventListenerInstancer : ReferenceCountable
    {
        InstanceEventListenerCb instanceEventListenerCb;
        ReleaseCb releaseCb;

        public EventListenerInstancer()
        {
            instanceEventListenerCb = new InstanceEventListenerCb(InstanceEventListenerCbImpl);
            releaseCb = new ReleaseCb(ReleaseCbImpl);

            setPtr(ManagedEventListenerInstancer_Create(instanceEventListenerCb, releaseCb));
        }

        public abstract EventListener InstanceEventListener(String name, Element element);

        public virtual void Release()
        {

        }

        private IntPtr InstanceEventListenerCbImpl(String name, IntPtr element)
        {
            EventListener evtListener = InstanceEventListener(name, ElementManager.getElement(element));
            if(evtListener != null)
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
        delegate IntPtr InstanceEventListenerCb(String name, IntPtr element);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	    delegate void ReleaseCb();

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ManagedEventListenerInstancer_Create(InstanceEventListenerCb instanceEventListener, ReleaseCb release);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedEventListenerInstancer_Delete(IntPtr eventListenerInstancer);

        #endregion
    }
}

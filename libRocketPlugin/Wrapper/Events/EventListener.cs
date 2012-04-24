using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public abstract class EventListener : RocketNativeObject, IDisposable
    {
        private static Event pooledEvent = new Event();
        private ProcessEventCb processEventCb;
        private AttachDetatchCb onAttachCb;
        private AttachDetatchCb onDetatchCb;
        private EventListenerInstancer listenerInstancer;

        public EventListener()
        {
            processEventCb = new ProcessEventCb(processEventCbImpl);
            onAttachCb = new AttachDetatchCb(onAttachCbImpl);
            onDetatchCb = new AttachDetatchCb(onDetatchCbImpl);

            setPtr(ManagedEventListener_Create(processEventCb, onAttachCb, onDetatchCb));
        }

        public void Dispose()
        {
            ManagedEventListener_Delete(ptr);
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
            //If the listenerInstancer is not null then this was created by a EventListenerInstancer.
            //It must be alerted that this object is done so it can release its reference.
            if (listenerInstancer != null)
            {
                listenerInstancer.EventListenerFinished(this);
                Dispose();
            }
        }

        internal void setListenerInstancer(EventListenerInstancer listenerInstancer)
        {
            this.listenerInstancer = listenerInstancer;
        }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ProcessEventCb(IntPtr evt);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void AttachDetatchCb(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ManagedEventListener_Create(ProcessEventCb processEvent, AttachDetatchCb onAttach, AttachDetatchCb onDetatch);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedEventListener_Delete(IntPtr managedEventListener);

        #endregion
    }
}

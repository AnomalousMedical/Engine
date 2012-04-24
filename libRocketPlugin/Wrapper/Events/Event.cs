using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public class Event : RocketNativeObject
    {
        private StringRetriever stringRetriever = new StringRetriever();

        public enum EventPhase
        {
            PHASE_UNKNOWN, PHASE_CAPTURE, PHASE_TARGET, PHASE_BUBBLE
        }

        public Event()
        {

        }

        public Event(IntPtr evt)
            :base(evt)
        {

        }

        public EventPhase Phase
        {
            get
            {
                return Event_GetPhase(ptr);
            }
            internal set
            {
                Event_SetPhase(ptr, value);
            }
        }

        public Element CurrentElement
        {
            get
            {
                return new Element(Event_GetCurrentElement(ptr));
            }
            internal set
            {
                Event_SetCurrentElement(ptr, value.Ptr);
            }
        }

        public Element TargetElement
        {
            get
            {
                return new Element(Event_GetTargetElement(ptr));
            }
        }

        public String Type
        {
            get
            {
                Event_GetType(ptr, stringRetriever.StringCallback);
                return stringRetriever.CurrentString;
            }
        }

        public bool IsPropagating
        {
            get
            {
                return Event_IsPropagating(ptr);
            }
        }

        public void StopPropagation()
        {
            Event_StopPropagation(ptr);
        }

        internal void changePtr(IntPtr evt)
        {
            setPtr(evt);
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern EventPhase Event_GetPhase(IntPtr evt);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Event_SetPhase(IntPtr evt, EventPhase phase);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Event_SetCurrentElement(IntPtr evt, IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Event_GetCurrentElement(IntPtr evt);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Event_GetTargetElement(IntPtr evt);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Event_GetType(IntPtr evt, StringRetriever.Callback stringCb);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Event_IsPropagating(IntPtr evt);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Event_StopPropagation(IntPtr evt);

        #endregion
    }
}

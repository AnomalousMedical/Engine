using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace libRocketPlugin
{
    public class Event : RocketNativeObject
    {
        private StringRetriever stringRetriever = new StringRetriever();
        private RktDictionary parameters = new RktDictionary();

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
                return ElementManager.getElement(Event_GetCurrentElement(ptr));
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
                return ElementManager.getElement(Event_GetTargetElement(ptr));
            }
        }

        public String Type
        {
            get
            {
                Event_GetType(ptr, stringRetriever.StringCallback);
                return stringRetriever.retrieveString();
            }
        }

        public bool IsPropagating
        {
            get
            {
                return Event_IsPropagating(ptr);
            }
        }

        public RktDictionary Parameters
        {
            get
            {
                return parameters;
            }
        }

        public void StopPropagation()
        {
            Event_StopPropagation(ptr);
        }

        public byte Event_GetParameter(String key, byte default_value)
        {
            return Event_GetParameter_Byte(ptr, key, default_value);
        }

        public char GetParameter(String key, char default_value)
        {
            return Event_GetParameter_Char(ptr, key, default_value);
        }

        public float GetParameter(String key, float default_value)
        {
            return Event_GetParameter_Float(ptr, key, default_value);
        }

        public int GetParameter(String key, int default_value)
        {
            return Event_GetParameter_Int(ptr, key, default_value);
        }

        public String GetParameter(String key, String default_value)
        {
            Event_GetParameter_String(ptr, key, stringRetriever.StringCallback);
            if (stringRetriever.GotString)
            {
                return stringRetriever.retrieveString();
            }
            else
            {
                return default_value;
            }
        }

        public ushort GetParameter(String key, ushort default_value)
        {
            return Event_GetParameter_Word(ptr, key, default_value);
        }

        internal void changePtr(IntPtr evt)
        {
            setPtr(evt);
            parameters.changePointer(Event_GetParameters(evt));
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

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Event_GetParameters(IntPtr evt);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte Event_GetParameter_Byte(IntPtr evt, String key, byte default_value);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern char Event_GetParameter_Char(IntPtr evt, String key, char default_value);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float Event_GetParameter_Float(IntPtr evt, String key, float default_value);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Event_GetParameter_Int(IntPtr evt, String key, int default_value);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Event_GetParameter_String(IntPtr evt, String key, StringRetriever.Callback setString);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort Event_GetParameter_Word(IntPtr evt, String key, ushort default_value);

        #endregion
    }
}

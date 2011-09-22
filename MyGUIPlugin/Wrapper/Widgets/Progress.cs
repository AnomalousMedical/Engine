using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class Progress : Widget
    {
        public Progress(IntPtr progress)
            :base(progress)
        {

        }

        public uint Range
        {
            get
            {
                return Progress_getProgressRange(widget).ToUInt32();
            }
            set
            {
                Progress_setProgressRange(widget, new UIntPtr(value));
            }
        }

        public uint Position
        {
            get
            {
                return Progress_getProgressPosition(widget).ToUInt32();
            }
            set
            {
                Progress_setProgressPosition(widget, new UIntPtr(value));
            }
        }

        public bool AutoTrack
        {
            get
            {
                return Progress_getProgressAutoTrack(widget);
            }
            set
            {
                Progress_setProgressAutoTrack(widget, value);
            }
        }

        public Align StartPoint
        {
            get
            {
                return Progress_getProgressStartPoint(widget);
            }
            set
            {
                Progress_setProgressStartPoint(widget, value);
            }
        }

        #region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Progress_setProgressRange(IntPtr progress, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr Progress_getProgressRange(IntPtr progress);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Progress_setProgressPosition(IntPtr progress, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr Progress_getProgressPosition(IntPtr progress);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Progress_setProgressAutoTrack(IntPtr progress, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Progress_getProgressAutoTrack(IntPtr progress);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Progress_setProgressStartPoint(IntPtr progress, Align value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Align Progress_getProgressStartPoint(IntPtr progress);

        #endregion
    }
}

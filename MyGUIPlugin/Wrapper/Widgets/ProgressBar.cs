using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public enum FlowDirection
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop,
        MAX
    };

    public class ProgressBar : Widget
    {
        public ProgressBar(IntPtr progress)
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

        public FlowDirection FlowDirection
        {
            get
            {
                return Progress_getFlowDirection(widget);
            }
            set
            {
                Progress_setFlowDirection(widget, value);
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
        private static extern void Progress_setFlowDirection(IntPtr progress, FlowDirection value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern FlowDirection Progress_getFlowDirection(IntPtr progress);

        #endregion
    }
}

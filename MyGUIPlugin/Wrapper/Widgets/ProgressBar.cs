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
                return ProgressBar_getProgressRange(widget).ToUInt32();
            }
            set
            {
                ProgressBar_setProgressRange(widget, new UIntPtr(value));
            }
        }

        public uint Position
        {
            get
            {
                return ProgressBar_getProgressPosition(widget).ToUInt32();
            }
            set
            {
                ProgressBar_setProgressPosition(widget, new UIntPtr(value));
            }
        }

        public bool AutoTrack
        {
            get
            {
                return ProgressBar_getProgressAutoTrack(widget);
            }
            set
            {
                ProgressBar_setProgressAutoTrack(widget, value);
            }
        }

        public FlowDirection FlowDirection
        {
            get
            {
                return ProgressBar_getFlowDirection(widget);
            }
            set
            {
                ProgressBar_setFlowDirection(widget, value);
            }
        }

        #region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ProgressBar_setProgressRange(IntPtr progress, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr ProgressBar_getProgressRange(IntPtr progress);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ProgressBar_setProgressPosition(IntPtr progress, UIntPtr value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr ProgressBar_getProgressPosition(IntPtr progress);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ProgressBar_setProgressAutoTrack(IntPtr progress, bool value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ProgressBar_getProgressAutoTrack(IntPtr progress);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ProgressBar_setFlowDirection(IntPtr progress, FlowDirection value);

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern FlowDirection ProgressBar_getFlowDirection(IntPtr progress);

        #endregion
    }
}

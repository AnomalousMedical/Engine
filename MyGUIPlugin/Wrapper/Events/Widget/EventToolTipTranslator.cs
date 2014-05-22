using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;
using Engine;

namespace MyGUIPlugin
{
    class EventToolTipTranslator : MyGUIWidgetEventTranslator
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget, ToolTipType type, uint index, int x, int y);
        static ToolTipEventArgs eventArgs = new ToolTipEventArgs();

        private NativeEventDelegate nativeEventCallback;

        public EventToolTipTranslator()
        {
            nativeEventCallback = new NativeEventDelegate(nativeEvent);
        }

        public override void Dispose()
        {
            base.Dispose();
            nativeEventCallback = null;
        }

        protected override IntPtr doInitialize(Widget widget)
        {
            return EventToolTipTranslator_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, ToolTipType type, uint index, int x, int y)
        {
            //Fill out the ToolTipEventArgs
            eventArgs.Index = index;
            eventArgs.Type = type;
            eventArgs.Point = new Vector2(x, y);
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr EventToolTipTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}
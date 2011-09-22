using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    class EventMouseSetFocusTranslator : MyGUIWidgetEventTranslator
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget, IntPtr old);
        static FocusEventArgs eventArgs = new FocusEventArgs();

        private NativeEventDelegate nativeEventCallback;

        public EventMouseSetFocusTranslator()
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
            return EventMouseSetFocusTranslator_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, IntPtr old)
        {
            //Fill out the FocusEventArgs
            eventArgs.OtherWidget = WidgetManager.getWidget(old);
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr EventMouseSetFocusTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;
using Engine.Platform;

namespace MyGUIPlugin
{
    class EventKeyButtonReleasedTranslator : MyGUIWidgetEventTranslator
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget, KeyboardButtonCode key);
        static KeyEventArgs eventArgs = new KeyEventArgs();

        private NativeEventDelegate nativeEventCallback;

        public EventKeyButtonReleasedTranslator()
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
            return EventKeyButtonReleasedTranslator_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, KeyboardButtonCode key)
        {
            //Fill out the KeyEventArgs
            eventArgs.Key = key;
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr EventKeyButtonReleasedTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}
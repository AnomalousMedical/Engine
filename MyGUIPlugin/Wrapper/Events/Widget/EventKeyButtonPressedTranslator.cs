using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;
using Engine.Platform;

namespace MyGUIPlugin
{
    class EventKeyButtonPressedTranslator : MyGUIWidgetEventTranslator
    {
        delegate void NativeEventDelegate(IntPtr widget, KeyboardButtonCode key, char _char);
        static KeyEventArgs eventArgs = new KeyEventArgs();

        private NativeEventDelegate nativeEventCallback;

        public EventKeyButtonPressedTranslator()
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
            return EventKeyButtonPressedTranslator_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, KeyboardButtonCode key, char _char)
        {
            //Fill out the KeyEventArgs
            eventArgs.Key = key;
            eventArgs.Char = _char;
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr EventKeyButtonPressedTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}
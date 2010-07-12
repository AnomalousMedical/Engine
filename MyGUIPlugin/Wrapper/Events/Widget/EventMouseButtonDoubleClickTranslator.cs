using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    class EventMouseButtonDoubleClickTranslator : MyGUIEventTranslator
    {
        delegate void NativeEventDelegate(IntPtr widget);

        private NativeEventDelegate nativeEventCallback;

        public EventMouseButtonDoubleClickTranslator()
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
            return EventMouseButtonDoubleClickTranslator_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget)
        {
            fireEvent(EventArgs.Empty);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr EventMouseButtonDoubleClickTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}
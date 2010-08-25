using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    class EventMouseWheelTranslator : MyGUIEventTranslator
    {
        delegate void NativeEventDelegate(IntPtr widget, int relWheel);
        static MouseEventArgs eventArgs = new MouseEventArgs();

        private NativeEventDelegate nativeEventCallback;

        public EventMouseWheelTranslator()
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
            return EventMouseWheelTranslator_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, int relWheel)
        {
            //Fill out the MouseEventArgs
            eventArgs.RelativeWheelPosition = relWheel;
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr EventMouseWheelTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}
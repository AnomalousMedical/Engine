using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    public class WindowEventArgs : EventArgs
    {
        internal WindowEventArgs()
        {

        }

        public String Name { get; set; }

    }

    class EventWindowButtonPressedTranslator : MyGUIWidgetEventTranslator
    {
        delegate void NativeEventDelegate(IntPtr widget, IntPtr name);
        static WindowEventArgs eventArgs = new WindowEventArgs();

        private NativeEventDelegate nativeEventCallback;

        public EventWindowButtonPressedTranslator()
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
            return EventWindowButtonPressedTranslator_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, IntPtr name)
        {
            //Fill out the WindowEventArgs
            eventArgs.Name = Marshal.PtrToStringAnsi(name);
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr EventWindowButtonPressedTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}
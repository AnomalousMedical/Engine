using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs()
        {

        }

        public MessageBoxStyle Result { get; internal set; }
    }

    class EventMessageBoxResultTranslator : MyGUIWidgetEventTranslator
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget, MessageBoxStyle result);
        static MessageEventArgs eventArgs = new MessageEventArgs();

        private NativeEventDelegate nativeEventCallback;

        public EventMessageBoxResultTranslator()
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
            return EventMessageBoxResultTranslator_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, MessageBoxStyle result)
        {
            //Fill out the MessageEventArgs
            eventArgs.Result = result;
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr EventMessageBoxResultTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}
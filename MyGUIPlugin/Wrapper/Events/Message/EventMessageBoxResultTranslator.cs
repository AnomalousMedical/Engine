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

    public delegate void EventMessageResult(Message message, MessageEventArgs eventArgs);

    class EventMessageBoxResultTranslator : MyGUIEventTranslator
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr message, MessageBoxStyle result);
        static MessageEventArgs eventArgs = new MessageEventArgs();

        private NativeEventDelegate nativeEventCallback;
        private EventMessageResult boundEvent;
        private Message message;

        public EventMessageBoxResultTranslator(Message message)
        {
            this.message = message;
            nativeEventCallback = new NativeEventDelegate(nativeEvent);
            nativeEventTranslator = EventMessageBoxResultTranslator_Create(message.WidgetPtr, nativeEventCallback);
        }

        public override void Dispose()
        {
            base.Dispose();
            nativeEventCallback = null;
        }

        /// <summary>
        /// The event that will be called back.
        /// </summary>
        public event EventMessageResult BoundEvent
        {
            add
            {
                if (boundEvent == null)
                {
                    MyGUIEventTranslator_bindEvent(nativeEventTranslator);
                }
                boundEvent += value;
            }
            remove
            {
                boundEvent -= value;
                if (boundEvent == null)
                {
                    MyGUIEventTranslator_unbindEvent(nativeEventTranslator);
                }
            }
        }

        private void nativeEvent(IntPtr widget, MessageBoxStyle result)
        {
            //Fill out the MessageEventArgs
            eventArgs.Result = result;
            if (boundEvent != null)
            {
                boundEvent.Invoke(message, eventArgs);
            }
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr EventMessageBoxResultTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}
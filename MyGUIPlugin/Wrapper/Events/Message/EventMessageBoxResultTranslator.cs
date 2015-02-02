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
        static MessageEventArgs eventArgs = new MessageEventArgs();

        private CallbackHandler callbackHandler;
        private EventMessageResult boundEvent;
        private Message message;

        public EventMessageBoxResultTranslator(Message message)
        {
            this.message = message;
            callbackHandler = new CallbackHandler();
            nativeEventTranslator = callbackHandler.create(this, message);
        }

        public override void Dispose()
        {
            base.Dispose();
            callbackHandler.Dispose();
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
        private static extern IntPtr EventMessageBoxResultTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr message, MessageBoxStyle result
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static NativeEventDelegate nativeEventCallback;

            static CallbackHandler()
            {
                nativeEventCallback = new NativeEventDelegate(nativeEvent);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(NativeEventDelegate))]
            private static void nativeEvent(IntPtr message, MessageBoxStyle result, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as EventMessageBoxResultTranslator).nativeEvent(message, result);
            }

            private GCHandle handle;

            public IntPtr create(EventMessageBoxResultTranslator obj, Message message)
            {
                handle = GCHandle.Alloc(obj);
                return EventMessageBoxResultTranslator_Create(message.WidgetPtr, nativeEventCallback, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
                nativeEventCallback = null;
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            private NativeEventDelegate nativeEventCallback;

            public IntPtr create(EventMessageBoxResultTranslator obj, Message message)
            {
                nativeEventCallback = new NativeEventDelegate(obj.nativeEvent);
                return EventMessageBoxResultTranslator_Create(message.WidgetPtr, nativeEventCallback);
            }

            public void Dispose()
            {
                nativeEventCallback = null;
            }
        }
#endif

        #endregion
    }
}
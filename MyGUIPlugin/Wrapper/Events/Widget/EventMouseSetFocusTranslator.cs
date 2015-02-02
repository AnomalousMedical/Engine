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
        static FocusEventArgs eventArgs = new FocusEventArgs();

        private CallbackHandler callbackHandler;

        public EventMouseSetFocusTranslator()
        {
            callbackHandler = new CallbackHandler();
        }

        public override void Dispose()
        {
            base.Dispose();
            callbackHandler.Dispose();
        }

        protected override IntPtr doInitialize(Widget widget)
        {
            return callbackHandler.create(this, widget);
        }

        private void nativeEvent(IntPtr widget, IntPtr old)
        {
            //Fill out the FocusEventArgs
            eventArgs.OtherWidget = WidgetManager.getWidget(old);
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr EventMouseSetFocusTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget, IntPtr old
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
            private static void nativeEvent(IntPtr widget, IntPtr old, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as EventMouseSetFocusTranslator).nativeEvent(widget, old);
            }

            private GCHandle handle;

            public IntPtr create(EventMouseSetFocusTranslator obj, Widget widget)
            {
                handle = GCHandle.Alloc(obj);
                return EventMouseSetFocusTranslator_Create(widget.WidgetPtr, nativeEventCallback, GCHandle.ToIntPtr(handle));
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

            public IntPtr create(EventMouseSetFocusTranslator obj, Widget widget)
            {
                nativeEventCallback = new NativeEventDelegate(obj.nativeEvent);
                return EventMouseSetFocusTranslator_Create(widget.WidgetPtr, nativeEventCallback);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    class EventKeySetFocusTranslator : MyGUIWidgetEventTranslator
    {
        static FocusEventArgs eventArgs = new FocusEventArgs();

        private CallbackHandler callbackHandler;

        public EventKeySetFocusTranslator()
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
        private static extern IntPtr EventKeySetFocusTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback
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

            [MonoTouch.MonoPInvokeCallback(typeof(NativeEventDelegate))]
            private static void nativeEvent(IntPtr widget, IntPtr old, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as EventKeySetFocusTranslator).nativeEvent(widget, old);
            }

            private GCHandle handle;

            public IntPtr create(EventKeySetFocusTranslator obj, Widget widget)
            {
                handle = GCHandle.Alloc(obj);
                return EventKeySetFocusTranslator_Create(widget.WidgetPtr, nativeEventCallback, GCHandle.ToIntPtr(handle));
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

            public IntPtr create(EventKeySetFocusTranslator obj, Widget widget)
            {
                nativeEventCallback = new NativeEventDelegate(obj.nativeEvent);
                return EventKeySetFocusTranslator_Create(widget.WidgetPtr, nativeEventCallback);
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
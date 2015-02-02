using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using Engine.Platform;

namespace MyGUIPlugin
{
    class EventMouseMoveTranslator : MyGUIWidgetEventTranslator
    {
        static MouseEventArgs eventArgs = new MouseEventArgs();

        private CallbackHandler callbackHandler;

        public EventMouseMoveTranslator()
        {
            callbackHandler = new CallbackHandler();
            eventArgs.Button = MouseButtonCode.NUM_BUTTONS;
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

        private void nativeEvent(IntPtr widget, int left, int top)
        {
            //Fill out the MouseEventArgs
            eventArgs.Position = new IntVector2(left, top);
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr EventMouseMoveTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget, int left, int top
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
            private static void nativeEvent(IntPtr widget, int left, int top, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as EventMouseMoveTranslator).nativeEvent(widget, left, top);
            }

            private GCHandle handle;

            public IntPtr create(EventMouseMoveTranslator obj, Widget widget)
            {
                handle = GCHandle.Alloc(obj);
                return EventMouseMoveTranslator_Create(widget.WidgetPtr, nativeEventCallback, GCHandle.ToIntPtr(handle));
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

            public IntPtr create(EventMouseMoveTranslator obj, Widget widget)
            {
                nativeEventCallback = new NativeEventDelegate(obj.nativeEvent);
                return EventMouseMoveTranslator_Create(widget.WidgetPtr, nativeEventCallback);
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

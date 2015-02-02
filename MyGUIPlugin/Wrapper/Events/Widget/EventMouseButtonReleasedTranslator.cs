using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;
using Engine.Platform;
using Engine;

namespace MyGUIPlugin
{
    class EventMouseButtonReleasedTranslator : MyGUIWidgetEventTranslator
    {
        static MouseEventArgs eventArgs = new MouseEventArgs();

        private CallbackHandler callbackHandler;

        public EventMouseButtonReleasedTranslator()
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

        private void nativeEvent(IntPtr widget, int left, int top, MouseButtonCode id)
        {
            //Fill out the MouseEventArgs
            eventArgs.Button = id;
            eventArgs.Position = new IntVector2(left, top);
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr EventMouseButtonReleasedTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget, int left, int top, MouseButtonCode id
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
            private static void nativeEvent(IntPtr widget, int left, int top, MouseButtonCode id, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as EventMouseButtonReleasedTranslator).nativeEvent(widget, left, top, id);
            }

            private GCHandle handle;

            public IntPtr create(EventMouseButtonReleasedTranslator obj, Widget widget)
            {
                handle = GCHandle.Alloc(obj);
                return EventMouseButtonReleasedTranslator_Create(widget.WidgetPtr, nativeEventCallback, GCHandle.ToIntPtr(handle));
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

            public IntPtr create(EventMouseButtonReleasedTranslator obj, Widget widget)
            {
                nativeEventCallback = new NativeEventDelegate(obj.nativeEvent);
                return EventMouseButtonReleasedTranslator_Create(widget.WidgetPtr, nativeEventCallback);
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
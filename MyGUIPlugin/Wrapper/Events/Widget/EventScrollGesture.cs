using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;
using Engine;

namespace MyGUIPlugin
{
    class EventScrollGesture : MyGUIWidgetEventTranslator
    {
        static ScrollGestureEventArgs eventArgs = new ScrollGestureEventArgs();

        private CallbackHandler callbackHandler;

        public EventScrollGesture()
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

        private void nativeEvent(IntPtr widget, int absx, int absy, int deltax, int deltay)
        {
            //Fill out the ScrollGestureEventArgs
            eventArgs.AbsX = absx;
            eventArgs.AbsY = absy;
            eventArgs.DeltaX = deltax;
            eventArgs.DeltaY = deltay;
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr EventScrollGesture_Create(IntPtr widget, NativeEventDelegate nativeEventCallback
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget, int absx, int absy, int deltax, int deltay
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
            private static void nativeEvent(IntPtr widget, int absx, int absy, int deltax, int deltay, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as EventScrollGesture).nativeEvent(widget, absx, absy, deltax, deltay);
            }

            private GCHandle handle;

            public IntPtr create(EventScrollGesture obj, Widget widget)
            {
                handle = GCHandle.Alloc(obj);
                return EventScrollGesture_Create(widget.WidgetPtr, nativeEventCallback, GCHandle.ToIntPtr(handle));
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

            public IntPtr create(EventScrollGesture obj, Widget widget)
            {
                nativeEventCallback = new NativeEventDelegate(obj.nativeEvent);
                return EventScrollGesture_Create(widget.WidgetPtr, nativeEventCallback);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    public class ScrollEventArgs : EventArgs
    {
        public uint Position { get; internal set; }
    }

    class ScrollChangePositionET : MyGUIWidgetEventTranslator
    {
        static ScrollEventArgs eventArgs = new ScrollEventArgs();

        private CallbackHandler callbackHandler;

        public ScrollChangePositionET()
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

        private void nativeEvent(IntPtr widget, UIntPtr position)
        {
            eventArgs.Position = position.horriblyUnsafeToUInt32();
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr ScrollChangePositionET_Create(IntPtr widget, NativeEventDelegate nativeEventCallback
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget, UIntPtr position
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
            private static void nativeEvent(IntPtr widget, UIntPtr position, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ScrollChangePositionET).nativeEvent(widget, position);
            }

            private GCHandle handle;

            public IntPtr create(ScrollChangePositionET obj, Widget widget)
            {
                handle = GCHandle.Alloc(obj);
                return ScrollChangePositionET_Create(widget.WidgetPtr, nativeEventCallback, GCHandle.ToIntPtr(handle));
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

            public IntPtr create(ScrollChangePositionET obj, Widget widget)
            {
                nativeEventCallback = new NativeEventDelegate(obj.nativeEvent);
                return ScrollChangePositionET_Create(widget.WidgetPtr, nativeEventCallback);
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
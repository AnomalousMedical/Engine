﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    class ClickEventTranslator : MyGUIWidgetEventTranslator
    {
        private CallbackHandler callbackHandler;

        public ClickEventTranslator()
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

        private void nativeEvent(IntPtr widget)
        {
            fireEvent(EventArgs.Empty);
        }

#region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr ClickEventTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget
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
            private static void nativeEvent(IntPtr widget, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ClickEventTranslator).nativeEvent(widget);
            }

            private GCHandle handle;

            public IntPtr create(ClickEventTranslator obj, Widget widget)
            {
                handle = GCHandle.Alloc(obj);
                return ClickEventTranslator_Create(widget.WidgetPtr, nativeEventCallback, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            private NativeEventDelegate nativeEventCallback;

            public IntPtr create(ClickEventTranslator obj, Widget widget)
            {
                nativeEventCallback = new NativeEventDelegate(obj.nativeEvent);
                return ClickEventTranslator_Create(widget.WidgetPtr, nativeEventCallback);
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

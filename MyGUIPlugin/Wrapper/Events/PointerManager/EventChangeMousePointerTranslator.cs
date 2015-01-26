using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    public delegate void MousePointerChanged(String pointerName);

    class EventChangeMousePointerTranslator : MyGUIEventTranslator
    {
        private CallbackHandler callbackHandler;
        private MousePointerChanged boundEvent;

        public EventChangeMousePointerTranslator(PointerManager pointerManager)
        {
            callbackHandler = new CallbackHandler();
            nativeEventTranslator = callbackHandler.create(this, pointerManager);
        }

        public override void Dispose()
        {
            base.Dispose();
            callbackHandler.Dispose();
        }

        /// <summary>
        /// The event that will be called back.
        /// </summary>
        public event MousePointerChanged BoundEvent
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

        private void nativeEvent(IntPtr pointerName)
        {
            if (boundEvent != null)
            {
                boundEvent.Invoke(Marshal.PtrToStringAnsi(pointerName));
            }
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr EventChangeMousePointerTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr pointerName
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
            private static void nativeEvent(IntPtr pointerName, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as EventChangeMousePointerTranslator).nativeEvent(pointerName);
            }

            private GCHandle handle;

            public IntPtr create(EventChangeMousePointerTranslator obj, PointerManager pointerManager)
            {
                handle = GCHandle.Alloc(obj);
                return EventChangeMousePointerTranslator_Create(pointerManager.PointerManagerPtr, nativeEventCallback, GCHandle.ToIntPtr(handle));
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

            public IntPtr create(EventChangeMousePointerTranslator obj, PointerManager pointerManager)
            {
                nativeEventCallback = new NativeEventDelegate(obj.nativeEvent);
                return EventChangeMousePointerTranslator_Create(pointerManager.PointerManagerPtr, nativeEventCallback);
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
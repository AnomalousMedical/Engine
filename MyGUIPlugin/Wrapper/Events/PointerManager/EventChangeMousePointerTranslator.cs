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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr pointerName);

        private NativeEventDelegate nativeEventCallback;
        private MousePointerChanged boundEvent;

        public EventChangeMousePointerTranslator(PointerManager pointerManager)
        {
            nativeEventCallback = new NativeEventDelegate(nativeEvent);
            nativeEventTranslator = EventChangeMousePointerTranslator_Create(pointerManager.PointerManagerPtr, nativeEventCallback);
        }

        public override void Dispose()
        {
            base.Dispose();
            nativeEventCallback = null;
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

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr EventChangeMousePointerTranslator_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}
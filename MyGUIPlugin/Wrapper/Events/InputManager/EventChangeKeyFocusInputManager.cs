using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    class EventChangeKeyFocusInputManager : MyGUIEventTranslator
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget);

        private NativeEventDelegate nativeEventCallback;
        private FocusChangedEvent boundEvent;

        public EventChangeKeyFocusInputManager(InputManager inputManager)
        {
            nativeEventCallback = new NativeEventDelegate(nativeEvent);
            nativeEventTranslator = EventChangeKeyFocusInputManager_Create(inputManager.Ptr, nativeEventCallback);
        }

        public override void Dispose()
        {
            base.Dispose();
            nativeEventCallback = null;
        }

        /// <summary>
        /// The event that will be called back.
        /// </summary>
        public event FocusChangedEvent BoundEvent
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

        private void nativeEvent(IntPtr widget)
        {
            if (boundEvent != null)
            {
                boundEvent.Invoke(WidgetManager.getWidget(widget));
            }
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr EventChangeKeyFocusInputManager_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}
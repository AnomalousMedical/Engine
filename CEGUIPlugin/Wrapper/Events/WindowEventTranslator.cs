using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CEGUIPlugin
{
    delegate void FireWindowEventDelegate(EventArgs args);
    
    class WindowEventTranslator : IDisposable
    {
        FireWindowEventDelegate fireWindowEvent;
        Window ceguiWindow;
        IntPtr nativeEventTranslator;
        BasicEventDelegate basicEventCallback;

        public WindowEventTranslator(String eventName, FireWindowEventDelegate fireWindowEvent, Window ceguiWindow)
        {
            this.fireWindowEvent = fireWindowEvent;
            this.ceguiWindow = ceguiWindow;
            basicEventCallback = new BasicEventDelegate(basicEvent);
            nativeEventTranslator = WindowEventTranslator_create(eventName, basicEventCallback);
        }

        public void Dispose()
        {
            WindowEventTranslator_delete(nativeEventTranslator);
            nativeEventTranslator = IntPtr.Zero;
            basicEventCallback = null;
        }

        public void bindEvent()
        {
            WindowEventTranslator_bindEvent(nativeEventTranslator, ceguiWindow.CEGUIWindow);
        }

        public void unbindEvent()
        {
            WindowEventTranslator_unbindEvent(nativeEventTranslator);
        }

        bool basicEvent()
        {
            fireWindowEvent(null);

            return true;
        }

#region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool BasicEventDelegate();

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr WindowEventTranslator_create(String eventName, BasicEventDelegate basicEvent);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowEventTranslator_delete(IntPtr nativeEventTranslator);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowEventTranslator_bindEvent(IntPtr nativeEventTranslator, IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowEventTranslator_unbindEvent(IntPtr nativeEventTranslator);

#endregion
    }
}

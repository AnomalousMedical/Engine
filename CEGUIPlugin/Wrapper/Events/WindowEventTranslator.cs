using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CEGUIPlugin
{
    class WindowEventTranslator : CEGUIEventTranslator
    {
        WindowEventDelegate eventCallback;

        public WindowEventTranslator(String eventName, Window window)
            : base(window)
        {
            eventCallback = new WindowEventDelegate(eventRecieved);
            setNativeEventTranslator(WindowEventTranslator_create(eventName, eventCallback));
        }

        private bool eventRecieved(IntPtr window)
        {
            return fireEvent(new WindowEventArgs(WindowManager.Singleton.getWindow(window)));
        }

#region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool WindowEventDelegate(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr WindowEventTranslator_create(String eventName, WindowEventDelegate eventDelegate);

#endregion
    }
}

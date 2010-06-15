using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CEGUIPlugin
{
    class KeyEventTranslator : CEGUIEventTranslator
    {
        KeyEventDelegate eventCallback;

        public KeyEventTranslator(String eventName, Window window)
            : base(window)
        {
            eventCallback = new KeyEventDelegate(eventRecieved);
            setNativeEventTranslator(KeyEventTranslator_create(eventName, eventCallback));
        }

        private bool eventRecieved(IntPtr window, uint codepoint, KeyScan scancode, uint sysKeys)
        {
            return fireEvent(new KeyEventArgs(WindowManager.Instance.getWindow(window), codepoint, scancode, sysKeys));
        }

#region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool KeyEventDelegate(IntPtr window, uint codepoint, KeyScan scancode, uint sysKeys);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr KeyEventTranslator_create(String eventName, KeyEventDelegate eventDelegate);

#endregion
    }
}

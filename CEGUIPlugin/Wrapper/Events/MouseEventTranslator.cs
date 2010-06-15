using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace CEGUIPlugin
{
    class MouseEventTranslator : CEGUIEventTranslator
    {
        MouseEventDelegate eventCallback;

        public MouseEventTranslator(String eventName, Window window)
            : base(window)
        {
            eventCallback = new MouseEventDelegate(eventRecieved);
            setNativeEventTranslator(MouseEventTranslator_create(eventName, eventCallback));
        }

        private bool eventRecieved(IntPtr window, Vector2 position, Vector2 moveDelta, MouseButton button, uint sysKeys, float wheelChange, uint clickCount)
        {
            return fireEvent(new MouseEventArgs(WindowManager.Instance.getWindow(window), position, moveDelta, button, sysKeys, wheelChange, clickCount));
        }

#region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool MouseEventDelegate(IntPtr window, Vector2 position, Vector2 moveDelta, MouseButton button, uint sysKeys, float wheelChange, uint clickCount);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr MouseEventTranslator_create(String eventName, MouseEventDelegate eventDelegate);

#endregion
    }
}

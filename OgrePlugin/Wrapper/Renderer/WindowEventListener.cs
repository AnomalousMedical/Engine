using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    abstract class WindowEventListener : IDisposable
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void WindowEventDelegate(IntPtr renderWindow);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool WindowClosingDelegate(IntPtr renderWindow);

        IntPtr listener;

        WindowEventDelegate windowMovedCallback;
        WindowEventDelegate windowResizedCallback;
        WindowClosingDelegate windowClosingCallback;
        WindowEventDelegate windowClosedCallback;
        WindowEventDelegate windowFocusChangeCallback;

        public WindowEventListener()
        {
            windowMovedCallback = new WindowEventDelegate(windowMoved);
            windowResizedCallback = new WindowEventDelegate(windowResized);
            windowClosingCallback = new WindowClosingDelegate(windowClosing);
            windowClosedCallback = new WindowEventDelegate(windowClosed);
            windowFocusChangeCallback = new WindowEventDelegate(windowFocusChange);

            listener = NativeWindowListener_Create(windowMovedCallback, windowResizedCallback, windowClosingCallback, windowClosedCallback, windowFocusChangeCallback);
        }

        public virtual void Dispose()
        {
            NativeWindowListener_Delete(listener);

            windowMovedCallback = null;
            windowResizedCallback = null;
            windowClosingCallback = null;
            windowClosedCallback = null;
            windowFocusChangeCallback = null;
        }

        internal IntPtr OgreListener
        {
            get
            {
                return listener;
            }
        }

        /// <summary>
        /// Called when the window has moved position.
        /// </summary>
        /// <param name="renderWindow"></param>
        protected abstract void windowMoved(IntPtr renderWindow);

        /// <summary>
        /// Called when the window has changed size.
        /// </summary>
        /// <param name="renderWindow"></param>
        protected abstract void windowResized(IntPtr renderWindow);

        /// <summary>
        /// Called when the window is closing only if x is pressed. Return true to close the window, false to cancel.
        /// </summary>
        /// <param name="renderWindow"></param>
        /// <returns>Return true to close the window, false to cancel.</returns>
        protected abstract bool windowClosing(IntPtr renderWindow);

        /// <summary>
        /// Called when the window is closed only if x is pressed.
        /// </summary>
        /// <param name="renderWindow"></param>
        protected abstract void windowClosed(IntPtr renderWindow);

        /// <summary>
        /// Window has lost/gained focus.
        /// </summary>
        /// <param name="renderWindow"></param>
        protected abstract void windowFocusChange(IntPtr renderWindow);

#region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr NativeWindowListener_Create(WindowEventDelegate windowMovedCallback, WindowEventDelegate windowResizedCallback, WindowClosingDelegate windowClosingCallback, WindowEventDelegate windowClosedCallback, WindowEventDelegate windowFocusChangeCallback);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeWindowListener_Delete(IntPtr windowListener);

#endregion
    }
}

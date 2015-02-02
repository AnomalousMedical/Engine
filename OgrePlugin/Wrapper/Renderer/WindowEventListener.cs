using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    abstract class WindowEventListener : IDisposable
    {
        IntPtr listener;
        private CallbackHandler callbackHandler;

        public WindowEventListener()
        {
            callbackHandler = new CallbackHandler();
            listener = callbackHandler.create(this);
        }

        public virtual void Dispose()
        {
            NativeWindowListener_Delete(listener);
            callbackHandler.Dispose();
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

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr NativeWindowListener_Create(WindowEventDelegate windowMovedCallback, WindowEventDelegate windowResizedCallback, WindowClosingDelegate windowClosingCallback, WindowEventDelegate windowClosedCallback, WindowEventDelegate windowFocusChangeCallback
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeWindowListener_Delete(IntPtr windowListener);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void WindowEventDelegate(IntPtr renderWindow
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool WindowClosingDelegate(IntPtr renderWindow
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            static WindowEventDelegate windowMovedCallback;
            static WindowEventDelegate windowResizedCallback;
            static WindowClosingDelegate windowClosingCallback;
            static WindowEventDelegate windowClosedCallback;
            static WindowEventDelegate windowFocusChangeCallback;

            static CallbackHandler()
            {
                windowMovedCallback = new WindowEventDelegate(windowMoved);
                windowResizedCallback = new WindowEventDelegate(windowResized);
                windowClosingCallback = new WindowClosingDelegate(windowClosing);
                windowClosedCallback = new WindowEventDelegate(windowClosed);
                windowFocusChangeCallback = new WindowEventDelegate(windowFocusChange);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(WindowEventDelegate))]
            private static void windowMoved(IntPtr renderWindow, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as WindowEventListener).windowMoved(renderWindow);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(WindowEventDelegate))]
            private static void windowResized(IntPtr renderWindow, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as WindowEventListener).windowResized(renderWindow);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(WindowClosingDelegate))]
            private static bool windowClosing(IntPtr renderWindow, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as WindowEventListener).windowClosing(renderWindow);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(WindowEventDelegate))]
            private static void windowClosed(IntPtr renderWindow, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as WindowEventListener).windowClosed(renderWindow);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(WindowEventDelegate))]
            private static void windowFocusChange(IntPtr renderWindow, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as WindowEventListener).windowFocusChange(renderWindow);
            }

            private GCHandle handle;

            public IntPtr create(WindowEventListener obj)
            {
                handle = GCHandle.Alloc(obj);
                return NativeWindowListener_Create(windowMovedCallback, windowResizedCallback, windowClosingCallback, windowClosedCallback, windowFocusChangeCallback, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            WindowEventDelegate windowMovedCallback;
            WindowEventDelegate windowResizedCallback;
            WindowClosingDelegate windowClosingCallback;
            WindowEventDelegate windowClosedCallback;
            WindowEventDelegate windowFocusChangeCallback;

            public IntPtr create(WindowEventListener obj)
            {
                windowMovedCallback = new WindowEventDelegate(obj.windowMoved);
                windowResizedCallback = new WindowEventDelegate(obj.windowResized);
                windowClosingCallback = new WindowClosingDelegate(obj.windowClosing);
                windowClosedCallback = new WindowEventDelegate(obj.windowClosed);
                windowFocusChangeCallback = new WindowEventDelegate(obj.windowFocusChange);

                return NativeWindowListener_Create(windowMovedCallback, windowResizedCallback, windowClosingCallback, windowClosedCallback, windowFocusChangeCallback);
            }

            public void Dispose()
            {
                windowMovedCallback = null;
                windowResizedCallback = null;
                windowClosingCallback = null;
                windowClosedCallback = null;
                windowFocusChangeCallback = null;
            }
        }
#endif

#endregion
    }
}

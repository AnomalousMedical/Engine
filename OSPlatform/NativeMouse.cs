using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;
using System.Runtime.InteropServices;

namespace Anomalous.OSPlatform
{
    class NativeMouse : MouseHardware, IDisposable
    {
        private NativeOSWindow window;
        private IntPtr nativeMouse;
        private CallbackHandler callbackHandler;

        public NativeMouse(NativeOSWindow window, Mouse mouse)
            : base(mouse)
        {
            this.window = window;

            callbackHandler = new CallbackHandler();

            nativeMouse = callbackHandler.create(this, window);

            fireSizeChanged(window.WindowWidth, window.WindowHeight);
            window.Resized += window_Resized;
        }

        public void Dispose()
        {
            window.Resized -= window_Resized;
            NativeMouse_delete(nativeMouse);
            nativeMouse = IntPtr.Zero;
            callbackHandler.Dispose();
        }

        internal void injectButtonDown(MouseButtonCode button)
        {
            fireButtonDown(button);
        }

        internal void injectButtonUp(MouseButtonCode button)
        {
            fireButtonUp(button);
        }

        internal void injectMoved(int x, int y)
        {
            fireMoved(x, y);
        }

        internal void injectWheel(int z)
        {
            fireWheel(z);
        }

        void window_Resized(OSWindow window)
        {
            fireSizeChanged(window.WindowWidth, window.WindowHeight);
        }

        #region PInvoke

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr NativeMouse_new(IntPtr osWindow, MouseButtonDownDelegate mouseButtonDownCB, MouseButtonUpDelegate mouseButtonUpCB, MouseMoveDelegate mouseMoveCB, MouseWheelDelegate mouseWheelCB
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeMouse_delete(IntPtr mouse);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void MouseButtonDownDelegate(MouseButtonCode id
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void MouseButtonUpDelegate(MouseButtonCode id
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void MouseMoveDelegate(int absX, int absY
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void MouseWheelDelegate(int relZ
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static MouseButtonDownDelegate mouseButtonDownCB;
            private static MouseButtonUpDelegate mouseButtonUpCB;
            private static MouseMoveDelegate mouseMoveCB;
            private static MouseWheelDelegate mouseWheelCB;

            static CallbackHandler()
            {
                mouseButtonDownCB = new MouseButtonDownDelegate(fireButtonDown);
                mouseButtonUpCB = new MouseButtonUpDelegate(fireButtonUp);
                mouseMoveCB = new MouseMoveDelegate(fireMoved);
                mouseWheelCB = new MouseWheelDelegate(fireWheel);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(MouseButtonDownDelegate))]
            private static void fireButtonDown(MouseButtonCode id, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as NativeMouse).fireButtonDown(id);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(MouseButtonUpDelegate))]
            private static void fireButtonUp(MouseButtonCode id, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as NativeMouse).fireButtonUp(id);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(MouseMoveDelegate))]
            private static void fireMoved(int absX, int absY, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as NativeMouse).fireMoved(absX, absY);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(MouseWheelDelegate))]
            private static void fireWheel(int relZ, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as NativeMouse).fireWheel(relZ);
            }

            private GCHandle handle;

            public IntPtr create(NativeMouse obj, NativeOSWindow window)
            {
                handle = GCHandle.Alloc(obj);
                return NativeMouse_new(window._NativePtr, mouseButtonDownCB, mouseButtonUpCB, mouseMoveCB, mouseWheelCB, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            MouseButtonDownDelegate mouseButtonDownCB;
            MouseButtonUpDelegate mouseButtonUpCB;
            MouseMoveDelegate mouseMoveCB;
            MouseWheelDelegate mouseWheelCB;

            public IntPtr create(NativeMouse obj, NativeOSWindow window)
            {
                mouseButtonDownCB = new MouseButtonDownDelegate(obj.fireButtonDown);
                mouseButtonUpCB = new MouseButtonUpDelegate(obj.fireButtonUp);
                mouseMoveCB = new MouseMoveDelegate(obj.fireMoved);
                mouseWheelCB = new MouseWheelDelegate(obj.fireWheel);

                return NativeMouse_new(window._NativePtr, mouseButtonDownCB, mouseButtonUpCB, mouseMoveCB, mouseWheelCB);
            }

            public void Dispose()
            {

            }
        }
#endif
        #endregion
    }
}

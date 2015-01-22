using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;

namespace Anomalous.OSPlatform
{
    class NativeKeyboard : KeyboardHardware, IDisposable
    {
        private IntPtr nativeKeyboard;
        private CallbackHandler callbackHandler;

        private NativeOSWindow window;

        public NativeKeyboard(NativeOSWindow window, Keyboard keyboard)
            :base(keyboard)
        {
            this.window = window;
            window.Deactivated += window_Deactivated;

            callbackHandler = new CallbackHandler();

            nativeKeyboard = callbackHandler.create(this, window);
        }

        public void Dispose()
        {
            window.Deactivated -= window_Deactivated;
            NativeKeyboard_delete(nativeKeyboard);
            nativeKeyboard = IntPtr.Zero;
            callbackHandler.Dispose();
        }

        void window_Deactivated(OSWindow window)
        {
            fireReleaseAllKeys();
        }

        #region PInvoke

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr NativeKeyboard_new(IntPtr osWindow, KeyDownDelegate keyDownCB, KeyUpDelegate keyUpCB
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeKeyboard_delete(IntPtr keyboard);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void KeyDownDelegate(KeyboardButtonCode keyCode, uint character
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void KeyUpDelegate(KeyboardButtonCode keyCode
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static KeyDownDelegate keyDownCB;
            private static KeyUpDelegate keyUpCB;

            static CallbackHandler()
            {
                keyDownCB = new KeyDownDelegate(fireKeyPressed);
                keyUpCB = new KeyUpDelegate(fireKeyReleased);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(KeyDownDelegate))]
            private static void fireKeyPressed(KeyboardButtonCode keyCode, uint character, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as NativeKeyboard).fireKeyPressed(keyCode, character);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(KeyUpDelegate))]
            private static void fireKeyReleased(KeyboardButtonCode keyCode, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as NativeKeyboard).fireKeyReleased(keyCode);
            }

            private GCHandle handle;

            public IntPtr create(NativeKeyboard obj, NativeOSWindow window)
            {
                handle = GCHandle.Alloc(obj);
                return NativeKeyboard_new(window._NativePtr, keyDownCB, keyUpCB, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            private KeyDownDelegate keyDownCB;
            private KeyUpDelegate keyUpCB;

            public IntPtr create(NativeKeyboard obj, NativeOSWindow window)
            {
                keyDownCB = new KeyDownDelegate(obj.fireKeyPressed);
                keyUpCB = new KeyUpDelegate(obj.fireKeyReleased);

                return NativeKeyboard_new(window._NativePtr, keyDownCB, keyUpCB);
            }

            public void Dispose()
            {

            }
        }
#endif

        #endregion
    }
}

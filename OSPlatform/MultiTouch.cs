using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Platform;
using Logging;
using Engine;

namespace Anomalous.OSPlatform
{
    public enum TouchType
    {
        None,
        Trackpad,
        Screen
    }

    public class MultiTouch : TouchHardware, IDisposable
    {
        private IntPtr nativeMultiTouch;
        private CallbackHandler callbackHandler;

        public MultiTouch(NativeOSWindow nativeWindow, Touches touches) 
            :base(touches)
        {
            callbackHandler = new CallbackHandler();
            Log.Info("Activating MultiTouch on window {0}", nativeWindow._NativePtr);
            nativeMultiTouch = callbackHandler.create(this, nativeWindow);
        }

        public void Dispose()
        {
            MultiTouch_delete(nativeMultiTouch);
            callbackHandler.Dispose();
        }

        public static bool IsAvailable
        {
            get
            {
                return MultiTouch_isMultitouchAvailable();
            }
        }

        private void touchStarted(TouchInfo touchInfo)
        {
            fireTouchStarted(touchInfo);
        }

        private void touchEnded(TouchInfo touchInfo)
        {
            fireTouchEnded(touchInfo);
        }

        private void touchMoved(TouchInfo touchInfo)
        {
            fireTouchMoved(touchInfo);
        }

        private void allTouchesCanceled()
        {
            fireAllTouchesCanceled();
        }

#region PInvoke
        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MultiTouch_new(IntPtr nativeWindow, TouchEventDelegate touchStartedCB, TouchEventDelegate touchEndedCB, TouchEventDelegate touchMovedCB, TouchEventCanceledDelegate touchCanceledCB
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiTouch_delete(IntPtr multiTouch);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool MultiTouch_isMultitouchAvailable();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void TouchEventDelegate(TouchInfo touchInfo
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void TouchEventCanceledDelegate(
#if FULL_AOT_COMPILE
        IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static TouchEventDelegate touchStartedCB;
            private static TouchEventDelegate touchEndedCB;
            private static TouchEventDelegate touchMovedCB;
            private static TouchEventCanceledDelegate touchCanceledCB;

            static CallbackHandler()
            {
                touchStartedCB = new TouchEventDelegate(touchStarted);
                touchEndedCB = new TouchEventDelegate(touchEnded);
                touchMovedCB = new TouchEventDelegate(touchMoved);
                touchCanceledCB = new TouchEventCanceledDelegate(allTouchesCanceled);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(TouchEventDelegate))]
            private static void touchStarted(TouchInfo touchInfo, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as MultiTouch).touchStarted(touchInfo);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(TouchEventDelegate))]
            private static void touchEnded(TouchInfo touchInfo, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as MultiTouch).touchEnded(touchInfo);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(TouchEventDelegate))]
            private static void touchMoved(TouchInfo touchInfo, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as MultiTouch).touchMoved(touchInfo);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(TouchEventCanceledDelegate))]
            private static void allTouchesCanceled(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as MultiTouch).allTouchesCanceled();
            }

            private GCHandle handle;

            public IntPtr create(MultiTouch obj, NativeOSWindow nativeWindow)
            {
                GCHandle.Alloc(handle);
                return MultiTouch_new(nativeWindow._NativePtr, touchStartedCB, touchEndedCB, touchMovedCB, touchCanceledCB, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            TouchEventDelegate touchStartedCB;
            TouchEventDelegate touchEndedCB;
            TouchEventDelegate touchMovedCB;
            TouchEventCanceledDelegate touchCanceledCB;

            public IntPtr create(MultiTouch obj, NativeOSWindow nativeWindow)
            {
                touchStartedCB = new TouchEventDelegate(obj.touchStarted);
                touchEndedCB = new TouchEventDelegate(obj.touchEnded);
                touchMovedCB = new TouchEventDelegate(obj.touchMoved);
                touchCanceledCB = new TouchEventCanceledDelegate(obj.allTouchesCanceled);

                return MultiTouch_new(nativeWindow._NativePtr, touchStartedCB, touchEndedCB, touchMovedCB, touchCanceledCB);
            }

            public void Dispose()
            {

            }
        }
#endif

#endregion
    }
}

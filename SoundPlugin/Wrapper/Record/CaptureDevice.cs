
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SoundPlugin
{
    public unsafe class CaptureDevice : SoundPluginObject, IDisposable
    {
        public delegate void BufferFullCallback(byte* buffer, int length);

        private OpenALManager openAlManager;
        private CallbackHandler callbackHandler;

        internal CaptureDevice(IntPtr ptr, OpenALManager openAlManager)
            : base(ptr)
        {
            this.openAlManager = openAlManager;
            callbackHandler = new CallbackHandler(this);
        }

        public void Dispose()
        {
            openAlManager.destroyCaptureDevice(this);
            callbackHandler.Dispose();
        }

        public void start(BufferFullCallback callback)
        {
            callbackHandler.startCapture(callback);
        }

        public void stop()
        {
            callbackHandler.stopCapture();
        }

        public bool Valid
        {
            get
            {
                return CaptureDevice_IsValid(Pointer);
            }
        }

        #region PInvoke

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CaptureDevice_Start(IntPtr captureDevice, NativeBufferFullCallback callback
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CaptureDevice_Stop(IntPtr captureDevice);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CaptureDevice_IsValid(IntPtr captureDevice);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void NativeBufferFullCallback(byte* buffer, int length
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static NativeBufferFullCallback staticBufferFullCallback;

            static CallbackHandler()
            {
                staticBufferFullCallback = staticBufferFull;
            }

            [MonoTouch.MonoPInvokeCallback(typeof(BufferFullCallback))]
            private static void staticBufferFull(byte* buffer, int length, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as CallbackHandler).heldBuffer(buffer, length);
            }

            private BufferFullCallback heldBuffer = null;
            private CaptureDevice captureDevice;
            private GCHandle handle;

            public CallbackHandler(CaptureDevice captureDevice)
            {
                handle = GCHandle.Alloc(this);
                this.captureDevice = captureDevice;
            }

            public void Dispose()
            {
                handle.Free();
            }

            public void startCapture(BufferFullCallback callback)
            {
                heldBuffer = callback;
                CaptureDevice_Start(captureDevice.Pointer, staticBufferFullCallback, GCHandle.ToIntPtr(handle));
            }

            public void stopCapture()
            {
                CaptureDevice_Stop(captureDevice.Pointer);
                heldBuffer = null;
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            private NativeBufferFullCallback nativeBufferFull;
            private BufferFullCallback heldBuffer = null;
            private CaptureDevice captureDevice;

            public CallbackHandler(CaptureDevice captureDevice)
            {
                this.captureDevice = captureDevice;
                nativeBufferFull = bufferFull;
            }

            public void Dispose()
            {

            }

            public void startCapture(BufferFullCallback callback)
            {
                heldBuffer = callback;
                CaptureDevice_Start(captureDevice.Pointer, nativeBufferFull);
            }

            public void stopCapture()
            {
                CaptureDevice_Stop(captureDevice.Pointer);
                heldBuffer = null;
            }

            private void bufferFull(byte* buffer, int length)
            {
                heldBuffer(buffer, length);
            }
        }
#endif

        #endregion
    }
}

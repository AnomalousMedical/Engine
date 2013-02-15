
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SoundPlugin
{
    public unsafe class CaptureDevice : SoundPluginObject, IDisposable
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void BufferFullCallback(byte* buffer, int length);

        BufferFullCallback heldBuffer = null;
        private OpenALManager openAlManager;

        internal CaptureDevice(IntPtr ptr, OpenALManager openAlManager)
            : base(ptr)
        {
            this.openAlManager = openAlManager;
        }

        public void Dispose()
        {
            openAlManager.destroyCaptureDevice(this);
        }

        public void start(BufferFullCallback callback)
        {
            heldBuffer = callback;
            CaptureDevice_Start(Pointer, heldBuffer);
        }

        public void stop()
        {
            CaptureDevice_Stop(Pointer);
            heldBuffer = null;
        }

        public bool Valid
        {
            get
            {
                return CaptureDevice_IsValid(Pointer);
            }
        }

        #region PInvoke

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void CaptureDevice_Start(IntPtr captureDevice, BufferFullCallback callback);

        [DllImport("SoundWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void CaptureDevice_Stop(IntPtr captureDevice);

        [DllImport("SoundWrapper", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CaptureDevice_IsValid(IntPtr captureDevice);

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    public class HardwarePixelBuffer : IDisposable
    {
        internal static HardwarePixelBuffer createWrapper(IntPtr hardwarePixelBuffer)
        {
            return new HardwarePixelBuffer(hardwarePixelBuffer);
        }

        IntPtr hardwarePixelBuffer;
        RenderTexture renderTexture;

        private HardwarePixelBuffer(IntPtr hardwarePixelBuffer)
        {
            this.hardwarePixelBuffer = hardwarePixelBuffer;
        }

        public void Dispose()
        {
            hardwarePixelBuffer = IntPtr.Zero;
            if(renderTexture != null)
            {
                renderTexture.Dispose();
            }
        }

        public RenderTexture getRenderTarget()
        {
            if(renderTexture == null)
            {
                renderTexture = new RenderTexture(HardwarePixelBuffer_getRenderTarget(hardwarePixelBuffer));
            }
            return renderTexture;
        }

        public void blitFromMemory(PixelBox src, int left, int top, int right, int bottom)
        {
            HardwarePixelBuffer_blitFromMemory(hardwarePixelBuffer, src.OgreBox, left, top, right, bottom);
        }

        public void blitFromMemory(PixelBox src)
        {
            HardwarePixelBuffer_blitFromMemoryFill(hardwarePixelBuffer, src.OgreBox);
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr HardwarePixelBuffer_getRenderTarget(IntPtr hardwarePixelBuffer);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void HardwarePixelBuffer_blitFromMemory(IntPtr hardwarePixelBuffer, IntPtr src, int left, int top, int right, int bottom);
        
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void HardwarePixelBuffer_blitFromMemoryFill(IntPtr hardwarePixelBuffer, IntPtr src);

#endregion
    }
}

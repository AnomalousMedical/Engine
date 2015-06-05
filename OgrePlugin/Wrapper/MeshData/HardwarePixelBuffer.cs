using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    public class HardwarePixelBuffer : HardwareBuffer
    {
        internal static HardwarePixelBuffer createWrapper(IntPtr hardwarePixelBuffer)
        {
            return new HardwarePixelBuffer(hardwarePixelBuffer);
        }

        RenderTexture renderTexture;

        private HardwarePixelBuffer(IntPtr hardwarePixelBuffer)
            :base(hardwarePixelBuffer)
        {
            
        }

        public override void Dispose()
        {
            if(renderTexture != null)
            {
                renderTexture.Dispose();
            }
            base.Dispose();
        }

        public RenderTexture getRenderTarget()
        {
            if(renderTexture == null)
            {
                renderTexture = new RenderTexture(HardwarePixelBuffer_getRenderTarget(hardwareBuffer));
            }
            return renderTexture;
        }

        public void blitFromMemory(PixelBox src, int left, int top, int right, int bottom)
        {
            HardwarePixelBuffer_blitFromMemory(hardwareBuffer, src.OgreBox, left, top, right, bottom);
        }

        public void blitFromMemory(PixelBox src)
        {
            HardwarePixelBuffer_blitFromMemoryFill(hardwareBuffer, src.OgreBox);
        }

        public void blitToMemory(PixelBox dst)
        {
            HardwarePixelBuffer_blitToMemoryFill(hardwareBuffer, dst.OgreBox);
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr HardwarePixelBuffer_getRenderTarget(IntPtr hardwarePixelBuffer);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void HardwarePixelBuffer_blitFromMemory(IntPtr hardwarePixelBuffer, IntPtr src, int left, int top, int right, int bottom);
        
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void HardwarePixelBuffer_blitFromMemoryFill(IntPtr hardwarePixelBuffer, IntPtr src);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void HardwarePixelBuffer_blitToMemoryFill(IntPtr hardwarePixelBuffer, IntPtr src);

#endregion
    }
}

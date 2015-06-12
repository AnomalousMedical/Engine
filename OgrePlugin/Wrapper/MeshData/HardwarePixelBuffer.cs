using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

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

        public void blit(HardwarePixelBufferSharedPtr hardwarePixelBuffer, IntRect src, IntRect dest)
        {
            HardwarePixelBuffer_blit(hardwareBuffer, hardwarePixelBuffer.HeapSharedPtr, src.Left, src.Top, src.Right, src.Bottom, dest.Left, dest.Top, dest.Right, dest.Bottom);
        }

        /// <summary>
        /// Copies a region from normal memory to a region of this pixelbuffer.
        /// </summary>
        public void blitFromMemory(PixelBox src, IntRect dest)
        {
            HardwarePixelBuffer_blitFromMemory(hardwareBuffer, src.OgreBox, dest.Left, dest.Top, dest.Right, dest.Bottom);
        }

        /// <summary>
        /// Copies a region from normal memory to a region of this pixelbuffer.
        /// </summary>
        /// <param name="src"></param>
        public void blitFromMemory(PixelBox src)
        {
            HardwarePixelBuffer_blitFromMemoryFill(hardwareBuffer, src.OgreBox);
        }

        /// <summary>
        /// Copies a region of this pixelbuffer to normal memory.
        /// </summary>
        /// <param name="dst"></param>
        public void blitToMemory(PixelBox dst)
        {
            HardwarePixelBuffer_blitToMemoryFill(hardwareBuffer, dst.OgreBox);
        }

        /// <summary>
        /// Copies a region of this pixelbuffer to normal memory.
        /// </summary>
        /// <param name="srcBox"></param>
        /// <param name="dst"></param>
        public void blitToMemory(IntRect srcBox, PixelBox dst)
        {
            HardwarePixelBuffer_blitToMemory(hardwareBuffer, dst.OgreBox, srcBox.Left, srcBox.Top, srcBox.Right, srcBox.Bottom);
        }

        /// <summary>
        /// Lock the buffer for a given region and options. You can specify all 0's in your region
        /// to get the entire texture.
        /// </summary>
        /// <param name="region">The region to lock.</param>
        /// <param name="options">The options to set when locking.</param>
        /// <returns></returns>
        public void lockBuf(IntRect region, LockOptions options)
        {
            HardwarePixelBuffer_lock(hardwareBuffer, region.Left, region.Top, region.Right, region.Bottom, options);
        }
        
        /// <summary>
        /// Get the current pixel box lock on this buffer, only valid if you locked first. You do not have
        /// to manage the PixelBox returned, it can be garbage collected normally.
        /// </summary>
        public PixelBox getCurrentLock()
        {
            return new PixelBox(HardwarePixelBuffer_getCurrentLock(hardwareBuffer));
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr HardwarePixelBuffer_getRenderTarget(IntPtr hardwarePixelBuffer);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void HardwarePixelBuffer_blit(IntPtr hardwarePixelBuffer, IntPtr src, int srcLeft, int srcTop, int srcRight, int srcBottom, int dstLeft, int dstTop, int dstRight, int dstBottom);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void HardwarePixelBuffer_blitFromMemory(IntPtr hardwarePixelBuffer, IntPtr src, int left, int top, int right, int bottom);
        
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void HardwarePixelBuffer_blitFromMemoryFill(IntPtr hardwarePixelBuffer, IntPtr src);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void HardwarePixelBuffer_blitToMemoryFill(IntPtr hardwarePixelBuffer, IntPtr dst);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void HardwarePixelBuffer_blitToMemory(IntPtr hardwarePixelBuffer, IntPtr dst, int left, int top, int right, int bottom);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void HardwarePixelBuffer_lock(IntPtr hardwarePixelBuffer, int left, int top, int right, int bottom, LockOptions options);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr HardwarePixelBuffer_getCurrentLock(IntPtr hardwarePixelBuffer);

#endregion
    }
}

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

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern IntPtr HardwarePixelBuffer_getRenderTarget(IntPtr hardwarePixelBuffer);

#endregion
    }
}

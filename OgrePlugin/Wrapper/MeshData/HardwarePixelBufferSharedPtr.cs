using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public class HardwarePixelBufferSharedPtr : IDisposable
    {
        private SharedPtr<HardwarePixelBuffer> sharedPtr;

        internal HardwarePixelBufferSharedPtr(SharedPtr<HardwarePixelBuffer> sharedPtr)
        {
            this.sharedPtr = sharedPtr;
        }

        public void Dispose()
        {
            sharedPtr.Dispose();
        }

        public HardwarePixelBuffer Value
        {
            get
            {
                return sharedPtr.Value;
            }
        }

        internal IntPtr HeapSharedPtr
        {
            get
            {
                return sharedPtr.HeapSharedPtr;
            }
        }
    }
}

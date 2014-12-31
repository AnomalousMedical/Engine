using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class HardwareVertexBufferSharedPtr : IDisposable
    {
        private SharedPtr<HardwareVertexBuffer> sharedPtr;

        internal HardwareVertexBufferSharedPtr(SharedPtr<HardwareVertexBuffer> sharedPtr)
        {
            this.sharedPtr = sharedPtr;
        }

        public void Dispose()
        {
            sharedPtr.Dispose();
        }

        public HardwareVertexBuffer Value
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

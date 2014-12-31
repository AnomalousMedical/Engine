using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class HardwareIndexBufferSharedPtr : IDisposable
    {
        private SharedPtr<HardwareIndexBuffer> sharedPtr;

        internal HardwareIndexBufferSharedPtr(SharedPtr<HardwareIndexBuffer> sharedPtr)
        {
            this.sharedPtr = sharedPtr;
        }

        public void Dispose()
        {
            sharedPtr.Dispose();
        }

        public HardwareIndexBuffer Value
        {
            get
            {
                return sharedPtr.Value;
            }
        }
    }
}

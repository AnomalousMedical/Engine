using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgreNextPlugin
{
    [NativeSubsystemType]
    public class OgreDataStreamPtr : IDisposable
    {
        private SharedPtr<OgreDataStream> sharedPtr;

        internal OgreDataStreamPtr(SharedPtr<OgreDataStream> sharedPtr)
        {
            this.sharedPtr = sharedPtr;
        }

        public void Dispose()
        {
            if (sharedPtr != null)
            {
                sharedPtr.Dispose();
            }
        }

        public OgreDataStream Value
        {
            get
            {
                if (sharedPtr != null)
                {
                    return sharedPtr.Value;
                }
                return null;
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

using Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class GpuSharedParametersPtr : IDisposable
    {
        private SharedPtr<GpuSharedParameters> sharedPtr;

        internal GpuSharedParametersPtr(SharedPtr<GpuSharedParameters> sharedPtr)
        {
            this.sharedPtr = sharedPtr;
        }

        public void Dispose()
        {
            sharedPtr.Dispose();
        }

        public GpuSharedParameters Value
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

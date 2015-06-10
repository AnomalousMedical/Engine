using Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class GpuProgramParametersSharedPtr : IDisposable
    {
        private SharedPtr<GpuProgramParameters> sharedPtr;

        internal GpuProgramParametersSharedPtr(SharedPtr<GpuProgramParameters> sharedPtr)
        {
            this.sharedPtr = sharedPtr;
        }

        public void Dispose()
        {
            sharedPtr.Dispose();
        }

        public GpuProgramParameters Value
        {
            get
            {
                return sharedPtr.Value;
            }
        }
    }
}

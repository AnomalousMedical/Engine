using Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class HighLevelGpuProgramSharedPtr : IDisposable
    {
        private SharedPtr<HighLevelGpuProgram> sharedPtr;

        internal HighLevelGpuProgramSharedPtr(SharedPtr<HighLevelGpuProgram> sharedPtr)
        {
            this.sharedPtr = sharedPtr;
        }

        public void Dispose()
        {
            sharedPtr.Dispose();
        }

        public HighLevelGpuProgram Value
        {
            get
            {
                return sharedPtr.Value;
            }
        }
    }
}

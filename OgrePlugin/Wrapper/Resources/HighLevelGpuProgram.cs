using Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class HighLevelGpuProgram : GpuProgram
    {
        internal HighLevelGpuProgram(IntPtr ptr)
            :base(ptr)
        {
            
        }

        internal static HighLevelGpuProgram createWrapper(IntPtr nativeObject)
        {
            return new HighLevelGpuProgram(nativeObject);
        }
    }
}

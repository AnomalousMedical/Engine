using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Shim;

namespace OSPlatform.Win32
{
    class StackTraceShim : Engine.Shim.StackTrace
    {
        private System.Diagnostics.StackTrace trace;

        public StackTraceShim(System.Diagnostics.StackTrace trace)
        {
            this.trace = trace;
        }

        public StackFrame GetFrame(int index)
        {
            return new StackFrameShim(trace.GetFrame(index));
        }

        public IEnumerable<StackFrame> GetFrames()
        {
            return trace.GetFrames().Select(i => new StackFrameShim(i));
        }
    }
}

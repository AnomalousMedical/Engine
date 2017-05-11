using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Shim
{
    public interface StackTrace
    {
        StackFrame GetFrame(int index);

        IEnumerable<StackFrame> GetFrames();
    }
}

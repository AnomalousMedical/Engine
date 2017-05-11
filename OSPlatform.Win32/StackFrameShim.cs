using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPlatform.Win32
{
    class StackFrameShim : Engine.Shim.StackFrame
    {
        private System.Diagnostics.StackFrame frame;

        public StackFrameShim(System.Diagnostics.StackFrame frame)
        {
            this.frame = frame;
        }

        public int GetFileColumnNumber()
        {
            return frame.GetFileColumnNumber();
        }

        public int GetFileLineNumber()
        {
            return frame.GetFileLineNumber();
        }

        public string GetFileName()
        {
            return frame.GetFileName();
        }

        public string GetMethod()
        {
            return frame.GetMethod().ToString();
        }
    }
}

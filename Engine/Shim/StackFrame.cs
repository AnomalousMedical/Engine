using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Shim
{
    public interface StackFrame
    {
        string GetFileName();

        string GetMethod();

        int GetFileLineNumber();

        int GetFileColumnNumber();
    }
}

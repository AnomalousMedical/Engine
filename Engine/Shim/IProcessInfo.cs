using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Shim
{
    public interface IProcessInfo
    {
        long PrivateMemorySize64 { get; }

        long WorkingSet64 { get; }

        long VirtualMemorySize64 { get; }
    }
}

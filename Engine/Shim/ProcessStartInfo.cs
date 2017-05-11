using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Shim
{
    public interface ProcessStartInfo
    {
        String Arguments { get; set; }
        string Verb { get; set; }
        bool UseShellExecute { get; set; }
    }
}

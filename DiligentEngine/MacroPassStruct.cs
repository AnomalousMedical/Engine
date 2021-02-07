using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DiligentEngine
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct MacroPassStruct
    {
        public String name;
        public String definition;
    }
}

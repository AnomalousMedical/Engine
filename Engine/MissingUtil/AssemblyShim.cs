﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection
{
    public static class AssemblyShim
    {
        public static String CurrentAssemblyName
        {
            get
            {
#if ENABLE_LEGACY_SHIMS
                return "Unsupported";
#else
                return Assembly.GetCallingAssembly().GetName().Name;
#endif
            }
        }
    }
}

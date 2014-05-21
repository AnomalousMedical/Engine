using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class EnvironmentShim
    {
        public static String[] GetCommandLineArgs()
        {
#if ENABLE_LEGACY_SHIMS
            throw new NotImplementedException();
#else
            return Environment.GetCommandLineArgs();
#endif
        }
    }
}

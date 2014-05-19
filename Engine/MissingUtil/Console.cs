using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ENABLE_LEGACY_SHIMS
namespace System
{
    static class Console
    {
        public static void WriteLine(String line)
        {

        }
    }
}
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection
{
    public static class PathShim
    {
        public static String GetFullPath(String path)
        {
#if ENABLE_LEGACY_SHIMS
            throw new NotImplementedException();
#else
            return Path.GetFullPath(path);
#endif
        }
    }
}

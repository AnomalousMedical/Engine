using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection
{
    public static class AppDomainShim
    {
        public static IEnumerable<Assembly> GetCurrentDomainAssemblies()
        {
#if ENABLE_LEGACY_SHIMS
            throw new NotImplementedException();
#else
            return AppDomain.CurrentDomain.GetAssemblies();
#endif
        }
    }
}

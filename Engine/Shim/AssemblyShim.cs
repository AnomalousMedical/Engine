using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection
{
    public interface IAssemblyShimImpl
    {
        String CurrentAssemblyName { get; }

        IEnumerable<Assembly> LoadedAssemblies { get; }

        Assembly LoadFile(String path);
    }

    public static class AssemblyShim
    {
        private static IAssemblyShimImpl impl;

        public static void SetShimImpl(IAssemblyShimImpl impl)
        {
            AssemblyShim.impl = impl;
        }

        public static String CurrentAssemblyName
        {
            get
            {
                return impl.CurrentAssemblyName;
            }
        }

        public static IEnumerable<Assembly> LoadedAssemblies
        {
            get
            {
                return impl.LoadedAssemblies;
            }
        }

        public static Assembly LoadFile(String path)
        {
            return impl.LoadFile(path);
        }
    }
}

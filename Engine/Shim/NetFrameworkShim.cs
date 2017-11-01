using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Shim
{
    public interface INetFrameworkShim
    {
        String CurrentAssemblyName { get; }

        IEnumerable<Assembly> LoadedAssemblies { get; }

        Assembly LoadFile(String path);
        void PrintStackTrace();
        void ParallelFor(int fromInclusive, int toExclusive, Action<int> body);
    }

    public static class NetFrameworkShim
    {
        private static INetFrameworkShim impl;

        public static void SetShimImpl(INetFrameworkShim impl)
        {
            NetFrameworkShim.impl = impl;
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

        public static void PrintStackTrace()
        {
            impl.PrintStackTrace();
        }

        public static void ParallelFor(int fromInclusive, int toExclusive, Action<int> body)
        {
            impl.ParallelFor(fromInclusive, toExclusive, body);
        }
    }
}

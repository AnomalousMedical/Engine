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
        IProcessInfo IProcessInfo { get; }

        Assembly LoadFile(String path);
        void PrintStackTrace();
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

        public static IProcessInfo ProcessInfo
        {
            get
            {
                return impl.IProcessInfo;
            }
        }
    }
}

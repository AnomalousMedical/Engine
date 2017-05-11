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
        StackTrace CreateStackTrace(bool fNeedFileInfo);
        void ParallelFor(int fromInclusive, int toExclusive, Action<int> body);
        void Process_Start(ProcessStartInfo startInfo);
        Timer CreateTimer(int updateDelay);
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

        public static void Process_Start(ProcessStartInfo startInfo)
        {
            impl.Process_Start(startInfo);
        }

        public static IProcessInfo ProcessInfo
        {
            get
            {
                return impl.IProcessInfo;
            }
        }

        public static StackTrace CreateStackTrace(bool fNeedFileInfo)
        {
            return impl.CreateStackTrace(fNeedFileInfo);
        }

        public static void ParallelFor(int fromInclusive, int toExclusive, Action<int> body)
        {
            impl.ParallelFor(fromInclusive, toExclusive, body);
        }

        public static Timer CreateTimer(int updateDelay)
        {
            return impl.CreateTimer(updateDelay);
        }
    }
}

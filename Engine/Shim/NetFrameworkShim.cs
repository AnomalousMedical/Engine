using Logging;
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
    }
}

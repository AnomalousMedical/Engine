using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.Shim
{
    class FullNetFrameworkShim : INetFrameworkShim
    {
        public FullNetFrameworkShim()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(i => i.FullName == args.Name);
        }

        public string CurrentAssemblyName
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Name;
            }
        }

        public void ParallelFor(int fromInclusive, int toExclusive, Action<int> body)
        {
            Parallel.For(fromInclusive, toExclusive, body);
        }
    }
}

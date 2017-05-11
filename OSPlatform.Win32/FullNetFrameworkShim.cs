using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform.Win32
{
    public class FullNetFrameworkShim : IAssemblyShimImpl
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

        public Assembly LoadFile(string path)
        {
            return Assembly.LoadFile(path);
        }

        public IEnumerable<Assembly> LoadedAssemblies
        {
            get
            {
                return AppDomain.CurrentDomain.GetAssemblies();
            }
        }
    }
}

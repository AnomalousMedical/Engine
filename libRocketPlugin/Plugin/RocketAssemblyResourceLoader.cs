using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace libRocketPlugin
{
    public class RocketAssemblyResourceLoader : RocketFileSystemExtension
    {
        private Assembly assembly;

        public RocketAssemblyResourceLoader(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public bool canOpenFile(string file)
        {
            return assembly.GetManifestResourceInfo(file) != null;
        }

        public Stream openFile(string file)
        {
            return assembly.GetManifestResourceStream(file);
        }
    }
}

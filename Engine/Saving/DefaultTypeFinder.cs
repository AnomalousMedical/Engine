using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    class DefaultTypeFinder : TypeFinder
    {
        public Type findType(string assemblyQualifiedName)
        {
            return PluginManager.Instance.findType(assemblyQualifiedName);
        }
    }
}

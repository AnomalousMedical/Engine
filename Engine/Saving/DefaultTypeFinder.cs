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
#if !FIXLATER_DISABLED
            return PluginManager.Instance.findType(assemblyQualifiedName);
#else
            throw new NotImplementedException();
#endif
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    public interface TypeFinder
    {
        Type findType(String assemblyQualifiedName);
    }
}

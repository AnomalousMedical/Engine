using Engine.Saving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Tests
{
    class TestTypeFinder : TypeFinder
    {
        public Type findType(string assemblyQualifiedName)
        {
            return Type.GetType(assemblyQualifiedName);
        }
    }
}

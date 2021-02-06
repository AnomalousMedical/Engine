using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    static class TypeDetector
    {
        public static bool IsBool(String lookupType)
        {
            return lookupType?.Equals("bool", StringComparison.InvariantCultureIgnoreCase) == true || lookupType?.Equals("boolean", StringComparison.InvariantCultureIgnoreCase) == true;
        }
    }
}

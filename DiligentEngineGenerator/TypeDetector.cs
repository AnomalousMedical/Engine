using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    static class TypeDetector
    {
        public static bool IsBool(String lookupType)
        {
            return lookupType?.Equals("bool") == true || lookupType?.Equals("boolean") == true;
        }
    }
}

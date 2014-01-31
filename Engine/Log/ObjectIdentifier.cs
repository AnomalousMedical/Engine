using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging
{
    /// <summary>
    /// This class provides a way to identify objects, It should only be used for debugging something specific and
    /// then all code removed. This should never be used in production.
    /// </summary>
    public static class ObjectIdentifier
    {
        private static Dictionary<Object, String> ids = new Dictionary<object, String>();
        private static Dictionary<Type, int> typeCounts = new Dictionary<Type, int>();

        /// <summary>
        /// Get a unique name for a given obj, a new one if this has not been seen before or else the original one that
        /// was specified.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String GetObjectIdentifier(Object obj)
        {
            String retVal;
            if (!ids.TryGetValue(obj, out retVal))
            {
                int typeCount;
                Type type = obj.GetType();
                if (!typeCounts.TryGetValue(type, out typeCount))
                {
                    typeCount = 0;
                    typeCounts.Add(type, typeCount);
                }
                retVal = String.Format("{0} #{1}", type.Name, ++typeCount);
                ids.Add(obj, retVal);
                typeCounts[type] = typeCount;
            }
            return retVal;
        }

        /// <summary>
        /// Clear all entries in the ObjectIdentifier.
        /// </summary>
        public static void Clear()
        {
            ids.Clear();
            typeCounts.Clear();
        }
    }
}

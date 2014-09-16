using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    public class DefaultTypeFinder : TypeFinder
    {
        public Type findType(string assemblyQualifiedName)
        {
            return DefaultTypeFinder.FindType(assemblyQualifiedName);
        }

        /// <summary>
        /// Find a type the default way without needing an instance of this class.
        /// </summary>
        /// <param name="assemblyQualifiedName"></param>
        /// <returns></returns>
        public static Type FindType(string assemblyQualifiedName)
        {
#if !FIXLATER_DISABLED
            return PluginManager.Instance.findType(assemblyQualifiedName);
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// Create a name for a type that can be found with this type finder. Pretty much
        /// just strips the version info from the type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String CreateShortTypeString(Type type)
        {
            String shortAssemblyName = type.Assembly().FullName;
            return String.Format("{0}, {1}", type.FullName, shortAssemblyName.Remove(shortAssemblyName.IndexOf(',')));
        }

        public static String GetTypeNameWithoutAssembly(String typeName)
        {
            int findIndex = typeName.LastIndexOf("]]");
            String shortName;
            if (findIndex == -1)
            {
                findIndex = typeName.IndexOf(',');
                if (findIndex != -1)
                {
                    shortName = typeName.Substring(0, findIndex);
                }
                else
                {
                    shortName = typeName;
                }
            }
            else
            {
                shortName = typeName.Substring(0, findIndex + 2);
            }
            return shortName;
        }
    }
}

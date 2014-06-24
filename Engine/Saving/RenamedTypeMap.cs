using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class RenamedTypeMap
    {
        private Dictionary<String, Type> renamedTypes = new Dictionary<string, Type>();

        public void addRenamedType(String name, Type type)
        {
            renamedTypes.Add(name, type);
        }

        public bool tryGetType(String name, out Type type)
        {
            return renamedTypes.TryGetValue(name, out type);
        }

        public Type this[String typeName]
        {
            get
            {
                return renamedTypes[typeName];
            }
        }
    }
}

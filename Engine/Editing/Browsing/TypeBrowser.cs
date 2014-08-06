using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editing
{
    /// <summary>
    /// This class can quickly create a browser of types that are subclasses of a given type.
    /// </summary>
    public class TypeBrowser : Browser
    {
        static String[] delimiter = { "." };

        public TypeBrowser(String rootNodeName, String prompt, Type baseType)
            : base(rootNodeName, prompt)
        {
            foreach (Assembly assembly in AppDomainShim.GetCurrentDomainAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(baseType) && !type.IsAbstract())
                    {
                        this.addNode(type.Namespace, delimiter, new BrowserNode(type.Name, type));
                    }
                }
            }
        }
    }
}

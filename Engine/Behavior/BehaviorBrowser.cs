using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using System.Reflection;

namespace Engine
{
    class BehaviorBrowser : Browser
    {
        static String[] delimiter = { "." };

        public BehaviorBrowser()
            :base("Behaviors")
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type.IsSubclassOf(typeof(Behavior)) && !type.IsAbstract)
                    {
                        this.addNode(type.Namespace, delimiter, new BrowserNode(type.Name, type));
                    }
                }
            }
        }
    }
}

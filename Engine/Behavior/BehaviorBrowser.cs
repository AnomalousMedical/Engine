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
            : base("Behaviors", "Choose Behavior")
        {
            foreach (Assembly assembly in AppDomainShim.GetCurrentDomainAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(Behavior)) && !type.IsAbstract())
                    {
                        this.addNode(type.Namespace, delimiter, new BrowserNode(type.Name, type));
                    }
                }
            }
        }
    }
}

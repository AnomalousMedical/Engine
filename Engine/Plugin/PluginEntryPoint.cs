using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public abstract class PluginEntryPointAttribute : Attribute
    {
        public abstract void createPluginInterfaces(PluginManager pluginManager);
    }
}

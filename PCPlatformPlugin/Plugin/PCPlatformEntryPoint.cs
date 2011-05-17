using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

[assembly: PCPlatform.PCPlatformEntryPoint()]

namespace PCPlatform
{
    class PCPlatformEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new PCPlatformPlugin());
        }
    }
}

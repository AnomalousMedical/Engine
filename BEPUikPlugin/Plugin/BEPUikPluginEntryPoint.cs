using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

[assembly: BEPUikPlugin.BEPUikPluginEntryPoint()]

namespace BEPUikPlugin
{
    class BEPUikPluginEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new BEPUIkInterface());
        }
    }
}

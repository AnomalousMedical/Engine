using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

[assembly: libRocketPlugin.RocketPluginEntryPoint()]

namespace libRocketPlugin
{
    class RocketPluginEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new RocketInterface());
        }
    }
}

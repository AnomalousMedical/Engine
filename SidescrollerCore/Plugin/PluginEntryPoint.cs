using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

[assembly: Anomalous.SidescrollerCore.PluginEntryPoint()]

namespace Anomalous.SidescrollerCore
{
    class PluginEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new Plugin(pluginManager));
        }
    }
}

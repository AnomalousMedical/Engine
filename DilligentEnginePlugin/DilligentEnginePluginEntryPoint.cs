using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

[assembly: DilligentEnginePlugin.DilligentEnginePluginEntryPoint()]

namespace DilligentEnginePlugin
{
    class DilligentEnginePluginEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new DilligentEnginePluginInterface());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

[assembly: DiligentEnginePlugin.DiligentEnginePluginEntryPoint()]

namespace DiligentEnginePlugin
{
    class DiligentEnginePluginEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new DiligentEnginePluginInterface());
        }
    }
}

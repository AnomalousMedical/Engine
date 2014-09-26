using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Engine.BehaviorPluginEntryPoint()]

namespace Engine
{
    class BehaviorPluginEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new BehaviorPluginInterface());
        }
    }
}

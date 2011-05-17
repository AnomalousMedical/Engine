using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

[assembly: BulletPlugin.BulletPluginEntryPoint()]

namespace BulletPlugin
{
    class BulletPluginEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new BulletInterface());
        }
    }
}

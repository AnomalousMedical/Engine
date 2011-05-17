using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

[assembly: SoundPlugin.SoundPluginEntryPoint()]

namespace SoundPlugin
{
    class SoundPluginEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new SoundPluginInterface());
        }
    }
}

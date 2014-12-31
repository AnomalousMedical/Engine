using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

[assembly: Anomalous.OSPlatform.NativePlatformEntryPoint()]

namespace Anomalous.OSPlatform
{
    class NativePlatformEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new NativePlatformPlugin());
        }
    }
}

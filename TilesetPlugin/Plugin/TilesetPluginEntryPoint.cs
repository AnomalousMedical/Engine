using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

[assembly: Anomalous.TilesetPlugin.TilesetPluginEntryPoint()]

namespace Anomalous.TilesetPlugin
{
    class TilesetPluginEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new TilesetInterface(pluginManager));
        }
    }
}

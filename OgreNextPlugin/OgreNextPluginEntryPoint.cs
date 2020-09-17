using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

[assembly: OgreNextPlugin.OgreNextPluginEntryPoint()]

namespace OgreNextPlugin
{
    class OgreNextPluginEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new OgreInterface());
        }
    }
}

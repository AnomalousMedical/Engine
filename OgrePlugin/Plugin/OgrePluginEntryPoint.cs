using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

[assembly: OgrePlugin.OgrePluginEntryPoint()]

namespace OgrePlugin
{
    class OgrePluginEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new OgreInterface());
        }
    }
}

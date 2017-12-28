using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

[assembly: Anomalous.OgreOpenVr.OgreOpenVrEntryPoint()]

namespace Anomalous.OgreOpenVr
{
    class OgreOpenVrEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new OgreOpenVrInterface());
        }
    }
}

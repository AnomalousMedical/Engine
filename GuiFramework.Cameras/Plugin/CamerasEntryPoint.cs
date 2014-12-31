using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Anomalous.GuiFramework.Cameras.CamerasEntryPoint()]

namespace Anomalous.GuiFramework.Cameras
{
    class CamerasEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new CamerasInterface());
        }
    }
}

using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Anomalous.GuiFramework.Cameras.GuiFrameworkCamerasEntryPoint()]

namespace Anomalous.GuiFramework.Cameras
{
    class GuiFrameworkCamerasEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new GuiFrameworkCamerasInterface());
        }
    }
}

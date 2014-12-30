using Engine;
using Medical.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: libRocketWidget.RocketWidgetEntryPoint()]

namespace libRocketWidget
{
    class RocketWidgetEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new RocketWidgetInterface());
        }
    }
}

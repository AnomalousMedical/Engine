using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Anomalous.libRocketWidget.RocketWidgetEntryPoint()]

namespace Anomalous.libRocketWidget
{
    class RocketWidgetEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new RocketWidgetInterface());
        }
    }
}

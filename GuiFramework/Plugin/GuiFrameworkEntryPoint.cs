using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Anomalous.GuiFramework.GuiFrameworkEntryPoint()]

namespace Anomalous.GuiFramework
{
    class GuiFrameworkEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new GuiFrameworkInterface());
        }
    }
}

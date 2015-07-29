using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Anomalous.GuiFramework.Debugging.GuiFrameworkDebuggingEntryPoint()]

namespace Anomalous.GuiFramework.Debugging
{
    class GuiFrameworkDebuggingEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new GuiFrameworkDebuggingInterface());
        }
    }
}

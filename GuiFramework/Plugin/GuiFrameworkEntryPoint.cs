using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: GuiFramework.Plugin.GuiFrameworkEntryPoint()]

namespace GuiFramework.Plugin
{
    class GuiFrameworkEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new GuiFrameworkInterface());
        }
    }
}

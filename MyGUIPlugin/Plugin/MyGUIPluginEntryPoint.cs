using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

[assembly: MyGUIPlugin.MyGUIPluginEntryPoint()]

namespace MyGUIPlugin
{
    class MyGUIPluginEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new MyGUIInterface());
        }
    }
}

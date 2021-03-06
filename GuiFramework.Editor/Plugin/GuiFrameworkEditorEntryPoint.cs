﻿using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Anomalous.GuiFramework.Editor.GuiFrameworkEditorEntryPoint()]

namespace Anomalous.GuiFramework.Editor
{
    class GuiFrameworkEditorEntryPoint : PluginEntryPointAttribute
    {
        public override void createPluginInterfaces(PluginManager pluginManager)
        {
            pluginManager.addPlugin(new GuiFrameworkEditorInterface());
        }
    }
}

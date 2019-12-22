using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Cameras;
using Anomalous.GuiFramework.Editor;
using Anomalous.libRocketWidget;
using Anomalous.OSPlatform;
using Anomaly;
using BulletPlugin;
using Engine;
using GameAppTest;
using libRocketPlugin;
using MyGUIPlugin;
using OgrePlugin;
using SoundPlugin;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameAppTestNetCoreAnomaly
{
    class GameAppAnomaly : IAnomalyImplementation
    {
        private Startup startup;

        public GameAppAnomaly(Startup startup)
        {
            this.startup = startup;
        }

        public void AddPlugins(PluginManager pluginManager)
        {
            pluginManager.addPluginAssembly(typeof(OgreInterface).Assembly());
            pluginManager.addPluginAssembly(typeof(BulletInterface).Assembly());
            pluginManager.addPluginAssembly(typeof(NativePlatformPlugin).Assembly());
            pluginManager.addPluginAssembly(typeof(MyGUIInterface).Assembly());
            pluginManager.addPluginAssembly(typeof(RocketInterface).Assembly());
            pluginManager.addPluginAssembly(typeof(SoundPluginInterface).Assembly());
            pluginManager.addPluginAssembly(typeof(GuiFrameworkInterface).Assembly());
            pluginManager.addPluginAssembly(typeof(RocketWidgetInterface).Assembly());
            pluginManager.addPluginAssembly(typeof(GuiFrameworkCamerasInterface).Assembly());
            foreach (var assembly in startup.AdditionalPluginAssemblies)
            {
                pluginManager.addPluginAssembly(assembly);
            }
            pluginManager.addPluginAssembly(typeof(GuiFrameworkEditorInterface).Assembly);
        }
    }
}

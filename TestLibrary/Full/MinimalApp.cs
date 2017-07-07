using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Cameras;
using Anomalous.libRocketWidget;
using Anomalous.Minimus.Full.GUI;
using Anomalous.Minimus.OgreOnly;
using Anomalous.OSPlatform;
using Autofac;
using Autofac.Core;
using BEPUikPlugin;
using BulletPlugin;
using Engine;
using Engine.ObjectManagement;
using Engine.Platform;
using Engine.Renderer;
using libRocketPlugin;
using Logging;
using MyGUIPlugin;
using OgrePlugin;
using SoundPlugin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.Minimus.Full
{
    public class MinimalApp : IStartup
    {
        public string Title => "Anomalous Minimus w/ Pharos";

        public MinimalApp()
        {

        }

        public void OnInit(PharosApp pharosApp, PluginManager pluginManager)
        {
            var scope = pluginManager.GlobalScope;

            var testWindow = scope.Resolve<TestWindow>();
            testWindow.Visible = true;

            var rocketWindow = scope.Resolve<RocketWindow>();
            rocketWindow.Visible = true;
        }

        public void RegisterServices(ContainerBuilder builder)
        {
            //Individual windows, can be created more than once.
            builder.RegisterType<TestWindow>()
                .OnActivated(a =>
                {
                    a.Context.Resolve<GUIManager>().addManagedDialog(a.Instance);
                });

            builder.RegisterType<RocketWindow>()
                .OnActivated(a =>
                {
                    a.Context.Resolve<GUIManager>().addManagedDialog(a.Instance);
                });
        }
    }
}

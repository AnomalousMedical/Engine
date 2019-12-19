using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Cameras;
using Anomalous.Minimus.Full.GUI;
using Anomalous.OSPlatform;
using Autofac;
using Engine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Anomalous.Minimus.Full
{
    public class Startup : IStartup
    {
        public string Title => "Anomalous Minimus with Core App";

        public string Name => "Anomalous Minimus";

        public IEnumerable<Assembly> AdditionalPluginAssemblies => new Assembly[0];

        private SceneViewController sceneViewController;

        public Startup()
        {

        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<SceneController>();

            serviceCollection.TryAddSingleton<EngineController>();

            //Register gui services

            serviceCollection.TryAddSingleton<DocumentController>();

            serviceCollection.TryAddSingleton<TaskMenu>(
                s => new TaskMenu(s.GetRequiredService<DocumentController>(), 
                s.GetRequiredService<TaskController>(), 
                s.GetRequiredService<GUIManager>(), 
                new LayoutElementName(GUILocationNames.FullscreenPopup)));

            serviceCollection.RegisterType<TaskController>()
                .OnActivated(a =>
                {
                    var app = a.Context.Resolve<App>();
                    a.Instance.addTask(new CallbackTask("Exit", "Exit", "", "Main", (item) =>
                    {
                        app.exit();
                    }));
                })
                .SingleInstance();

            serviceCollection.RegisterType<MDILayoutManager>()
                .SingleInstance();

            serviceCollection.RegisterType<GUIManager>()
                .SingleInstance();

            serviceCollection.RegisterType<AppButtonTaskbar>()
                .SingleInstance()
                .As<Taskbar>()
                .OnActivated(a =>
                {
                    //Taskbar
                    var taskbar = a.Instance;
                    var taskMenu = a.Context.Resolve<TaskMenu>();
                    taskbar.OpenTaskMenu += (int left, int top, int width, int height) =>
                    {
                        taskMenu.setSize(width, height);
                        taskMenu.show(left, top);
                    };
                    taskbar.setAppIcon("AppButton/WideImage", "AppButton/NarrowImage");
                });

            serviceCollection.RegisterType<BorderLayoutChainLink>();

            serviceCollection.RegisterType<LayoutChain>()
                .SingleInstance()
                .OnActivated(a =>
                {
                    var mdiLayout = a.Context.Resolve<MDILayoutManager>();
                    LayoutChain layoutChain = a.Instance;
                    //layoutChain.addLink(new SingleChildChainLink(GUILocationNames.Notifications, controller.NotificationManager.LayoutContainer), true);
                    layoutChain.addLink(new SingleChildChainLink(GUILocationNames.Taskbar, a.Context.Resolve<Taskbar>()), true);
                    layoutChain.addLink(new PopupAreaChainLink(GUILocationNames.FullscreenPopup), true);
                    layoutChain.SuppressLayout = true;
                    var editorBorder = a.Context.Resolve<BorderLayoutChainLink>(new TypedParameter(typeof(String), GUILocationNames.EditorBorderLayout));
                    layoutChain.addLink(editorBorder, true);
                    layoutChain.addLink(new MDIChainLink(GUILocationNames.MDI, mdiLayout), true);
                    layoutChain.addLink(new PopupAreaChainLink(GUILocationNames.ContentAreaPopup), true);
                    var contentArea = a.Context.Resolve<BorderLayoutChainLink>(new TypedParameter(typeof(String), GUILocationNames.ContentArea));
                    layoutChain.addLink(contentArea, true);
                    layoutChain.addLink(new FinalChainLink("SceneViews", mdiLayout.DocumentArea), true);
                    layoutChain.SuppressLayout = false;
                    layoutChain.layout();
                });

            serviceCollection.RegisterType<SceneViewController>()
                .SingleInstance();

            serviceCollection.RegisterType<SceneStatsDisplayManager>()
                .SingleInstance();

            //Individual windows, can be created more than once.
            serviceCollection.RegisterType<TestWindow>()
                .OnActivated(a =>
                {
                    a.Context.Resolve<GUIManager>().addManagedDialog(a.Instance);
                });

            serviceCollection.RegisterType<RocketWindow>()
                .OnActivated(a =>
                {
                    a.Context.Resolve<GUIManager>().addManagedDialog(a.Instance);
                });
        }

        public void Initialized(CoreApp pharosApp, PluginManager pluginManager)
        {
            var scope = pluginManager.GlobalScope;

            //Build gui
            sceneViewController = scope.Resolve<SceneViewController>();
            var sceneStatsDisplayManager = scope.Resolve<SceneStatsDisplayManager>();
            sceneStatsDisplayManager.StatsVisible = true;
            sceneViewController.createWindow("Camera 1", Vector3.UnitX * 100, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 100);

            var testWindow = scope.Resolve<TestWindow>();
            testWindow.Visible = true;

            var rocketWindow = scope.Resolve<RocketWindow>();
            rocketWindow.Visible = true;
        }
    }
}

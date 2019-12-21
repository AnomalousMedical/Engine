using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Cameras;
using Anomalous.Minimus.Full.GUI;
using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using Engine.Renderer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyGUIPlugin;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<SceneController>();

            services.TryAddSingleton<EngineController>();

            //Register gui services

            services.TryAddSingleton<DocumentController>();

            services.TryAddSingleton<TaskMenu>(
                s => new TaskMenu(s.GetRequiredService<DocumentController>(), 
                s.GetRequiredService<TaskController>(), 
                s.GetRequiredService<GUIManager>(), 
                new LayoutElementName(GUILocationNames.FullscreenPopup)));

            services.AddSingleton<TaskController>(s =>
            {
                var app = s.GetRequiredService<App>();
                var tc = new TaskController();
                tc.addTask(new CallbackTask("Exit", "Exit", "", "Main", (item) =>
                {
                    app.exit();
                }));

                return tc;
            });

            services.AddSingleton<MDILayoutManager>();

            services.AddSingleton<GUIManager>(s =>
            {
                return new GUIManager(s.GetRequiredService<MDILayoutManager>(), s.GetRequiredService<OSWindow>());
            });

            services.AddSingleton<Taskbar>(s =>
            {
                //Taskbar
                var taskbar = new AppButtonTaskbar();
                var taskMenu = s.GetRequiredService<TaskMenu>();
                taskbar.OpenTaskMenu += (int left, int top, int width, int height) =>
                {
                    taskMenu.setSize(width, height);
                    taskMenu.show(left, top);
                };
                taskbar.setAppIcon("AppButton/WideImage", "AppButton/NarrowImage");

                return taskbar;
            });

            services.AddTransient<BorderLayoutChainLink<GUILocationNames.EditorBorderLayoutType>>(s => new BorderLayoutChainLink<GUILocationNames.EditorBorderLayoutType>(GUILocationNames.EditorBorderLayout));
            services.AddTransient<BorderLayoutChainLink<GUILocationNames.ContentAreaType>>(s => new BorderLayoutChainLink<GUILocationNames.ContentAreaType>(GUILocationNames.ContentArea));

            services.AddSingleton<LayoutChain>(s =>
            {
                var mdiLayout = s.GetRequiredService<MDILayoutManager>();
                LayoutChain layoutChain = new LayoutChain();
                //layoutChain.addLink(new SingleChildChainLink(GUILocationNames.Notifications, controller.NotificationManager.LayoutContainer), true);
                var taskbar = s.GetRequiredService<Taskbar>();
                layoutChain.addLink(new SingleChildChainLink(GUILocationNames.Taskbar, taskbar), true);
                layoutChain.addLink(new PopupAreaChainLink(GUILocationNames.FullscreenPopup), true);
                layoutChain.SuppressLayout = true;
                var editorBorder = s.GetRequiredService<BorderLayoutChainLink<GUILocationNames.EditorBorderLayoutType>>();
                layoutChain.addLink(editorBorder, true);
                layoutChain.addLink(new MDIChainLink(GUILocationNames.MDI, mdiLayout), true);
                layoutChain.addLink(new PopupAreaChainLink(GUILocationNames.ContentAreaPopup), true);
                var contentArea = s.GetRequiredService<BorderLayoutChainLink<GUILocationNames.ContentAreaType>>();
                layoutChain.addLink(contentArea, true);
                layoutChain.addLink(new FinalChainLink("SceneViews", mdiLayout.DocumentArea), true);
                layoutChain.SuppressLayout = false;
                layoutChain.layout();

                return layoutChain;
            });

            services.AddSingleton<SceneViewController>(s =>
            {
                var ogrePlatformProvider = s.GetRequiredService<OgrePlatformProvider>();
                return new SceneViewController(
                    s.GetRequiredService<MDILayoutManager>(),
                    s.GetRequiredService<EventManager>(),
                    s.GetRequiredService<UpdateTimer>(),
                    s.GetRequiredService<RendererWindow>(),
                    ogrePlatformProvider.OgrePlatform.RenderManager,
                    s.GetService<BackgroundScene>());
            });

            services.AddSingleton<SceneStatsDisplayManager>();

            //Individual windows, can be created more than once.
            services.AddTransient<TestWindow>(s =>
            {
                var win = new TestWindow();
                s.GetRequiredService<GUIManager>().addManagedDialog(win);
                return win;
            });

            services.AddTransient<RocketWindow>(s =>
            {
                var win = new RocketWindow();
                s.GetRequiredService<GUIManager>().addManagedDialog(win);
                return win;
            });
        }

        public void Initialized(CoreApp pharosApp, PluginManager pluginManager)
        {
            var scope = pluginManager.GlobalScope;

            //Build gui
            sceneViewController = scope.ServiceProvider.GetRequiredService<SceneViewController>();
            var sceneStatsDisplayManager = scope.ServiceProvider.GetRequiredService<SceneStatsDisplayManager>();
            sceneStatsDisplayManager.StatsVisible = true;
            sceneViewController.createWindow("Camera 1", Vector3.UnitX * 100, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 100);

            var testWindow = scope.ServiceProvider.GetRequiredService<TestWindow>();
            testWindow.Visible = true;

            var rocketWindow = scope.ServiceProvider.GetRequiredService<RocketWindow>();
            rocketWindow.Visible = true;
        }
    }
}

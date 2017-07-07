using Anomalous.GameApp;
using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Cameras;
using Anomalous.OSPlatform;
using Anomalous.SidescrollerCore;
using Autofac;
using Engine;
using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameAppTest
{
    public class Startup : IStartup
    {
        public string Title => "Game App Test";

        public string Name => "GameAppTest";

        public IEnumerable<Assembly> AdditionalPluginAssemblies
        {
            get
            {
                yield return typeof(SidescrollerCorePlugin).Assembly();
            }
        }

        private SceneViewController sceneViewController;
        private SceneViewLightManager lightManager;

        public Startup()
        {

        }

        public void ConfigureServices(ContainerBuilder builder)
        {
            //builder.RegisterType<SceneController>()
            //    .SingleInstance();

            //builder.RegisterType<EngineController>()
            //    .SingleInstance();

            //Register gui services

            builder.RegisterType<DocumentController>()
                .SingleInstance();

            builder.RegisterType<TaskMenu>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(LayoutElementName), new LayoutElementName(GUILocationNames.FullscreenPopup)));

            builder.RegisterType<TaskController>()
                .OnActivated(a =>
                {
                    var app = a.Context.Resolve<App>();
                    a.Instance.addTask(new CallbackTask("Exit", "Exit", "", "Main", (item) =>
                    {
                        app.exit();
                    }));
                })
                .SingleInstance();

            builder.RegisterType<MDILayoutManager>()
                .SingleInstance();

            builder.RegisterType<GUIManager>()
                .SingleInstance();

            builder.RegisterType<AppButtonTaskbar>()
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

            builder.RegisterType<BorderLayoutChainLink>();

            builder.RegisterType<LayoutChain>()
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

            builder.RegisterType<SceneViewController>()
                .SingleInstance();

            builder.RegisterType<SceneStatsDisplayManager>()
                .SingleInstance();

            //Individual windows, can be created more than once.
            //builder.RegisterType<TestWindow>()
            //    .OnActivated(a =>
            //    {
            //        a.Context.Resolve<GUIManager>().addManagedDialog(a.Instance);
            //    });

            //builder.RegisterType<RocketWindow>()
            //    .OnActivated(a =>
            //    {
            //        a.Context.Resolve<GUIManager>().addManagedDialog(a.Instance);
            //    });
        }

        public void Initialized(GameApp app, PluginManager pluginManager)
        {
            var scope = pluginManager.GlobalScope;

            var guiManager = scope.Resolve<GUIManager>();

            //Build gui
            sceneViewController = scope.Resolve<SceneViewController>();
            var sceneStatsDisplayManager = scope.Resolve<SceneStatsDisplayManager>();
            sceneStatsDisplayManager.StatsVisible = true;
            sceneViewController.createWindow("Camera 1", Vector3.UnitX * 100, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 100);

            lightManager = scope.Resolve<SceneViewLightManager>();

            var sceneController = scope.Resolve<SceneController>();
            sceneController.OnSceneLoaded += SceneController_OnSceneLoaded;
            sceneController.OnSceneUnloading += SceneController_OnSceneUnloading;

            //var testWindow = scope.Resolve<TestWindow>();
            //testWindow.Visible = true;

            //var rocketWindow = scope.Resolve<RocketWindow>();
            //rocketWindow.Visible = true;
        }

        private void SceneController_OnSceneLoaded(SceneController controller, SimScene scene)
        {
            sceneViewController.createCameras(scene);
            lightManager.sceneLoaded(scene);
        }

        private void SceneController_OnSceneUnloading(SceneController controller, SimScene scene)
        {
            lightManager.sceneUnloading(scene);
            sceneViewController.destroyCameras();
        }
    }
}

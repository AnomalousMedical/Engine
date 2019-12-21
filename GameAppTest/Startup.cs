using Anomalous.GameApp;
using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Cameras;
//using Anomalous.OgreOpenVr;
using Anomalous.OSPlatform;
using Anomalous.SidescrollerCore;
using Anomalous.TilesetPlugin;
using Engine;
using Engine.ObjectManagement;
using Engine.Platform;
using Engine.Platform.Input;
using Engine.Renderer;
using Microsoft.Extensions.DependencyInjection;
using MyGUIPlugin;
using OgrePlugin;
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
                //yield return typeof(OgreOpenVrInterface).Assembly();
                yield return typeof(SidescrollerCorePlugin).Assembly();
                yield return typeof(TilesetInterface).Assembly();
            }
        }

        private SceneViewController sceneViewController;
        private SceneViewLightManager lightManager;

        public Startup()
        {

        }

        public void Disposing(GameApp app, PluginManager pluginManager)
        {
            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            SidescrollerCorePlugin.RegisterControls(services, EventLayers.Game);

            services.AddSingleton<DocumentController>();

            services.AddSingleton<TaskMenu>(s =>
            {
                return new TaskMenu(s.GetRequiredService<DocumentController>(), s.GetRequiredService<TaskController>(), s.GetRequiredService<GUIManager>(), new LayoutElementName(GUILocationNames.FullscreenPopup));
            });

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
        }

        public void Initialized(GameApp app, PluginManager pluginManager)
        {
            Logging.Log.Info($"Loading archive {app.PrimaryArchivePath}");
            VirtualFileSystem.Instance.addArchive(app.PrimaryArchivePath);

            var scope = pluginManager.GlobalScope;

            scope.ServiceProvider.GetRequiredService<VirtualTextureSceneViewLink>();

            var guiManager = scope.ServiceProvider.GetRequiredService<GUIManager>();
            var layoutChain = scope.ServiceProvider.GetRequiredService<LayoutChain>();
            guiManager.createGUILayout(layoutChain);

            //Build gui
            sceneViewController = scope.ServiceProvider.GetRequiredService<SceneViewController>();
            var sceneStatsDisplayManager = scope.ServiceProvider.GetRequiredService<SceneStatsDisplayManager>();
            sceneStatsDisplayManager.StatsVisible = true;
            sceneViewController.createWindow("Camera 1", Vector3.UnitX * 100, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 100);

            lightManager = scope.ServiceProvider.GetRequiredService<SceneViewLightManager>();

            var sceneController = scope.ServiceProvider.GetRequiredService<SceneController>();
            sceneController.OnSceneLoaded += SceneController_OnSceneLoaded;
            sceneController.OnSceneUnloading += SceneController_OnSceneUnloading;
            var scene = "Scenes\\TestLevel.sim.xml";
            if (VirtualFileSystem.Instance.fileExists(scene))
            {
                Logging.Log.Info($"Loading scene {scene}");
                sceneController.loadSceneDefinition("Scenes\\TestLevel.sim.xml");
            }
            else
            {
                Logging.Log.Info($"Cannot load scene {scene}");
            }
        }

        private void SceneController_OnSceneLoaded(SceneController controller, SimScene scene)
        {
            sceneViewController.createCameras(scene);
            lightManager.sceneLoaded(scene);
            //var vrFramework = scene.Scope.Resolve<OgreFramework>();
            var subScene = scene.getDefaultSubScene();
            var sceneManager = subScene.getSimElementManager<OgreSceneManager>();
            //vrFramework.Init(OgrePlugin.Root.getSingleton(), sceneManager.SceneManager);
        }

        private void SceneController_OnSceneUnloading(SceneController controller, SimScene scene)
        {
            lightManager.sceneUnloading(scene);
            sceneViewController.destroyCameras();
        }
    }
}

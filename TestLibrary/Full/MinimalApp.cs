using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Cameras;
using Anomalous.Minimus.Full.GUI;
using Anomalous.Minimus.OgreOnly;
using Anomalous.OSPlatform;
using Autofac;
using Engine;
using Engine.ObjectManagement;
using Engine.Platform;
using Logging;
using MyGUIPlugin;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.Minimus.Full
{
    public class MinimalApp : App
    {
        private EngineController engineController;
        private CoreConfig coreConfig;
        private LogFileListener logListener;

        private BorderLayoutChainLink editorBorder;
        private BorderLayoutChainLink contentArea;
        private SceneViewController sceneViewController;
        private SimScene scene;

        private SceneStatsDisplayManager sceneStatsDisplayManager;
        private SceneViewLightManager lightManager;

        //Taskbar
        private AppButtonTaskbar taskbar;
        private SingleChildChainLink taskbarLink;
        private TaskMenu taskMenu;
        private TaskController taskController = new TaskController();
        private DocumentController documentController = new DocumentController();

        public event Action<MinimalApp> Initialized;

        ContainerBuilder builder = new ContainerBuilder();
        IContainer container;
        ILifetimeScope sceneScope;

        public MinimalApp()
        {

        }

        public override void Dispose()
        {
            CoreConfig.save();

            //Note this isn't really right and not everything is being disposed that should be.
            sceneViewController.destroyCameras();
            scene.Dispose();

            IDisposableUtil.DisposeIfNotNull(taskbar);
            IDisposableUtil.DisposeIfNotNull(taskMenu);

            IDisposableUtil.DisposeIfNotNull(sceneViewController);
            IDisposableUtil.DisposeIfNotNull(editorBorder);
            IDisposableUtil.DisposeIfNotNull(contentArea);

            sceneScope.Dispose();
            container.Dispose();

            base.Dispose();

            logListener.Dispose();
        }

        public override bool OnInit()
        {
            coreConfig = new CoreConfig("Anomalous Minimus");

            //Create the log.
            logListener = new LogFileListener();
            logListener.openLogFile(CoreConfig.LogFile);
            Log.Default.addLogListener(logListener);
            Log.ImportantInfo("Running from directory {0}", FolderFinder.ExecutableFolder);

            //Main Window
            builder.Register(c => new NativeOSWindow("Anomalous Minimus", new IntVector2(-1, -1), new IntSize2(CoreConfig.EngineConfig.HorizontalRes, CoreConfig.EngineConfig.VerticalRes)))
                .SingleInstance()
                .As<OSWindow>()
                .As<NativeOSWindow>()
                .OnActivated(a =>
                {
                    var mainWindow = a.Instance;
                    a.Instance.Closed += w =>
                    {
                        if (PlatformConfig.CloseMainWindowOnShutdown)
                        {
                            mainWindow.close();
                        }
                        this.exit();
                    };

                    //Setup DPI
                    float pixelScale = mainWindow.WindowScaling;
                    switch (CoreConfig.ExtraScaling)
                    {
                        case UIExtraScale.Smaller:
                            pixelScale -= .15f;
                            break;
                        case UIExtraScale.Larger:
                            pixelScale += .25f;
                            break;
                    }

                    ScaleHelper._setScaleFactor(pixelScale);
                });

            builder.RegisterType<EngineController>()
                .SingleInstance();

            builder.RegisterType<MDILayoutManager>()
                .SingleInstance();

            builder.RegisterType<GUIManager>()
                .SingleInstance();

            builder.RegisterType<LayoutChain>()
                .SingleInstance()
                .OnActivated(a =>
                {
                    var mdiLayout2 = a.Context.Resolve<MDILayoutManager>();
                    LayoutChain layoutChain = a.Instance;
                    //layoutChain.addLink(new SingleChildChainLink(GUILocationNames.Notifications, controller.NotificationManager.LayoutContainer), true);
                    layoutChain.addLink(new PopupAreaChainLink(GUILocationNames.FullscreenPopup), true);
                    layoutChain.SuppressLayout = true;
                    editorBorder = new BorderLayoutChainLink(GUILocationNames.EditorBorderLayout);
                    layoutChain.addLink(editorBorder, true);
                    layoutChain.addLink(new MDIChainLink(GUILocationNames.MDI, mdiLayout2), true);
                    layoutChain.addLink(new PopupAreaChainLink(GUILocationNames.ContentAreaPopup), true);
                    contentArea = new BorderLayoutChainLink(GUILocationNames.ContentArea);
                    layoutChain.addLink(contentArea, true);
                    layoutChain.addLink(new FinalChainLink("SceneViews", mdiLayout2.DocumentArea), true);
                    layoutChain.SuppressLayout = false;
                    layoutChain.layout();
                });

            builder.RegisterType<TestWindow>()
                .OnActivated(a =>
                {
                    a.Context.Resolve<GUIManager>().addManagedDialog(a.Instance);
                    a.Instance.Visible = true;
                });

            builder.RegisterType<RocketWindow>()
                .OnActivated(a =>
                {
                    a.Context.Resolve<GUIManager>().addManagedDialog(a.Instance);
                    a.Instance.Visible = true;
                });

            container = builder.Build();
            sceneScope = container.BeginLifetimeScope(LifetimeScopes.Scene);

            engineController = sceneScope.Resolve<EngineController>();

            //Layout Chain
            var mdiLayout = sceneScope.Resolve<MDILayoutManager>();
            var guiManager = sceneScope.Resolve<GUIManager>();

            //Taskbar
            taskbar = new AppButtonTaskbar();
            taskbar.OpenTaskMenu += taskbar_OpenTaskMenu;
            taskbar.setAppIcon("AppButton/WideImage", "AppButton/NarrowImage");
            taskbarLink = new SingleChildChainLink(GUILocationNames.Taskbar, taskbar);
            guiManager.addLinkToChain(taskbarLink);
            guiManager.pushRootContainer(GUILocationNames.Taskbar);

            sceneViewController = new SceneViewController(mdiLayout, engineController.EventManager, engineController.MainTimer, engineController.PluginManager.RendererPlugin.PrimaryWindow, MyGUIInterface.Instance.OgrePlatform.RenderManager, null);
            sceneStatsDisplayManager = new SceneStatsDisplayManager(sceneViewController, OgreInterface.Instance.OgrePrimaryWindow.OgreRenderTarget);
            sceneStatsDisplayManager.StatsVisible = true;
            sceneViewController.createWindow("Camera 1", Vector3.UnitX * 100, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 100);
            lightManager = PluginManager.Instance.RendererPlugin.createSceneViewLightManager();

            //Task Menu
            taskMenu = new TaskMenu(documentController, taskController, guiManager, new LayoutElementName(GUILocationNames.FullscreenPopup));

            taskController.addTask(new CallbackTask("Exit", "Exit", "", "Main", (item) =>
                {
                    this.exit();
                }));

            //Create scene
            //Create a simple scene to use to show the models
            SimSceneDefinition sceneDefiniton = new SimSceneDefinition();
            OgreSceneManagerDefinition ogreScene = new OgreSceneManagerDefinition("Ogre");
            SimSubSceneDefinition mainSubScene = new SimSubSceneDefinition("Main");
            sceneDefiniton.addSimElementManagerDefinition(ogreScene);
            sceneDefiniton.addSimSubSceneDefinition(mainSubScene);
            mainSubScene.addBinding(ogreScene);
            sceneDefiniton.DefaultSubScene = "Main";

            scene = sceneDefiniton.createScene();
            sceneViewController.createCameras(scene);

            MyGUIInterface.Instance.CommonResourceGroup.addResource(GetType().AssemblyQualifiedName, "EmbeddedScalableResource", true);

            //Create windows
            sceneScope.Resolve<TestWindow>();
            sceneScope.Resolve<RocketWindow>();

            if (Initialized != null)
            {
                Initialized.Invoke(this);
            }

            lightManager.sceneLoaded(scene);

            return true;
        }

        void taskbar_OpenTaskMenu(int left, int top, int width, int height)
        {
            taskMenu.setSize(width, height);
            taskMenu.show(left, top);
        }

        public override int OnExit()
        {
            return 0;
        }

        public override void OnIdle()
        {
            engineController.MainTimer.OnIdle();
        }

        internal void saveCrashLog()
        {
            if (logListener != null)
            {
                DateTime now = DateTime.Now;
                String crashFile = String.Format(CultureInfo.InvariantCulture, "{0}/log {1}-{2}-{3} {4}.{5}.{6}.log", CoreConfig.CrashLogDirectory, now.Month, now.Day, now.Year, now.Hour, now.Minute, now.Second);
                logListener.saveCrashLog(crashFile);
            }
        }

        public EngineController EngineController
        {
            get
            {
                return engineController;
            }
        }

        public SimScene Scene
        {
            get
            {
                return scene;
            }
        }
    }
}

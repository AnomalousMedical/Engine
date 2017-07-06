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
    public class MinimalApp : App
    {
        private EngineController engineController;
        private CoreConfig coreConfig;
        private LogFileListener logListener;

        private SimScene scene;
        private SceneViewController sceneViewController;

        private SceneViewLightManager lightManager;

        public event Action<MinimalApp> Initialized;

        ContainerBuilder builder = new ContainerBuilder();
        IContainer container;
        ILifetimeScope sceneScope;
        private UpdateTimer mainTimer;

        public MinimalApp()
        {

        }

        public override void Dispose()
        {
            CoreConfig.save();

            lightManager.sceneUnloading(scene);
            sceneViewController.destroyCameras();
            scene.Dispose();

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
            RegisterEngine();
            RegisterGui();

            //Create containers
            container = builder.Build();
            sceneScope = container.BeginLifetimeScope(LifetimeScopes.Scene);

            //Build engine
            var pluginManager = sceneScope.Resolve<PluginManager>();
            engineController = sceneScope.Resolve<EngineController>();
            mainTimer = sceneScope.Resolve<UpdateTimer>();
            var frameClearManager = sceneScope.Resolve<FrameClearManager>();

            //Build gui
            sceneViewController = sceneScope.Resolve<SceneViewController>();
            var sceneStatsDisplayManager = sceneScope.Resolve<SceneStatsDisplayManager>();
            sceneStatsDisplayManager.StatsVisible = true;
            sceneViewController.createWindow("Camera 1", Vector3.UnitX * 100, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 100);
            lightManager = sceneScope.Resolve<SceneViewLightManager>();

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

        private void RegisterEngine()
        {
            builder.Register(c => new PluginManager(CoreConfig.ConfigFile))
                .SingleInstance()
                .OnActivated(a =>
                {
                    var pluginManager = a.Instance;
                    var mainWindow = a.Context.Resolve<NativeOSWindow>();
                    var mainTimer = a.Context.Resolve<UpdateTimer>();

                    MyGUIInterface.EventLayerKey = EventLayers.Gui;
                    MyGUIInterface.CreateGuiGestures = CoreConfig.EnableMultitouch && PlatformConfig.TouchType == TouchType.Screen;

                    //Configure plugins
                    pluginManager.OnConfigureDefaultWindow = delegate (out WindowInfo defaultWindow)
                    {
                        //Setup main window
                        defaultWindow = new WindowInfo(mainWindow, "Primary");
                        defaultWindow.Fullscreen = CoreConfig.EngineConfig.Fullscreen;
                        defaultWindow.MonitorIndex = 0;

                        if (CoreConfig.EngineConfig.Fullscreen)
                        {
                            mainWindow.setSize(CoreConfig.EngineConfig.HorizontalRes, CoreConfig.EngineConfig.VerticalRes);
                            mainWindow.ExclusiveFullscreen = true;
                            defaultWindow.Width = CoreConfig.EngineConfig.HorizontalRes;
                            defaultWindow.Height = CoreConfig.EngineConfig.VerticalRes;
                        }
                        else
                        {
                            mainWindow.Maximized = true;
                        }
                        mainWindow.show();
                    };

                    GuiFrameworkCamerasInterface.MoveCameraEventLayer = EventLayers.Cameras;
                    GuiFrameworkCamerasInterface.SelectWindowEventLayer = EventLayers.AfterGui;
                    GuiFrameworkCamerasInterface.ShortcutEventLayer = EventLayers.AfterGui;

                    pluginManager.addPluginAssembly(typeof(OgreInterface).Assembly);
                    pluginManager.addPluginAssembly(typeof(BulletInterface).Assembly);
                    pluginManager.addPluginAssembly(typeof(NativePlatformPlugin).Assembly);
                    pluginManager.addPluginAssembly(typeof(MyGUIInterface).Assembly);
                    pluginManager.addPluginAssembly(typeof(RocketInterface).Assembly);
                    pluginManager.addPluginAssembly(typeof(SoundPluginInterface).Assembly);
                    pluginManager.addPluginAssembly(typeof(BEPUikInterface).Assembly);
                    pluginManager.addPluginAssembly(typeof(GuiFrameworkInterface).Assembly);
                    pluginManager.addPluginAssembly(typeof(RocketWidgetInterface).Assembly);
                    pluginManager.addPluginAssembly(typeof(GuiFrameworkCamerasInterface).Assembly);
                    pluginManager.initializePlugins();

                    //Intialize the platform
                    BulletInterface.Instance.ShapeMargin = 0.005f;

                    if (OgreConfig.VSync && CoreConfig.EngineConfig.FPSCap < 300)
                    {
                        //Use a really high framerate cap if vsync is on since it will cap our 
                        //framerate for us. If the user has requested a higher rate use it anyway.
                        mainTimer.FramerateCap = 300;
                    }
                    else
                    {
                        mainTimer.FramerateCap = CoreConfig.EngineConfig.FPSCap;
                    }

                    var eventManager = a.Context.Resolve<EventManager>();

                    pluginManager.setPlatformInfo(mainTimer, eventManager);

                    SoundConfig.initialize(CoreConfig.ConfigFile);

                    GuiFrameworkInterface.Instance.handleCursors(mainWindow);
                    SoundPluginInterface.Instance.setResourceWindow(mainWindow);

                    var touchMouseGuiForwarder = new TouchMouseGuiForwarder(eventManager, a.Context.Resolve<InputHandler>(), a.Context.Resolve<SystemTimer>(), mainWindow, EventLayers.Last);
                    touchMouseGuiForwarder.ForwardTouchesAsMouse = PlatformConfig.ForwardTouchAsMouse;
                    var myGuiKeyboard = new MyGUIOnscreenKeyboardManager(touchMouseGuiForwarder);
                    var rocketKeyboard = new RocketWidgetOnscreenKeyboardManager(touchMouseGuiForwarder);
                });

            builder.RegisterType<NativeSystemTimer>()
                .SingleInstance()
                .As<SystemTimer>();

            builder.RegisterType<NativeUpdateTimer>()
                .SingleInstance()
                .As<UpdateTimer>()
                .As<NativeUpdateTimer>();

            builder.Register(c => new NativeInputHandler(c.Resolve<NativeOSWindow>(), CoreConfig.EnableMultitouch))
                .SingleInstance()
                .As<InputHandler>();

            builder.Register(c => new EventManager(c.Resolve<InputHandler>(), Enum.GetValues(typeof(EventLayers))))
                .SingleInstance()
                .OnActivated(a =>
                {
                    a.Context.Resolve<UpdateTimer>().addUpdateListener(new EventUpdateListener(a.Instance));
                });

            builder.Register<FrameClearManager>(c => new FrameClearManager(OgreInterface.Instance.OgrePrimaryWindow.OgreRenderTarget, Color.Blue))
               .SingleInstance();

            builder.RegisterType<EngineController>()
                .SingleInstance();

            builder.Register(c => c.Resolve<PluginManager>().RendererPlugin.createSceneViewLightManager())
                .As<SceneViewLightManager>()
                .SingleInstance();

            builder.Register(c => c.Resolve<PluginManager>().RendererPlugin.PrimaryWindow)
                .As<RendererWindow>()
                .SingleInstance();

            builder.Register(c => OgreInterface.Instance.OgrePrimaryWindow.OgreRenderTarget)
                .As<RenderTarget>()
                .SingleInstance()
                .ExternallyOwned();
        }

        private void RegisterGui()
        {
            //Register gui services
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

            builder.RegisterType<DocumentController>()
                .SingleInstance();

            builder.RegisterType<TaskMenu>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(LayoutElementName), new LayoutElementName(GUILocationNames.FullscreenPopup)));

            builder.RegisterType<TaskController>()
                .OnActivated(a =>
                {
                    a.Instance.addTask(new CallbackTask("Exit", "Exit", "", "Main", (item) =>
                    {
                        this.exit();
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

            builder.Register(c => new SceneViewController(c.Resolve<MDILayoutManager>(), sceneScope.Resolve<EventManager>(), sceneScope.Resolve<UpdateTimer>(), sceneScope.Resolve<RendererWindow>(), MyGUIInterface.Instance.OgrePlatform.RenderManager, null))
                .SingleInstance();

            builder.RegisterType<SceneStatsDisplayManager>()
                .SingleInstance();

            //Individual windows, can be created more than once.
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
        }

        public override int OnExit()
        {
            return 0;
        }

        public override void OnIdle()
        {
            mainTimer.OnIdle();
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

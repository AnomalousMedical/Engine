using System;
using Anomalous.OSPlatform;
using Engine;
using Engine.Renderer;
using OgrePlugin;
using Engine.Platform;
using Engine.ObjectManagement;
using Anomalous.GuiFramework;
using MyGUIPlugin;
using Anomalous.GuiFramework.Cameras;

namespace EyeOhEss2
{
    public class GUILocationNames
    {
        public const String ContentArea = "ContentArea";

        public const String ContentAreaPopup = "ContentAreaPopup";

        public const String EditorBorderLayout = "EditorBorderLayout";

        public const String FullscreenPopup = "FullscreenPopup";

        public const String MDI = "MDI";

        public const String Notifications = "Notifications";

        public const String Taskbar = "Taskbar";
    }

    public class EyeOhEssApp : App
    {
        public enum EventLayers
        {
            Gui = 0,
            AfterGui = 1,
            Tools = 2,
            Posing = 3,
            Cameras = 4,
            Selection = 5,
        }

        private PluginManager pluginManager;
        private FrameClearManager frameClearManager;

        //Platform
        private NativeSystemTimer systemTimer;
        private NativeUpdateTimer mainTimer;
        private EventManager eventManager;
        private NativeInputHandler inputHandler;
        private EventUpdateListener eventUpdate;
        private MDILayoutManager mdiLayout;
        private GUIManager guiManager;
        private SceneViewController sceneViewController;
        private SceneStatsDisplayManager sceneStatsDisplayManager;

        private NativeOSWindow mainWindow;

        public EyeOhEssApp()
        {

        }

        public override bool OnInit()
        {
            Logging.Log.Default.addLogListener(new Logging.LogConsoleListener());

            pluginManager = new PluginManager(new ConfigFile("Woot.txt"));

            //Configure plugins
            pluginManager.OnConfigureDefaultWindow = delegate(out WindowInfo defaultWindow)
            {
                //Setup main window
                defaultWindow = new WindowInfo(mainWindow, "Primary");
                defaultWindow.Fullscreen = false;
                defaultWindow.MonitorIndex = 0;

                mainWindow.Maximized = true;
                mainWindow.show();
            };

            MyGUIInterface.EventLayerKey = EventLayers.Gui;
            GuiFrameworkCamerasInterface.MoveCameraEventLayer = EventLayers.Cameras;
            GuiFrameworkCamerasInterface.SelectWindowEventLayer = EventLayers.AfterGui;

            mainWindow = new NativeOSWindow("Test App", new IntVector2(0, 0), new IntSize2(800, 600));
            mainWindow.Closed += mainWindow_Closed;

            pluginManager.addPluginAssembly(typeof(OgreInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(MyGUIInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(GuiFrameworkInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(GuiFrameworkCamerasInterface).Assembly);
            pluginManager.initializePlugins();

            frameClearManager = new FrameClearManager(OgreInterface.Instance.OgrePrimaryWindow.OgreRenderTarget, Color.Blue);

            systemTimer = new NativeSystemTimer();
            mainTimer = new NativeUpdateTimer(systemTimer);
            inputHandler = new NativeInputHandler(mainWindow, false);
            eventManager = new EventManager(inputHandler, Enum.GetValues(typeof(EventLayers)));
            eventUpdate = new EventUpdateListener(eventManager);
            mainTimer.addUpdateListener(eventUpdate);
            pluginManager.setPlatformInfo(mainTimer, eventManager);

            //OgreInterface.Instance.OgrePrimaryWindow.createSceneView(scene.getDefaultSubScene(), "Test", new Vector3(0, 0, -100), new Vector3(0, 0, 0));

            //Layout Chain
            mdiLayout = new MDILayoutManager();

            sceneViewController = new SceneViewController(mdiLayout, eventManager, mainTimer, pluginManager.RendererPlugin.PrimaryWindow, MyGUIInterface.Instance.OgrePlatform.getRenderManager(), null);
            sceneStatsDisplayManager = new SceneStatsDisplayManager(sceneViewController, OgreInterface.Instance.OgrePrimaryWindow.OgreRenderTarget);
            sceneStatsDisplayManager.StatsVisible = true;
            sceneViewController.createWindow("Camera 1", new Vector3(0, 0, -250), Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 100);

            //Layout
            LayoutChain layoutChain = new LayoutChain();
            //layoutChain.addLink(new SingleChildChainLink(GUILocationNames.Taskbar, mainForm.LayoutContainer), true);
            //layoutChain.addLink(new PopupAreaChainLink(GUILocationNames.FullscreenPopup), true);
            //layoutChain.SuppressLayout = true;
            layoutChain.addLink(new MDIChainLink(GUILocationNames.MDI, mdiLayout), true);
            //layoutChain.addLink(new PopupAreaChainLink(GUILocationNames.ContentAreaPopup), true);
            //layoutChain.addLink(new FinalChainLink("SceneViews", mdiLayout.DocumentArea), true);
            //layoutChain.SuppressLayout = false;

            guiManager = new GUIManager();
            guiManager.createGUI(mdiLayout, layoutChain, mainWindow);

            //Create a simple scene to use to show the models
            SimSceneDefinition sceneDefiniton = new SimSceneDefinition();
            OgreSceneManagerDefinition ogreScene = new OgreSceneManagerDefinition("Ogre");
            SimSubSceneDefinition mainSubScene = new SimSubSceneDefinition("Main");
            sceneDefiniton.addSimElementManagerDefinition(ogreScene);
            sceneDefiniton.addSimSubSceneDefinition(mainSubScene);
            mainSubScene.addBinding(ogreScene);
            sceneDefiniton.DefaultSubScene = "Main";

            var scene = sceneDefiniton.createScene();

            sceneViewController.createCameras(scene);

            layoutChain.layout();

            MessageBox.show("A very simple ios app works", "Woot", MessageBoxStyle.Ok | MessageBoxStyle.IconInfo);

            return true;
        }

        void mainWindow_Closed(OSWindow window)
        {
            this.exit();
        }

        public override int OnExit()
        {
            if (guiManager != null)
            {
                guiManager.Dispose();
            }
            if (sceneViewController != null)
            {
                sceneViewController.Dispose();
            }
            if (mdiLayout != null)
            {
                mdiLayout.Dispose();
            }
            if (frameClearManager != null)
            {
                frameClearManager.Dispose();
            }
            if (eventManager != null)
            {
                eventManager.Dispose();
            }
            if (inputHandler != null)
            {
                inputHandler.Dispose();
            }
            if (systemTimer != null)
            {
                systemTimer.Dispose();
            }
            if (pluginManager != null)
            {
                pluginManager.Dispose();
            }
            return 0;
        }

        public override void OnIdle()
        {
            mainTimer.OnIdle();
        }
    }
}


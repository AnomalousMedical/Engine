using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Cameras;
using Anomalous.Minimus.Full.GUI;
using Anomalous.OSPlatform;
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
        private NativeOSWindow mainWindow;
        private CoreConfig coreConfig;
        private LogFileListener logListener;

        private BorderLayoutChainLink editorBorder;
        private BorderLayoutChainLink contentArea;
        private GUIManager guiManager;
        private MDILayoutManager mdiLayout;
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

        //Windows
        private TestWindow testWindow;

        public event Action<MinimalApp> Initialized;

        public MinimalApp()
        {

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
            mainWindow = new NativeOSWindow("Anomalous Minimus", new IntVector2(-1, -1), new IntSize2(CoreConfig.EngineConfig.HorizontalRes, CoreConfig.EngineConfig.VerticalRes));
            mainWindow.Closed += mainWindow_Closed;
            mainWindow.CreateInternalResources += mainWindow_CreateInternalResources;
            mainWindow.DestroyInternalResources += mainWindow_DestroyInternalResources;

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

            engineController = new EngineController(mainWindow);

            //Layout Chain
            mdiLayout = new MDILayoutManager();

            LayoutChain layoutChain = new LayoutChain();
            //layoutChain.addLink(new SingleChildChainLink(GUILocationNames.Notifications, controller.NotificationManager.LayoutContainer), true);
            layoutChain.addLink(new PopupAreaChainLink(GUILocationNames.FullscreenPopup), true);
            layoutChain.SuppressLayout = true;
            editorBorder = new BorderLayoutChainLink(GUILocationNames.EditorBorderLayout, engineController.MainTimer);
            layoutChain.addLink(editorBorder, true);
            layoutChain.addLink(new MDIChainLink(GUILocationNames.MDI, mdiLayout), true);
            layoutChain.addLink(new PopupAreaChainLink(GUILocationNames.ContentAreaPopup), true);
            contentArea = new BorderLayoutChainLink(GUILocationNames.ContentArea, engineController.MainTimer);
            layoutChain.addLink(contentArea, true);
            layoutChain.addLink(new FinalChainLink("SceneViews", mdiLayout.DocumentArea), true);
            layoutChain.SuppressLayout = false;
            layoutChain.layout();

            guiManager = new GUIManager();
            guiManager.createGUI(mdiLayout, layoutChain, mainWindow);

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

            TestWindow testWindow = new TestWindow();
            guiManager.addManagedDialog(testWindow);
            testWindow.Visible = true;

            RocketWindow rocketWindow = new RocketWindow();
            guiManager.addManagedDialog(rocketWindow);
            rocketWindow.Visible = true;

            if(Initialized != null)
            {
                Initialized.Invoke(this);
            }

            lightManager.sceneLoaded(scene);

            return true;
        }

        void mainWindow_DestroyInternalResources(OSWindow window, InternalResourceType resourceType)
        {
            //libRocketPlugin.TextureDatabase.ReleaseTextures();
        }

        void mainWindow_CreateInternalResources(OSWindow window, InternalResourceType resourceType)
        {
            
        }

        void taskbar_OpenTaskMenu(int left, int top, int width, int height)
        {
            taskMenu.setSize(width, height);
            taskMenu.show(left, top);
        }

        public override int OnExit()
        {
            CoreConfig.save();

            //Note this isn't really right and not everything is being disposed that should be.
            sceneViewController.destroyCameras();
            scene.Dispose();

            IDisposableUtil.DisposeIfNotNull(testWindow);
            IDisposableUtil.DisposeIfNotNull(taskbar);
            IDisposableUtil.DisposeIfNotNull(taskMenu);

            IDisposableUtil.DisposeIfNotNull(sceneViewController);
            IDisposableUtil.DisposeIfNotNull(editorBorder);
            IDisposableUtil.DisposeIfNotNull(contentArea);
            IDisposableUtil.DisposeIfNotNull(mdiLayout);

            engineController.Dispose();
            mainWindow.Dispose();

            base.Dispose();

            logListener.closeLogFile();

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

        void mainWindow_Closed(OSWindow window)
        {
            if (PlatformConfig.CloseMainWindowOnShutdown)
            {
                mainWindow.close();
            }
            this.exit();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using Engine.Platform;
using Engine;
using OgrePlugin;
using Engine.Renderer;
using Engine.ObjectManagement;
using Engine.Resources;
using System.IO;
using Engine.Saving.XMLSaver;
using System.Xml;
using OgreModelEditor.Controller;
using System.Reflection;
using Anomalous.OSPlatform;
using Anomalous.GuiFramework;
using MyGUIPlugin;
using Anomalous.GuiFramework.Cameras;
using Anomalous.GuiFramework.Editor;
using Anomalous.GuiFramework.Debugging;

namespace OgreModelEditor
{
    class OgreModelEditorController : IDisposable
    {
        private const float DefaultOrbitDistance = 150.0f;

        //Engine
        private PluginManager pluginManager;
        private LogFileListener logListener;

        //Platform
        private NativeUpdateTimer mainTimer;
        private NativeSystemTimer systemTimer;
        private EventManager eventManager;
        private NativeInputHandler inputHandler;
        private EventUpdateListener eventUpdate;
        private App app;
        private IdleHandler idleHandler;
        private UpdateTimerEventListener updateEventListener = new UpdateTimerEventListener();
        private VirtualTextureSceneViewLink virtualTextureLink;

        //GUI
        private OgreModelEditorMain mainForm;
        private NativeOSWindow mainWindow;
        private MDIObjectEditor resourceEditor;
        private LogWindow consoleWindow;
        private MDILayoutManager mdiLayout;
        private GUIManager guiManager;
        private SplashScreen splashScreen;
        private VirtualTextureDebugger virtualTextureDebugger;

        //Controller
        private SceneViewController sceneViewController;
        private ModelController modelController;
        private SceneViewLightManager lightManager;
        private FrameClearManager frameClearManager;
        private SceneStatsDisplayManager sceneStatsDisplayManager;

        //Scene
        private SimScene scene;
        private String lastFileName = null;

        //Resources
        private Engine.Resources.ResourceManager resourceManager;
        private XmlSaver xmlSaver = new XmlSaver();
        private Engine.Resources.ResourceManager emptyResourceManager;
        private Engine.Resources.ResourceManager liveResourceManager;

        //Tools
        private SimObjectMover objectMover;

        public OgreModelEditorController(App app, String defaultModel)
        {
            this.app = app;

            //Create the log.
            logListener = new LogFileListener();
            logListener.openLogFile(OgreModelEditorConfig.DocRoot + "/log.log");
            Log.Default.addLogListener(logListener);

            //Main window
            mainWindow = new NativeOSWindow("Ogre Model Editor", new IntVector2(-1, -1), new IntSize2(OgreModelEditorConfig.EngineConfig.HorizontalRes, OgreModelEditorConfig.EngineConfig.VerticalRes));
            mainWindow.Closed += mainWindow_Closed;

            //Setup DPI
            ScaleHelper._setScaleFactor(mainWindow.WindowScaling);

            //Initailize plugins
            MyGUIInterface.EventLayerKey = EventLayers.Main;

            GuiFrameworkCamerasInterface.MoveCameraEventLayer = EventLayers.Cameras;
            GuiFrameworkCamerasInterface.SelectWindowEventLayer = EventLayers.AfterMain;
            GuiFrameworkCamerasInterface.ShortcutEventLayer = EventLayers.Main;

            GuiFrameworkEditorInterface.ToolsEventLayers = EventLayers.Tools;

            //Setup microcode cache load
            OgreInterface.MicrocodeCachePath = Path.Combine(OgreModelEditorConfig.DocRoot, "ShaderCache.mcc");
            OgreInterface.AllowMicrocodeCacheLoad = OgreModelEditorConfig.LastShaderVersion == UnifiedMaterialBuilder.Version;
            OgreModelEditorConfig.LastShaderVersion = UnifiedMaterialBuilder.Version;
            OgreInterface.TrackMemoryLeaks = true;

            NativePlatformPlugin.addPath(OgreModelEditorConfig.OpenGLESEmulatorPath);

            pluginManager = new PluginManager(OgreModelEditorConfig.ConfigFile);
            pluginManager.OnConfigureDefaultWindow = createWindow;
            pluginManager.addPluginAssembly(typeof(OgreInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(NativePlatformPlugin).Assembly);
            pluginManager.addPluginAssembly(typeof(MyGUIInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(GuiFrameworkInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(GuiFrameworkCamerasInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(GuiFrameworkEditorInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(GuiFrameworkDebuggingInterface).Assembly);
            pluginManager.initializePlugins();
            frameClearManager = new FrameClearManager(OgreInterface.Instance.OgrePrimaryWindow.OgreRenderTarget, new Color(0.2f, 0.2f, 0.2f));

            lightManager = pluginManager.RendererPlugin.createSceneViewLightManager();

            //Core resources
            MyGUIInterface.Instance.CommonResourceGroup.addResource(this.GetType().AssemblyQualifiedName, "EmbeddedResource", true);
            OgreResourceGroupManager.getInstance().addResourceLocation(GetType().AssemblyQualifiedName, "EmbeddedResource", "DebugShaders", true);
            OgreResourceGroupManager.getInstance().initializeAllResourceGroups();

            //Intialize the platform
            systemTimer = new NativeSystemTimer();

            mainTimer = new NativeUpdateTimer(systemTimer);
            mainTimer.FramerateCap = OgreModelEditorConfig.EngineConfig.MaxFPS;
            idleHandler = new IdleHandler(mainTimer.OnIdle);

            inputHandler = new NativeInputHandler(mainWindow, false);
            eventManager = new EventManager(inputHandler, Enum.GetValues(typeof(EventLayers)));
            eventUpdate = new EventUpdateListener(eventManager);
            mainTimer.addUpdateListener(eventUpdate);
            mainTimer.addUpdateListener(updateEventListener);
            pluginManager.setPlatformInfo(mainTimer, eventManager);
            GuiFrameworkInterface.Instance.handleCursors(mainWindow);

            //Layout Chain
            mdiLayout = new MDILayoutManager();

            //Scene views
            sceneViewController = new SceneViewController(mdiLayout, eventManager, mainTimer, pluginManager.RendererPlugin.PrimaryWindow, MyGUIInterface.Instance.OgrePlatform.RenderManager, null);
            sceneStatsDisplayManager = new SceneStatsDisplayManager(sceneViewController, OgreInterface.Instance.OgrePrimaryWindow.OgreRenderTarget);
            sceneStatsDisplayManager.StatsVisible = true;
            sceneViewController.createWindow("Camera 1", OgreModelEditorConfig.CameraConfig.MainCameraPosition, OgreModelEditorConfig.CameraConfig.MainCameraLookAt, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 100);

            virtualTextureLink = new VirtualTextureSceneViewLink(this);

            //Tools
            objectMover = new SimObjectMover("ModelMover", PluginManager.Instance.RendererPlugin, eventManager, sceneViewController);

            mainForm = new OgreModelEditorMain(this);

            LayoutChain layoutChain = new LayoutChain();
            layoutChain.addLink(new SingleChildChainLink(GUILocationNames.Taskbar, mainForm.LayoutContainer), true);
            layoutChain.addLink(new PopupAreaChainLink(GUILocationNames.FullscreenPopup), true);
            layoutChain.SuppressLayout = true;
            layoutChain.addLink(new MDIChainLink(GUILocationNames.MDI, mdiLayout), true);
            layoutChain.addLink(new PopupAreaChainLink(GUILocationNames.ContentAreaPopup), true);
            layoutChain.addLink(new FinalChainLink("SceneViews", mdiLayout.DocumentArea), true);
            layoutChain.SuppressLayout = false;

            guiManager = new GUIManager();
            guiManager.createGUI(mdiLayout, layoutChain, mainWindow);

            layoutChain.layout();

            splashScreen = new SplashScreen(mainWindow, 100, "OgreModelEditor.GUI.SplashScreen.SplashScreen.layout", "OgreModelEditor.GUI.SplashScreen.SplashScreen.xml");
            splashScreen.Hidden += splashScreen_Hidden;
            splashScreen.StatusUpdated += splashScreen_StatusUpdated;
            splashScreen.updateStatus(0, "Loading...");

            idleHandler.runTemporaryIdle(finishInitialization(defaultModel));
        }

        public void Dispose()
        {
            liveResourceManager.changeResourcesToMatch(emptyResourceManager);
            liveResourceManager.initializeResources();

            var activeWindow = sceneViewController.ActiveWindow;
            if(activeWindow != null)
            {
                OgreModelEditorConfig.CameraConfig.MainCameraPosition = activeWindow.Translation;
                OgreModelEditorConfig.CameraConfig.MainCameraLookAt = activeWindow.LookAt;
            }

            IDisposableUtil.DisposeIfNotNull(virtualTextureDebugger);

            if(consoleWindow != null)
            {
                Log.Default.removeLogListener(consoleWindow);
                consoleWindow.Dispose();
            }
            if(splashScreen != null)
            {
                splashScreen.Dispose();
            }
            if(resourceEditor != null)
            {
                resourceEditor.Dispose();
            }
            if(objectMover != null)
            {
                objectMover.Dispose();
            }
            if(guiManager != null)
            {
                guiManager.Dispose();
            }
            if(sceneViewController != null)
            {
                sceneViewController.Dispose();
            }
            if(mdiLayout != null)
            {
                mdiLayout.Dispose();
            }
            if(virtualTextureLink != null)
            {
                virtualTextureLink.Dispose();
            }
            if (modelController != null)
            {
                modelController.Dispose();
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
            if(lightManager != null)
            {
                pluginManager.RendererPlugin.destroySceneViewLightManager(lightManager);
            }
            if(frameClearManager != null)
            {
                frameClearManager.Dispose();
            }
            if (mainWindow != null)
            {
                mainWindow.Dispose();
            }
            if (pluginManager != null)
            {
                pluginManager.Dispose();
            }

            OgreModelEditorConfig.save();
            logListener.closeLogFile();
        }

        private IEnumerable<IdleStatus> finishInitialization(String defaultModel)
        {
            yield return IdleStatus.Ok;

            splashScreen.updateStatus(15, "Loading Resources");

            VirtualFileSystem.Instance.addArchive(OgreModelEditorConfig.VFSRoot);

            emptyResourceManager = pluginManager.createScratchResourceManager();
            liveResourceManager = pluginManager.createLiveResourceManager("Scene");
            if (!File.Exists(OgreModelEditorConfig.DocRoot + "/resources.xml"))
            {
                resourceManager = pluginManager.createScratchResourceManager();
            }
            else
            {
                XmlTextReader textReader = new XmlTextReader(OgreModelEditorConfig.DocRoot + "/resources.xml");
                resourceManager = xmlSaver.restoreObject(textReader) as Engine.Resources.ResourceManager;
                if (resourceManager == null)
                {
                    resourceManager = pluginManager.createScratchResourceManager();
                }
                liveResourceManager.changeResourcesToMatch(resourceManager);
                liveResourceManager.initializeResources();
                textReader.Close();
            }

            yield return IdleStatus.Ok;

            splashScreen.updateStatus(40, "Creating GUI");

            //Initialize controllers
            modelController = new ModelController(this);

            //Create the GUI
            resourceEditor = new MDIObjectEditor("Resource Editor", "OgreModelEditor.ResourceEditor");
            guiManager.addManagedDialog(resourceEditor);
            resourceEditor.Closed += resourceEditor_Closed;

            consoleWindow = new LogWindow();
            guiManager.addManagedDialog(consoleWindow);
            consoleWindow.Visible = true;
            Log.Default.addLogListener(consoleWindow);

            virtualTextureDebugger = new VirtualTextureDebugger(virtualTextureLink.VirtualTextureManager);
            guiManager.addManagedDialog(virtualTextureDebugger);

            yield return IdleStatus.Ok;

            splashScreen.updateStatus(70, "Creating Scene");

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
            lightManager.sceneLoaded(scene);
            objectMover.sceneLoaded(scene);
            virtualTextureLink.sceneLoaded(scene);

            yield return IdleStatus.Ok;

            if (!String.IsNullOrEmpty(defaultModel))
            {
                splashScreen.updateStatus(80, "Loading Model");

                openModel(defaultModel);

                yield return IdleStatus.Ok;
            }

            splashScreen.updateStatus(100, "Loaded");

            splashScreen.hide();

            yield return IdleStatus.Ok;
        }

        public void idle()
        {
            idleHandler.onIdle();
        }

        public void shutdown()
        {
            sceneViewController.destroyCameras();
            virtualTextureLink.sceneUnloading(scene);
            lightManager.sceneUnloading(scene);
            objectMover.sceneUnloading(scene);
            modelController.destroyModel();
            scene.Dispose();
        }

        public void openModel(String path)
        {
            var ogreResources = resourceManager.getSubsystemResource("Ogre");
            if (modelController.modelActive())
            {
                modelController.destroyModel();
                String lastDir = Path.GetDirectoryName(lastFileName);
                String lastParentDir = Path.GetDirectoryName(lastDir);
                String lastInnerDir = Path.GetFileName(lastDir);
                ogreResources.removeResourceGroup(lastInnerDir);
                liveResourceManager.changeResourcesToMatch(resourceManager);
                liveResourceManager.initializeResources();
                VirtualFileSystem.Instance.removeArchive(lastParentDir);
            }

            lastFileName = path;
            String dir = Path.GetDirectoryName(path);
            String parentDir = Path.GetDirectoryName(dir);
            String innerDir = Path.GetFileName(dir);
            VirtualFileSystem.Instance.addArchive(parentDir);
            var group = ogreResources.addResourceGroup(innerDir);
            group.addResource(innerDir, "EngineArchive", true);
            liveResourceManager.changeResourcesToMatch(resourceManager);
            liveResourceManager.initializeResources();
            String meshName = Path.GetFileName(path);
            modelController.createModel(meshName, scene);
            mainForm.setTextureNames(modelController.TextureNames);
            mainForm.currentFileChanged(path);
        }

        public void editExternalResources()
        {
            resourceEditor.EditInterface = resourceManager.getEditInterface();
            resourceEditor.Visible = true;
        }

        void resourceEditor_Closed(object sender, EventArgs e)
        {
            liveResourceManager.changeResourcesToMatch(resourceManager);
            liveResourceManager.initializeResources();
            using (XmlTextWriter textWriter = new XmlTextWriter(OgreModelEditorConfig.DocRoot + "/resources.xml", Encoding.Default))
            {
                textWriter.Formatting = Formatting.Indented;
                xmlSaver.saveObject(resourceManager, textWriter);
                textWriter.Close();
            }
        }

        public void refreshResources()
        {
            if (modelController.modelActive())
            {
                modelController.destroyModel();
                String dir = Path.GetDirectoryName(lastFileName);
                liveResourceManager.changeResourcesToMatch(emptyResourceManager);
                liveResourceManager.initializeResources();
                virtualTextureLink.clearCache();
                liveResourceManager.changeResourcesToMatch(resourceManager);
                liveResourceManager.initializeResources();
                String meshName = Path.GetFileName(lastFileName);
                modelController.createModel(meshName, scene);
                mainForm.setTextureNames(modelController.TextureNames);
            }
        }

        public void setBinormalDebug()
        {
            modelController.setBinormalDebug();
        }

        public void setTangentDebug()
        {
            modelController.setTangentDebug();
        }

        public void setNormalDebug()
        {
            modelController.setNormalDebug();
        }

        public void setParityDebug()
        {
            modelController.setParityDebug();
        }

        public void setNormalMaterial()
        {
            modelController.setNormalMaterial();
        }

        public void setTextureDebug(String textureName)
        {
            modelController.showIndividualTexture(textureName);
        }

        public void buildTangentVectors()
        {
            modelController.buildTangentVectors();
        }

        public void buildBinormalVectors()
        {
            modelController.buildBinormalVectors();
        }

        public void saveModel(String filename)
        {
            modelController.saveModel(filename);
        }

        internal void saveModelJSON(string filename)
        {
            modelController.saveModelJSON(filename);
        }

        public void createOneWindow()
        {
            sceneViewController.closeAllWindows();
            sceneViewController.createWindow("Camera 1", Vector3.Backward * DefaultOrbitDistance, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 100);
        }

        public void createTwoWindows()
        {
            sceneViewController.closeAllWindows();
            var cameraOne = sceneViewController.createWindow("Camera 1", Vector3.Backward * DefaultOrbitDistance, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 100);
            sceneViewController.createWindow("Camera 2", Vector3.Forward * DefaultOrbitDistance, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 150, cameraOne, WindowAlignment.Right);
        }

        public void createThreeWindows()
        {
            sceneViewController.closeAllWindows();
            var cameraOne = sceneViewController.createWindow("Camera 1", Vector3.Right * DefaultOrbitDistance, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 100);
            var cameraTwo = sceneViewController.createWindow("Camera 2", Vector3.Backward * DefaultOrbitDistance, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 150, cameraOne, WindowAlignment.Right);
            var cameraThree = sceneViewController.createWindow("Camera 3", Vector3.Left * DefaultOrbitDistance, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 200, cameraTwo, WindowAlignment.Right);
        }

        public void createFourWindows()
        {
            sceneViewController.closeAllWindows();
            var cameraOne = sceneViewController.createWindow("Camera 1", Vector3.Backward * DefaultOrbitDistance, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 100);
            var cameraTwo = sceneViewController.createWindow("Camera 2", Vector3.Forward * DefaultOrbitDistance, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 150, cameraOne, WindowAlignment.Right);
            var cameraThree = sceneViewController.createWindow("Camera 3", Vector3.Left * DefaultOrbitDistance, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 200, cameraOne, WindowAlignment.Bottom);
            var cameraFour = sceneViewController.createWindow("Camera 4", Vector3.Right * DefaultOrbitDistance, Vector3.Zero, Vector3.Min, Vector3.Max, 0.0f, float.MaxValue, 250, cameraTwo, WindowAlignment.Bottom);
        }

        public void enableMoveTool()
        {
            objectMover.ShowMoveTools = true;
            objectMover.ShowRotateTools = false;
            objectMover.Visible = true;
        }

        public void enableRotateTool()
        {
            objectMover.ShowMoveTools = false;
            objectMover.ShowRotateTools = true;
            objectMover.Visible = true;
        }

        public void enableSelectTool()
        {
            objectMover.Visible = false;
        }

        public void setShowSkeleton(bool show)
        {
            modelController.ShowSkeleton = show;
        }

        public void showVirtualTextureDebugger()
        {
            virtualTextureDebugger.Visible = true;
        }

        public void updateWindowTitle(String file)
        {
            if(String.IsNullOrEmpty(file))
            {
                mainWindow.Title = "Ogre Model Editor";
            }
            else
            {
                mainWindow.Title = String.Format("{0} - Ogre Model Editor", file);
            }
        }

        public void batchResaveMeshes(String rootPath)
        {
            Log.ImportantInfo("Upgrading meshes in {0}", rootPath);
            String[] meshFiles = Directory.GetFiles(rootPath, "*.mesh", SearchOption.AllDirectories);
            foreach (String meshFile in meshFiles)
            {
                Log.ImportantInfo("Upgrading mesh {0}", meshFile);
                openModel(meshFile);
                saveModel(meshFile);
            }
        }

        public void exit()
        {
            app.exit();
        }

        public void clearMicrocodeCache()
        {
            OgreInterface.Instance.deleteMicrocodeCache();
            MessageBox.show("Erased microcode cache.", "Microcode Erased", MessageBoxStyle.IconInfo | MessageBoxStyle.Ok);
        }

        /// <summary>
        /// Helper function to create the default window. This is the callback
        /// to the PluginManager.
        /// </summary>
        /// <param name="defaultWindow"></param>
        private void createWindow(out WindowInfo defaultWindow)
        {
            //Setup main window
            defaultWindow = new WindowInfo(mainWindow, "Primary");
            defaultWindow.Fullscreen = OgreModelEditorConfig.EngineConfig.Fullscreen;
            defaultWindow.MonitorIndex = 0;

            if (OgreModelEditorConfig.EngineConfig.Fullscreen)
            {
                mainWindow.setSize(OgreModelEditorConfig.EngineConfig.HorizontalRes, OgreModelEditorConfig.EngineConfig.VerticalRes);
                mainWindow.ExclusiveFullscreen = true;
            }
            else
            {
                mainWindow.Maximized = true;
            }
            mainWindow.show();
        }

        public SimObjectMover ObjectMover
        {
            get
            {
                return objectMover;
            }
        }

        public NativeUpdateTimer MainTimer
        {
            get
            {
                return mainTimer;
            }
        }

        public bool ShowStats
        {
            get
            {
                return sceneStatsDisplayManager.StatsVisible;
            }
            set
            {
                sceneStatsDisplayManager.StatsVisible = value;
            }
        }

        public GUIManager GuiManager
        {
            get
            {
                return guiManager;
            }
        }

        public NativeOSWindow MainWindow
        {
            get
            {
                return mainWindow;
            }
        }

        public FrameClearManager FrameClear
        {
            get
            {
                return frameClearManager;
            }
        }

        public SceneViewController SceneViewController
        {
            get
            {
                return sceneViewController;
            }
        }

        public PluginManager PluginManager
        {
            get
            {
                return pluginManager;
            }
        }

        public event Action<Clock> OnUpdate
        {
            add
            {
                updateEventListener.OnUpdate += value;
            }
            remove
            {
                updateEventListener.OnUpdate -= value;
            }
        }

        void mainWindow_Closed(OSWindow window)
        {
            exit();
        }

        void splashScreen_StatusUpdated(SplashScreen obj)
        {
            OgreInterface.Instance.OgrePrimaryWindow.OgreRenderTarget.update();
        }

        void splashScreen_Hidden(SplashScreen obj)
        {
            splashScreen.Dispose();
            splashScreen = null;
        }
    }
}

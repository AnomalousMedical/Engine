using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using Engine.Platform;
using Engine;
using Editor;
using OgrePlugin;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
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

namespace OgreModelEditor
{
    class OgreModelEditorController : IDisposable
    {
        private const float DefaultOrbitDistance = 200.0f;

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

        //GUI
        private OgreModelEditorMain mainForm;
        private NativeOSWindow mainWindow;
        private ObjectEditorForm objectEditor = new ObjectEditorForm();
        private ConsoleWindow consoleWindow = new ConsoleWindow();
        private MDILayoutManager mdiLayout;
        private GUIManager guiManager;

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
        private ToolInteropController toolInterop = new ToolInteropController();
        private MoveController moveController = new MoveController();
        private SelectionController selectionController = new SelectionController();
        private RotateController rotateController = new RotateController();
        private MovementTool movementTool;
        private RotateTool rotateTool;
        private ToolManager toolManager;

        public void Dispose()
        {
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

        public void initialize(App app)
        {
            this.app = app;

            //Create the log.
            logListener = new LogFileListener();
            logListener.openLogFile(OgreModelEditorConfig.DocRoot + "/log.log");
            Log.Default.addLogListener(logListener);
            Log.Default.addLogListener(consoleWindow);

            //Main window
            mainWindow = new NativeOSWindow("Ogre Model Editor", new IntVector2(-1, -1), new IntSize2(OgreModelEditorConfig.EngineConfig.HorizontalRes, OgreModelEditorConfig.EngineConfig.VerticalRes));
            mainWindow.Closed += mainWindow_Closed;

            //Setup DPI
            ScaleHelper._setScaleFactor(mainWindow.WindowScaling);

            //Initailize plugins
            MyGUIInterface.EventLayerKey = EventLayers.Main;

            CamerasInterface.MoveCameraEventLayer = EventLayers.Cameras;
            CamerasInterface.SelectWindowEventLayer = EventLayers.AfterMain;

            pluginManager = new PluginManager(OgreModelEditorConfig.ConfigFile);
            pluginManager.OnConfigureDefaultWindow = createWindow;
            pluginManager.addPluginAssembly(typeof(OgreInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(NativePlatformPlugin).Assembly);
            pluginManager.addPluginAssembly(typeof(MyGUIInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(GuiFrameworkInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(CamerasInterface).Assembly);
            pluginManager.initializePlugins();
            frameClearManager = new FrameClearManager(OgreInterface.Instance.OgrePrimaryWindow.OgreRenderTarget, new Color(0.2f, 0.2f, 0.2f));

            lightManager = pluginManager.RendererPlugin.createSceneViewLightManager();

            VirtualFileSystem.Instance.addArchive(OgreModelEditorConfig.VFSRoot);

            OgreResourceGroupManager.getInstance().addResourceLocation(typeof(OgreModelEditorController).AssemblyQualifiedName, "EmbeddedResource", "DebugShaders", true);
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

            OgreResourceGroupManager.getInstance().initializeAllResourceGroups();

            //Intialize the platform
            systemTimer = new NativeSystemTimer();

            mainTimer = new NativeUpdateTimer(systemTimer);

            mainTimer.FramerateCap = OgreModelEditorConfig.EngineConfig.MaxFPS;
            inputHandler = new NativeInputHandler(mainWindow, false);
            eventManager = new EventManager(inputHandler, Enum.GetValues(typeof(EventLayers)));
            eventUpdate = new EventUpdateListener(eventManager);
            mainTimer.addUpdateListener(eventUpdate);
            pluginManager.setPlatformInfo(mainTimer, eventManager);
            GuiFrameworkInterface.Instance.handleCursors(mainWindow);

            //Initialize controllers
            modelController = new ModelController(this);

            toolInterop.setMoveController(moveController);
            toolInterop.setSelectionController(selectionController);
            toolInterop.setRotateController(rotateController);

            toolManager = new ToolManager(eventManager);
            mainTimer.addUpdateListener(toolManager);
            toolInterop.setToolManager(toolManager);
            movementTool = new MovementTool("MovementTool", moveController);
            toolManager.addTool(movementTool);
            rotateTool = new RotateTool("RotateTool", rotateController);
            toolManager.addTool(rotateTool);

            //Create the GUI

            //Layout Chain
            mdiLayout = new MDILayoutManager();

            //Scene views
            sceneViewController = new SceneViewController(mdiLayout, eventManager, mainTimer, pluginManager.RendererPlugin.PrimaryWindow, MyGUIInterface.Instance.OgrePlatform.getRenderManager(), null);
            sceneStatsDisplayManager = new SceneStatsDisplayManager(sceneViewController, OgreInterface.Instance.OgrePrimaryWindow.OgreRenderTarget);
            sceneStatsDisplayManager.StatsVisible = true;
            createOneWindow();

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
            toolManager.createSceneElements(scene.getDefaultSubScene(), PluginManager.Instance);
            lightManager.sceneLoaded(scene);
        }

        public void shutdown()
        {
            toolManager.destroySceneElements(scene.getDefaultSubScene(), PluginManager.Instance);
            sceneViewController.destroyCameras();
            lightManager.sceneUnloading(scene);
            modelController.destroyModel();
            scene.Dispose();
        }

        public void openModel(String path)
        {
            OgreResourceGroupManager groupManager = OgreResourceGroupManager.getInstance();
            if (modelController.modelActive())
            {
                modelController.destroyModel();
                String lastDir = Path.GetDirectoryName(lastFileName);
                groupManager.removeResourceLocation(lastDir, lastDir);
                groupManager.initializeAllResourceGroups();
            }

            lastFileName = path;
            String dir = Path.GetDirectoryName(path);
            groupManager.addResourceLocation(dir, "FileSystem", dir, true);
            groupManager.initializeAllResourceGroups();
            String meshName = Path.GetFileName(path);
            modelController.createModel(meshName, scene);
            mainForm.setTextureNames(modelController.TextureNames);
            mainForm.currentFileChanged(path);
        }

        public void editExternalResources()
        {
            objectEditor.setEditInterface(resourceManager.getEditInterface(), null, null, null);
            liveResourceManager.changeResourcesToMatch(resourceManager);
            liveResourceManager.initializeResources();
            XmlTextWriter textWriter = new XmlTextWriter(OgreModelEditorConfig.DocRoot + "/resources.xml", Encoding.Default);
            textWriter.Formatting = Formatting.Indented;
            xmlSaver.saveObject(resourceManager, textWriter);
            textWriter.Close();
        }

        public void refreshResources()
        {
            if (modelController.modelActive())
            {
                modelController.destroyModel();
                String dir = Path.GetDirectoryName(lastFileName);
                liveResourceManager.changeResourcesToMatch(emptyResourceManager);
                liveResourceManager.initializeResources();
                OgreResourceGroupManager groupManager = OgreResourceGroupManager.getInstance();
                groupManager.destroyResourceGroup(dir);
                groupManager.initializeAllResourceGroups();
                liveResourceManager.changeResourcesToMatch(resourceManager);
                liveResourceManager.initializeResources();
                groupManager.addResourceLocation(dir, "FileSystem", dir, true);
                groupManager.initializeAllResourceGroups();
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
            toolManager.enableTool(movementTool);
            toolManager.setEnabled(true);
        }

        public void enableRotateTool()
        {
            toolManager.enableTool(rotateTool);
            toolManager.setEnabled(true);
        }

        public void enableSelectTool()
        {
            toolManager.setEnabled(false);
        }

        public void setShowSkeleton(bool show)
        {
            modelController.ShowSkeleton = show;
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

        public SelectionController Selection
        {
            get
            {
                return selectionController;
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

        void mainWindow_Closed(OSWindow window)
        {
            exit();
        }
    }
}

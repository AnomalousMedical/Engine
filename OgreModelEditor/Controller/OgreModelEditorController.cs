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
using OgreWrapper;
using Engine.Saving.XMLSaver;
using System.Xml;
using OgreModelEditor.Controller;
using System.Reflection;
using PCPlatform;

namespace OgreModelEditor
{
    class OgreModelEditorController : IDisposable, IDockProvider
    {
        #region Fields

        //Engine
        private PluginManager pluginManager;
        private LogFileListener logListener;

        //Platform
        private UpdateTimer mainTimer;
        private PCSystemTimer systemTimer;
        private EventManager eventManager;
        private PCInputHandler inputHandler;
        private EventUpdateListener eventUpdate;

        //GUI
        private DrawingWindow hiddenEmbedWindow;
        private OgreModelEditorMain mainForm;
        private ObjectEditorForm objectEditor = new ObjectEditorForm();
        private ConsoleWindow consoleWindow = new ConsoleWindow();

        //Controller
        private DrawingWindowController drawingWindowController = new DrawingWindowController();
        private ModelController modelController;
        private SceneViewLightManager lightManager;

        //Scene
        private SimScene scene;
        private String lastFileName = null;

        //Resources
        private ResourceManager resourceManager;
        private XmlSaver xmlSaver = new XmlSaver();
        private ResourceManager emptyResourceManager;
        private ResourceManager liveResourceManager;

        //Tools
        private ToolInteropController toolInterop = new ToolInteropController();
        private MoveController moveController = new MoveController();
        private SelectionController selectionController = new SelectionController();
        private RotateController rotateController = new RotateController();
        private MovementTool movementTool;
        private RotateTool rotateTool;
        private ToolManager toolManager;

        #endregion Fields

        public void Dispose()
        {
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
            if (pluginManager != null)
            {
                pluginManager.Dispose();
            }
            if (hiddenEmbedWindow != null)
            {
                hiddenEmbedWindow.Dispose();
            }

            OgreModelEditorConfig.save();
            logListener.closeLogFile();
        }

        public void initialize()
        {
            //Create the log.
            logListener = new LogFileListener();
            logListener.openLogFile(OgreModelEditorConfig.DocRoot + "/log.log");
            Log.Default.addLogListener(logListener);
            Log.Default.addLogListener(consoleWindow);

            //Initailize plugins
            hiddenEmbedWindow = new DrawingWindow();
            pluginManager = new PluginManager(OgreModelEditorConfig.ConfigFile);
            pluginManager.OnConfigureDefaultWindow = createWindow;
            pluginManager.addPluginAssembly(typeof(OgreInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(PCPlatformPlugin).Assembly);
            pluginManager.initializePlugins();
            pluginManager.RendererPlugin.PrimaryWindow.setEnabled(false);

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
                resourceManager = xmlSaver.restoreObject(textReader) as ResourceManager;
                if (resourceManager == null)
                {
                    resourceManager = pluginManager.createScratchResourceManager();
                }
                liveResourceManager.changeResourcesToMatch(resourceManager);
                liveResourceManager.initializeResources();
                textReader.Close();
            }

            OgreResourceGroupManager.getInstance().initializeAllResourceGroups();

            //Create the GUI
            mainForm = new OgreModelEditorMain();

            //Intialize the platform
            systemTimer = new PCSystemTimer();

            PCUpdateTimer win32Timer = new PCUpdateTimer(systemTimer);
            WindowsMessagePump windowsPump = new WindowsMessagePump();
            windowsPump.MessageReceived += new PumpMessageEvent(win32Timer_MessageReceived);
            win32Timer.MessagePump = windowsPump;
            mainTimer = win32Timer;

            mainTimer.FramerateCap = OgreModelEditorConfig.EngineConfig.MaxFPS;
            inputHandler = new PCInputHandler(mainForm, false, false, false, mainTimer, windowsPump);
            eventManager = new EventManager(inputHandler, Enum.GetValues(typeof(EventLayers)));
            eventUpdate = new EventUpdateListener(eventManager);
            mainTimer.addUpdateListener(eventUpdate);
            pluginManager.setPlatformInfo(mainTimer, eventManager);

            //Initialize controllers
            drawingWindowController.initialize(this, eventManager, pluginManager.RendererPlugin, OgreModelEditorConfig.ConfigFile);
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

            mainForm.SuspendLayout();

            //Initialize GUI
            mainForm.initialize(this);
            if (!mainForm.restoreWindows(OgreModelEditorConfig.DocRoot + "/windows.ini", getDockContent))
            {
                mainForm.showDockContent(consoleWindow);
                drawingWindowController.createOneWaySplit();
                modelController.createDefaultWindows();
            }

            mainForm.ResumeLayout();

            //startup the form
            mainForm.Show();

            //Create a simple scene to use to show the models
            SimSceneDefinition sceneDefiniton = new SimSceneDefinition();
            OgreSceneManagerDefinition ogreScene = new OgreSceneManagerDefinition("Ogre");
            SimSubSceneDefinition mainSubScene = new SimSubSceneDefinition("Main");
            sceneDefiniton.addSimElementManagerDefinition(ogreScene);
            sceneDefiniton.addSimSubSceneDefinition(mainSubScene);
            mainSubScene.addBinding(ogreScene);
            sceneDefiniton.DefaultSubScene = "Main";

            scene = sceneDefiniton.createScene();
            drawingWindowController.createCameras(mainTimer, scene);
            toolManager.createSceneElements(scene.getDefaultSubScene(), PluginManager.Instance);
            lightManager.sceneLoaded(scene);
        }

        void win32Timer_MessageReceived(ref WinMsg message)
        {
            Message msg = Message.Create(message.hwnd, message.message, message.wParam, message.lParam);
            ManualMessagePump.pumpMessage(ref msg);
        }

        public void start()
        {
            mainTimer.startLoop();
        }

        public void shutdown()
        {
            mainForm.saveWindows(OgreModelEditorConfig.DocRoot + "/windows.ini");
            mainTimer.stopLoop();
            toolManager.destroySceneElements(scene.getDefaultSubScene(), PluginManager.Instance);
            drawingWindowController.destroyCameras();
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

        public void showStats(bool show)
        {
            drawingWindowController.showStats(show);
        }

        public void createOneWindow()
        {
            drawingWindowController.createOneWaySplit();
        }

        public void createTwoWindows()
        {
            drawingWindowController.createTwoWaySplit();
        }

        public void createThreeWindows()
        {
            drawingWindowController.createThreeWayUpperSplit();
        }

        public void createFourWindows()
        {
            drawingWindowController.createFourWaySplit();
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

        /// <summary>
        /// Helper function to create the default window. This is the callback
        /// to the PluginManager.
        /// </summary>
        /// <param name="defaultWindow"></param>
        private void createWindow(out WindowInfo defaultWindow)
        {
            defaultWindow = new WindowInfo(hiddenEmbedWindow, "Primary");
        }

        /// <summary>
        /// Callback to restore the dock windows.
        /// </summary>
        /// <param name="persistString"></param>
        /// <returns></returns>
        private IDockContent getDockContent(String persistString)
        {
            DockContent content = modelController.getDockContent(persistString);
            if (content != null)
            {
                return content;
            }
            if (persistString == typeof(ConsoleWindow).ToString())
            {
                return consoleWindow;
            }
            Vector3 translation;
            Vector3 lookAt;
            String name;
            if (DrawingWindowHost.RestoreFromString(persistString, out name, out translation, out lookAt))
            {
                return drawingWindowController.createDrawingWindowHost(name, translation, lookAt);
            }
            return null;
        }

        #region IDockProvider Members

        public void showDockContent(DockContent content)
        {
            mainForm.showDockContent(content);
        }

        public void hideDockContent(DockContent content)
        {
            mainForm.hideDockContent(content);
        }

        #endregion

        public SelectionController Selection
        {
            get
            {
                return selectionController;
            }
        }

        public UpdateTimer MainTimer
        {
            get
            {
                return mainTimer;
            }
        }
    }
}

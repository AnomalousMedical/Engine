using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Logging;
using Engine.Renderer;
using Engine.Platform;
using Engine.Resources;
using System.IO;
using Engine.ObjectManagement;
using Editor;
using System.Xml;
using Engine.Saving.XMLSaver;
using Engine.Saving;
using Engine.Editing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Diagnostics;

namespace Anomaly
{
    /// <summary>
    /// This is the primary controller for the Anomaly editor.
    /// </summary>
    class AnomalyController : IDisposable, IDockProvider
    {

        #region Fields
        //Engine
        private PluginManager pluginManager;
        private LogFileListener logListener;

        //GUI
        private AnomalyMain mainForm;
        private DrawingWindow hiddenEmbedWindow;
        private ObjectEditorForm objectEditor = new ObjectEditorForm();
        private DrawingWindowController drawingWindowController = new DrawingWindowController();
        private MovePanel movePanel = new MovePanel();
        private TemplatePanel templatePanel = new TemplatePanel();
        private SimObjectPanel simObjectPanel = new SimObjectPanel();
        private EulerRotatePanel rotatePanel = new EulerRotatePanel();
        private Dictionary<String, DebugVisualizer> debugVisualizers = new Dictionary<string, DebugVisualizer>();
        private ConsoleWindow consoleWindow = new ConsoleWindow();

        //Platform
        private UpdateTimer mainTimer;
        private SystemTimer systemTimer;
        private EventManager eventManager;
        private InputHandler inputHandler;
        private EventUpdateListener eventUpdate;
        private FullSpeedUpdateListener fixedUpdate;

        //Scene
        private TemplateController templates;
        private SceneController sceneController = new SceneController();
        private ResourceController resourceController = new ResourceController();
        private SimObjectController simObjectController = new SimObjectController();
        private InstanceBuilder instanceBuilder;

        //Tools
        private ToolInteropController toolInterop = new ToolInteropController();
        private MoveController moveController = new MoveController();
        private SelectionController selectionController = new SelectionController();
        private RotateController rotateController = new RotateController();
        private MovementTool movementTool;
        private RotateTool rotateTool;
        private ToolManager toolManager;

        Stopwatch stopwatch = new Stopwatch();

        //Serialization
        private XmlSaver xmlSaver = new XmlSaver();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public AnomalyController()
        {
            
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Intialize all plugins and create everything.
        /// </summary>
        public void initialize(AnomalyProject project)
        {
            //Create the log.
            logListener = new LogFileListener();
            logListener.openLogFile(AnomalyConfig.DocRoot + "/log.log");
            Log.Default.addLogListener(logListener);
            Log.Default.addLogListener(consoleWindow);

            //Load the config file and set the resource root up.
            Resource.ResourceRoot = project.ResourceSection.ResourceRoot;
            Log.Default.sendMessage("Resource root is \"{0}\".", LogLevel.ImportantInfo, "Editor", Resource.ResourceRoot);

            //Initialize the plugins
            hiddenEmbedWindow = new DrawingWindow();
            pluginManager = new PluginManager(AnomalyConfig.ConfigFile);
            pluginManager.OnConfigureDefaultWindow = createWindow;
            DynamicDLLPluginLoader pluginLoader = new DynamicDLLPluginLoader();
            project.PluginSection.resetPluginIterator();
            while (project.PluginSection.hasNext())
            {
                pluginLoader.addPath(project.PluginSection.nextPlugin());
            }
            pluginLoader.loadPlugins(pluginManager);
            pluginManager.initializePlugins();
            pluginManager.RendererPlugin.PrimaryWindow.setEnabled(false);

            //Create the main form
            mainForm = new AnomalyMain();

            //Intialize the platform
            systemTimer = pluginManager.PlatformPlugin.createTimer();
            mainTimer = new UpdateTimer(systemTimer, new WindowsFormsUpdate());
            mainTimer.FramerateCap = AnomalyConfig.EngineConfig.MaxFPS;
            inputHandler = pluginManager.PlatformPlugin.createInputHandler(mainForm, false, false, false);
            eventManager = new EventManager(inputHandler);
            eventUpdate = new EventUpdateListener(eventManager);
            mainTimer.addFixedUpdateListener(eventUpdate);
            pluginManager.setPlatformInfo(mainTimer, eventManager);

            //Initialize controllers
            templates = new TemplateController(project.WorkingDirectory, this);
            instanceBuilder = new InstanceBuilder(templates);
            sceneController.initialize(this);
            sceneController.OnSceneLoaded += sceneController_OnSceneLoaded;
            sceneController.OnSceneUnloading += sceneController_OnSceneUnloading;
            simObjectController.initialize(this);
            resourceController.initialize(this);
            toolInterop.setMoveController(moveController);
            toolInterop.setSelectionController(selectionController);
            toolInterop.setRotateController(rotateController);

            toolManager = new ToolManager(eventManager);
            mainTimer.addFixedUpdateListener(toolManager);
            fixedUpdate = new FullSpeedUpdateListener(sceneController);
            mainTimer.addFullSpeedUpdateListener(fixedUpdate);
            toolInterop.setToolManager(toolManager);
            movementTool = new MovementTool("MovementTool", moveController);
            toolManager.addTool(movementTool);
            rotateTool = new RotateTool("RotateTool", rotateController);
            toolManager.addTool(rotateTool);

            //Initialize the windows
            mainForm.initialize(this);
            drawingWindowController.initialize(this, eventManager, pluginManager.RendererPlugin, AnomalyConfig.ConfigFile);
            movePanel.initialize(moveController);
            templatePanel.initialize(templates);
            simObjectPanel.intialize(this);
            rotatePanel.initialize(rotateController);

            //Initialize debug visualizers
            foreach (DebugInterface debugInterface in pluginManager.getDebugInterfaces())
            {
                DebugVisualizer visualizer = new DebugVisualizer();
                visualizer.initialize(debugInterface);
                debugVisualizers.Add(visualizer.Text, visualizer);
            }

            mainForm.SuspendLayout();

            //Attempt to restore windows, or create default layout.
            if (!mainForm.restoreWindows(AnomalyConfig.DocRoot + "/windows.ini", getDockContent))
            {
                drawingWindowController.createOneWaySplit();
                mainForm.showDockContent(movePanel);
                rotatePanel.Show(movePanel.Pane, DockAlignment.Right, 0.5);
                mainForm.showDockContent(templatePanel);
                mainForm.showDockContent(simObjectPanel);
                foreach (DebugVisualizer visualizer in debugVisualizers.Values)
                {
                    mainForm.showDockContent(visualizer);
                }
                mainForm.showDockContent(consoleWindow);
            }
            else
            {
                foreach (DebugVisualizer visualizer in debugVisualizers.Values)
                {
                    if (visualizer.DockPanel == null)
                    {
                        mainForm.showDockContent(visualizer);
                    }
                }
            }

            mainForm.ResumeLayout();
        }

        /// <summary>
        /// Dispose of this controller and cleanup.
        /// </summary>
        public void Dispose()
        {
            if (eventManager != null)
            {
                eventManager.Dispose();
            }
            if (inputHandler != null)
            {
                pluginManager.PlatformPlugin.destroyInputHandler(inputHandler);
            }
            if(systemTimer != null)
            {
                pluginManager.PlatformPlugin.destroyTimer(systemTimer);
            }
            if (pluginManager != null)
            {
                pluginManager.Dispose();
            }
            if (hiddenEmbedWindow != null)
            {
                hiddenEmbedWindow.Dispose();
            }

            AnomalyConfig.save();
            logListener.closeLogFile();
        }

        /// <summary>
        /// Show the form to the user and start the loop.
        /// </summary>
        public void start()
        {
            mainForm.Show();
            mainTimer.startLoop();
        }

        /// <summary>
        /// Stop the loop and begin the process of shutting down the program.
        /// </summary>
        public void shutdown()
        {
            mainForm.saveWindows(AnomalyConfig.DocRoot + "/windows.ini");
            sceneController.destroyScene();
            mainTimer.stopLoop();
        }

        /// <summary>
        /// Show the primary ObjectEditorForm for the given EditInterface.
        /// </summary>
        /// <param name="editInterface">The EditInterface to display on the form.</param>
        public void showObjectEditor(EditInterface editInterface)
        {
            objectEditor.EditorPanel.setEditInterface(editInterface);
            objectEditor.ShowDialog(mainForm);
            objectEditor.EditorPanel.clearEditInterface();
        }

        public void showDockContent(DockContent content)
        {
            mainForm.showDockContent(content);
        }

        public void hideDockContent(DockContent content)
        {
            mainForm.hideDockContent(content);
        }

        /// <summary>
        /// Create a new empty scene.
        /// </summary>
        public void createNewScene()
        {
            ScenePackage emptyScene = new ScenePackage();
            emptyScene.ResourceManager = pluginManager.createEmptyResourceManager();
            emptyScene.SceneDefinition = new SimSceneDefinition();
            emptyScene.SimObjectManagerDefinition = new SimObjectManagerDefinition();
            changeScene(emptyScene);
        }

        /// <summary>
        /// Load an exisiting scene.
        /// </summary>
        /// <param name="filename">The filename to load.</param>
        public void loadScene(String filename)
        {
            stopwatch.Start();
            XmlTextReader textReader = new XmlTextReader(filename);
            ScenePackage scenePackage = xmlSaver.restoreObject(textReader) as ScenePackage;
            if (scenePackage != null)
            {
                changeScene(scenePackage);
            }
            else
            {
                MessageBox.Show(mainForm, String.Format("Could not load scene from {0}.", filename), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            textReader.Close();
            stopwatch.Stop();
            Log.Info("Scene loaded in {0} seconds.", stopwatch.Elapsed.TotalSeconds);
            stopwatch.Reset();
        }

        /// <summary>
        /// Save the scene to the given filename.
        /// </summary>
        /// <param name="filename">The filename to save to.</param>
        public void saveScene(String filename)
        {
            ScenePackage scenePackage = new ScenePackage();
            scenePackage.SceneDefinition = sceneController.getSceneDefinition();
            scenePackage.ResourceManager = resourceController.getResourceManager();
            scenePackage.SimObjectManagerDefinition = simObjectController.getSimObjectManagerDefinition();
            XmlTextWriter fileWriter = new XmlTextWriter(filename, Encoding.Default);
            fileWriter.Formatting = Formatting.Indented;
            xmlSaver.saveObject(scenePackage, fileWriter);
            fileWriter.Close();
        }
        
        /// <summary>
        /// Put the editor into static mode. This allows full editing privlidges
        /// of all objects.
        /// </summary>
        public void setStaticMode()
        {
            simObjectController.captureSceneProperties();
            movePanel.Enabled = true;
            rotatePanel.Enabled = true;
            templatePanel.Enabled = true;
            simObjectPanel.Enabled = true;
            sceneController.setDynamicMode(false);
            sceneController.destroyScene();
            sceneController.createScene();
            toolManager.setEnabled(true);
        }

        /// <summary>
        /// Put the editor into dynamic mode. This allows the objects to move
        /// and behave as they would in the scene, however, it limits editing
        /// capability.
        /// </summary>
        public void setDynamicMode()
        {
            toolManager.setEnabled(false);
            movePanel.Enabled = false;
            rotatePanel.Enabled = false;
            templatePanel.Enabled = false;
            simObjectPanel.Enabled = false;
            sceneController.setDynamicMode(true);
            sceneController.destroyScene();
            sceneController.createScene();
        }

        public void enableMoveTool()
        {
            toolManager.enableTool(movementTool);
            toolManager.setEnabled(!sceneController.isDynamicMode());
        }

        public void enableRotateTool()
        {
            toolManager.enableTool(rotateTool);
            toolManager.setEnabled(!sceneController.isDynamicMode());
        }

        public void enableSelectTool()
        {
            toolManager.setEnabled(false);
        }

        public void importInstances(String filename)
        {
            XmlTextReader textReader = null;
            try
            {
                textReader = new XmlTextReader(filename);
                SimObjectManagerDefinition managerDefintion = simObjectController.getSimObjectManagerDefinition();
                instanceBuilder.loadInstances(textReader, managerDefintion);
                simObjectController.setSceneManagerDefintion(managerDefintion);
                sceneController.destroyScene();
                sceneController.createScene();
            }
            catch(Exception e)
            {
                Log.Default.printException(e);
                MessageBox.Show(mainForm, String.Format("An exception occured when loading the instances:\n{0}.", e.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (textReader != null)
                {
                    textReader.Close();
                }
            }
        }

        /// <summary>
        /// Restore function for restoring the window layout.
        /// </summary>
        /// <param name="persistString">The string describing the window.</param>
        /// <returns>The IDockContent associated with the given string.</returns>
        private IDockContent getDockContent(String persistString)
        {
            if (persistString == movePanel.GetType().ToString())
            {
                return movePanel;
            }
            if (persistString == rotatePanel.GetType().ToString())
            {
                return rotatePanel;
            }
            if (persistString == templatePanel.GetType().ToString())
            {
                return templatePanel;
            }
            if (persistString == simObjectPanel.GetType().ToString())
            {
                return simObjectPanel;
            }
            if (persistString == consoleWindow.GetType().ToString())
            {
                return consoleWindow;
            }
            String name;
            if (DebugVisualizer.RestoreFromPersistance(persistString, out name))
            {
                if (debugVisualizers.ContainsKey(name))
                {
                    return debugVisualizers[name];
                }
                return null;
            }
            Vector3 translation;
            Vector3 lookAt;
            if (DrawingWindowHost.RestoreFromString(persistString, out name, out translation, out lookAt))
            {
                return drawingWindowController.createDrawingWindowHost(name, translation, lookAt);
            }
            return null;
        }

        /// <summary>
        /// Helper function to create the default window. This is the callback
        /// to the PluginManager.
        /// </summary>
        /// <param name="defaultWindow"></param>
        private void createWindow(out DefaultWindowInfo defaultWindow)
        {
            defaultWindow = new DefaultWindowInfo(hiddenEmbedWindow);
        }

        /// <summary>
        /// Callback for when the scene is loaded.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="scene"></param>
        private void sceneController_OnSceneLoaded(SceneController controller, SimScene scene)
        {
            drawingWindowController.createCameras(mainTimer, scene);
            toolManager.createSceneElements(scene.getDefaultSubScene(), pluginManager);
        }

        /// <summary>
        /// Callback for when the scene is unloading.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="scene"></param>
        private void sceneController_OnSceneUnloading(SceneController controller, SimScene scene)
        {
            drawingWindowController.destroyCameras();
            toolManager.destroySceneElements(scene.getDefaultSubScene(), pluginManager);
        }

        /// <summary>
        /// Helper funciton to change to a new scene from a ScenePackage.
        /// </summary>
        /// <param name="scenePackage">The ScenePackage to load.</param>
        private void changeScene(ScenePackage scenePackage)
        {
            sceneController.destroyScene();
            sceneController.setSceneDefinition(scenePackage.SceneDefinition);
            resourceController.setResources(scenePackage.ResourceManager);
            simObjectController.setSceneManagerDefintion(scenePackage.SimObjectManagerDefinition);
            sceneController.createScene();
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// The PluginManager with all plugins currently loaded.
        /// </summary>
        public PluginManager PluginManager
        {
            get
            {
                return pluginManager;
            }
        }

        /// <summary>
        /// The EventManager for the editor.
        /// </summary>
        public EventManager EventManager
        {
            get
            {
                return eventManager;
            }
        }

        /// <summary>
        /// The main UpdateTimer driving the main thread.
        /// </summary>
        public UpdateTimer MainTimer
        {
            get
            {
                return mainTimer;
            }
        }

        /// <summary>
        /// The MoveController to move objects with.
        /// </summary>
        public MoveController MoveController
        {
            get
            {
                return moveController;
            }
        }

        /// <summary>
        /// The TemplateController that manages the currently created templates.
        /// </summary>
        public TemplateController TemplateController
        {
            get
            {
                return templates;
            }
        }

        /// <summary>
        /// The SceneController that handles aspects of the scene.
        /// </summary>
        public SceneController SceneController
        {
            get
            {
                return sceneController;
            }
        }

        /// <summary>
        /// The ResourceController that manages the resources.
        /// </summary>
        public ResourceController ResourceController
        {
            get
            {
                return resourceController;
            }
        }

        /// <summary>
        /// The SelectionController that manages the current selection.
        /// </summary>
        public SelectionController SelectionController
        {
            get
            {
                return selectionController;
            }
        }

        /// <summary>
        /// The SimObjectController that manages the SimObjects.
        /// </summary>
        public SimObjectController SimObjectController
        {
            get
            {
                return simObjectController;
            }
        }

        /// <summary>
        /// The RotateController that handles rotating objects.
        /// </summary>
        public RotateController RotateController
        {
            get
            {
                return rotateController;
            }
        }

        /// <summary>
        /// Get the SplitViewController.
        /// </summary>
        public DrawingWindowController ViewController
        {
            get
            {
                return drawingWindowController;
            }
        }

        #endregion Properties
    }
}

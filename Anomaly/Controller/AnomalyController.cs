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

namespace Anomaly
{
    /// <summary>
    /// This is the primary controller for the Anomaly editor.
    /// </summary>
    class AnomalyController : IDisposable
    {

        #region Fields
        //Engine
        private PluginManager pluginManager;
        private LogFileListener logListener;

        //GUI
        private AnomalyMain mainForm;
        private DrawingWindow hiddenEmbedWindow;
        private ObjectEditorForm objectEditor = new ObjectEditorForm();
        private SplitViewController splitViewController = new SplitViewController();

        //Platform
        private UpdateTimer mainTimer;
        private EventManager eventManager;
        private InputHandler inputHandler;
        private EventUpdateListener eventUpdate;

        //Scene
        private TemplateController templates;
        private SceneController sceneController = new SceneController();
        private ResourceController resourceController = new ResourceController();
        private SimObjectController simObjectController = new SimObjectController();

        //Tools
        private ToolInteropController toolInterop = new ToolInteropController();
        private MoveController moveController = new MoveController();
        private SelectionController selectionController = new SelectionController();
        private RotateController rotateController = new RotateController();
        private MovementTool movementTool;
        private RotateTool rotateTool;
        private ToolManager toolManager;

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
        public void intialize()
        {
            //Create the log.
            logListener = new LogFileListener();
            logListener.openLogFile(AnomalyConfig.DocRoot + "/log.log");
            Log.Default.addLogListener(logListener);

            //Load the config file and set the resource root up.
            Resource.ResourceRoot = AnomalyConfig.ResourceSection.ResourceRoot;

            //Initialize the plugins
            hiddenEmbedWindow = new DrawingWindow();
            pluginManager = new PluginManager();
            pluginManager.OnConfigureDefaultWindow = createWindow;
            DynamicDLLPluginLoader pluginLoader = new DynamicDLLPluginLoader();
            AnomalyConfig.PluginSection.resetPluginIterator();
            while(AnomalyConfig.PluginSection.hasNext())
            {
                pluginLoader.addPath(AnomalyConfig.PluginSection.nextPlugin());
            }
            pluginLoader.loadPlugins(pluginManager);
            pluginManager.initializePlugins();
            pluginManager.RendererPlugin.PrimaryWindow.setEnabled(false);

            //Create the main form
            mainForm = new AnomalyMain();

            //Intialize the platform
            mainTimer = pluginManager.PlatformPlugin.createTimer();
            inputHandler = pluginManager.PlatformPlugin.createInputHandler(mainForm, false, false, false);
            eventManager = new EventManager(inputHandler);
            eventUpdate = new EventUpdateListener(eventManager);
            mainTimer.addFixedUpdateListener(eventUpdate);
            pluginManager.setPlatformInfo(mainTimer, eventManager);

            //Initialize controllers
            templates = new TemplateController(AnomalyConfig.DocRoot, this);
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
            toolInterop.setToolManager(toolManager);
            movementTool = new MovementTool("MovementTool", moveController);
            toolManager.addTool(movementTool);
            rotateTool = new RotateTool("RotateTool", rotateController);
            toolManager.addTool(rotateTool);
            toolManager.enableTool(movementTool);

            //Initialize the windows
            mainForm.initialize(this);
            splitViewController.initialize(eventManager, pluginManager.RendererPlugin, mainForm.SplitControl);
            splitViewController.createFourWaySplit();
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
            mainTimer.processMessageLoop(true);
            mainForm.Show();
            mainTimer.startLoop();
        }

        /// <summary>
        /// Stop the loop and begin the process of shutting down the program.
        /// </summary>
        public void shutdown()
        {
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
            sceneController.setMode(false);
            sceneController.destroyScene();
            sceneController.createScene();
        }

        /// <summary>
        /// Put the editor into dynamic mode. This allows the objects to move
        /// and behave as they would in the scene, however, it limits editing
        /// capability.
        /// </summary>
        public void setDynamicMode()
        {
            sceneController.setMode(true);
            sceneController.destroyScene();
            sceneController.createScene();
        }

        public void enableMoveTool()
        {
            toolManager.enableTool(movementTool);
        }

        public void enableRotateTool()
        {
            toolManager.enableTool(rotateTool);
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
            splitViewController.createCameras(mainTimer, scene);
            toolManager.createSceneElements(scene.getDefaultSubScene(), pluginManager);
        }

        /// <summary>
        /// Callback for when the scene is unloading.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="scene"></param>
        private void sceneController_OnSceneUnloading(SceneController controller, SimScene scene)
        {
            splitViewController.destroyCameras(mainTimer);
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
        public SplitViewController ViewController
        {
            get
            {
                return splitViewController;
            }
        }

        #endregion Properties
    }
}

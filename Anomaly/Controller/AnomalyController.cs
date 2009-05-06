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
            AnomalyConfig.ConfigFile.loadConfigFile();
            Resource.ResourceRoot = AnomalyConfig.ConfigFile.createOrRetrieveConfigSection("Resources").getValue("Root", ".");

            //Initialize the plugins
            hiddenEmbedWindow = new DrawingWindow();
            pluginManager = new PluginManager();
            pluginManager.OnConfigureDefaultWindow = createWindow;
            DynamicDLLPluginLoader pluginLoader = new DynamicDLLPluginLoader();
            ConfigSection plugins = AnomalyConfig.ConfigFile.createOrRetrieveConfigSection("Plugins");
            for (int i = 0; plugins.hasValue("Plugin" + i); ++i)
            {
                pluginLoader.addPath(plugins.getValue("Plugin" + i, ""));
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

            //Initialize the windows
            mainForm.initialize(this);
            splitViewController.initialize(eventManager, pluginManager.RendererPlugin, mainForm.SplitControl);
            splitViewController.createFourWaySplit();
        }

        void sceneController_OnSceneUnloading(SceneController controller, SimScene scene)
        {
            splitViewController.destroyCameras(mainTimer);
        }

        void sceneController_OnSceneLoaded(SceneController controller, SimScene scene)
        {
            splitViewController.createCameras(mainTimer, scene);
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

        public void showObjectEditor(EditInterface editInterface)
        {
            objectEditor.EditorPanel.setEditInterface(editInterface);
            objectEditor.ShowDialog(mainForm);
            objectEditor.EditorPanel.clearEditInterface();
        }

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

            AnomalyConfig.ConfigFile.writeConfigFile();
            logListener.closeLogFile();
        }

        public void createNewScene()
        {
            ScenePackage emptyScene = new ScenePackage();
            emptyScene.ResourceManager = pluginManager.createSecondaryResourceManager();
            emptyScene.SceneDefinition = new SimSceneDefinition();
            emptyScene.SimObjectManagerDefinition = new SimObjectManagerDefinition();
            changeScene(emptyScene);
        }

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

        private void changeScene(ScenePackage scenePackage)
        {
            sceneController.destroyScene();
            sceneController.setSceneDefinition(scenePackage.SceneDefinition);
            resourceController.setResources(scenePackage.ResourceManager);
            simObjectController.setSceneManagerDefintion(scenePackage.SimObjectManagerDefinition);
            sceneController.createDynamicScene();
        }

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

        private void createWindow(out DefaultWindowInfo defaultWindow)
        {
            defaultWindow = new DefaultWindowInfo(hiddenEmbedWindow);
        }

        #endregion Functions

        #region Properties

        public PluginManager PluginManager
        {
            get
            {
                return pluginManager;
            }
        }

        public EventManager EventManager
        {
            get
            {
                return eventManager;
            }
        }

        public UpdateTimer MainTimer
        {
            get
            {
                return mainTimer;
            }
        }

        public MoveController MoveController
        {
            get
            {
                return moveController;
            }
        }

        public TemplateController TemplateController
        {
            get
            {
                return templates;
            }
        }

        public SceneController SceneController
        {
            get
            {
                return sceneController;
            }
        }

        public ResourceController ResourceController
        {
            get
            {
                return resourceController;
            }
        }

        public SelectionController SelectionController
        {
            get
            {
                return selectionController;
            }
        }

        public SimObjectController SimObjectController
        {
            get
            {
                return simObjectController;
            }
        }

        public RotateController RotateController
        {
            get
            {
                return rotateController;
            }
        }

        #endregion Properties
    }
}

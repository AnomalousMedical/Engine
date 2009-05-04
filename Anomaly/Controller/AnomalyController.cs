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

        //Platform
        private UpdateTimer mainTimer;
        private EventManager eventManager;
        private InputHandler inputHandler;
        private EventUpdateListener eventUpdate;

        //Scene
        private TemplateController templates;
        private SceneController sceneController = new SceneController();

        //Tools
        private MoveController moveController = new MoveController();

        private SplitViewController splitViewController = new SplitViewController();

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
            templates = new TemplateController(AnomalyConfig.DocRoot, moveController);
            sceneController.initialize(this);
            sceneController.OnSceneLoaded += new SceneLoaded(sceneController_OnSceneLoaded);
            sceneController.OnSceneUnloading += new SceneUnloading(sceneController_OnSceneUnloading);

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

        #endregion Properties
    }
}

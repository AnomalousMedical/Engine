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
    /// <summary>
    /// This delegate is called when a new scene is loaded.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="scene"></param>
    delegate void SceneLoaded(AnomalyController controller, SimScene scene);

    /// <summary>
    /// This delegate is called when a scene is about to unload.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="scene"></param>
    delegate void SceneUnloading(AnomalyController controller, SimScene scene);

    /// <summary>
    /// This delegate is called when a scene has unloaded and is destroyed.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="scene"></param>
    delegate void SceneUnloaded(AnomalyController controller);

    class AnomalyController : IDisposable
    {

        #region Fields
        //Engine
        private PluginManager pluginManager;
        private LogFileListener logListener;

        //GUI
        private AnomalyMain mainForm;
        private DrawingWindow hiddenEmbedWindow;
        private ObjectEditorForm objectEditor;

        //Platform
        private UpdateTimer mainTimer;
        private EventManager eventManager;
        private InputHandler inputHandler;
        private EventUpdateListener eventUpdate;

        //Scene
        private SimScene scene;
        private TemplateController templates = new TemplateController();

        //Tools
        private MoveController moveController = new MoveController();

        private SplitViewController splitViewController = new SplitViewController();

        #endregion Fields

        #region Events

        /// <summary>
        /// This event is fired when a scene is loaded.
        /// </summary>
        public event SceneLoaded OnSceneLoaded;

        /// <summary>
        /// This event is fired when a scene starts unloading.
        /// </summary>
        public event SceneUnloading OnSceneUnloading;

        /// <summary>
        /// This event is fired when a scene has finished unloading.
        /// </summary>
        public event SceneUnloaded OnSceneUnloaded;

        #endregion Events

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
            objectEditor = new ObjectEditorForm();

            //Intialize the platform
            mainTimer = pluginManager.PlatformPlugin.createTimer();
            inputHandler = pluginManager.PlatformPlugin.createInputHandler(mainForm, false, false, false);
            eventManager = new EventManager(inputHandler);
            eventUpdate = new EventUpdateListener(eventManager);
            mainTimer.addFixedUpdateListener(eventUpdate);
            pluginManager.setPlatformInfo(mainTimer, eventManager);

            //Initialize the windows
            mainForm.initialize(this);
            splitViewController.initialize(eventManager, pluginManager.RendererPlugin, mainForm.SplitControl);
            splitViewController.createFourWaySplit();
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
            if (scene != null)
            {
                destroyScene();
            }
            mainTimer.stopLoop();
        }

        public void createNewScene()
        {
            setupResources();
            setupScene();
            //temp
            if (File.Exists("simObjects.xml"))
            {
                XmlTextReader textReader = new XmlTextReader("simObjects.xml");
                XmlSaver xmlSaver = new XmlSaver();
                SimObjectManagerDefinition simObjectManagerDef = xmlSaver.restoreObject(textReader) as SimObjectManagerDefinition;
                textReader.Close();
                CopySaver copySaver = new CopySaver();
                SimObjectDefinition clone = (SimObjectDefinition)copySaver.copyObject(simObjectManagerDef.getSimObject("Test"));
                clone.Name = "Clone";
                clone.Translation = new Vector3(5.0f, 0.0f, 0.0f);
                simObjectManagerDef.addSimObject(clone);

                SimObjectManager manager = simObjectManagerDef.createSimObjectManager(scene.getDefaultSubScene());
                scene.buildScene();
            }
        }

        private void setupScene()
        {
            //Create a scene definition
            SimSceneDefinition sceneDef;
            if (!File.Exists(AnomalyConfig.DocRoot + "/scene.xml"))
            {
                sceneDef = new SimSceneDefinition();
                objectEditor.EditorPanel.setEditInterface(sceneDef.getEditInterface());
                objectEditor.ShowDialog();
                objectEditor.EditorPanel.clearEditInterface();

                XmlTextWriter textWriter = new XmlTextWriter(AnomalyConfig.DocRoot + "/scene.xml", Encoding.Unicode);
                textWriter.Formatting = Formatting.Indented;
                XmlSaver xmlSaver = new XmlSaver();
                xmlSaver.saveObject(sceneDef, textWriter);
                textWriter.Close();
            }
            else
            {
                XmlTextReader textReader = new XmlTextReader(AnomalyConfig.DocRoot + "/scene.xml");
                XmlSaver xmlSaver = new XmlSaver();
                sceneDef = xmlSaver.restoreObject(textReader) as SimSceneDefinition;
                textReader.Close();
            }

            scene = sceneDef.createScene();
            splitViewController.createCameras(mainTimer, scene);
            if (OnSceneLoaded != null)
            {
                OnSceneLoaded.Invoke(this, scene);
            }
        }

        private void setupResources()
        {
            ResourceManager secondaryResources;
            if (!File.Exists(AnomalyConfig.DocRoot + "/resources.xml"))
            {
                secondaryResources = pluginManager.createSecondaryResourceManager();

                objectEditor.EditorPanel.setEditInterface(secondaryResources.getEditInterface());
                objectEditor.ShowDialog();
                objectEditor.EditorPanel.clearEditInterface();

                XmlTextWriter resourceWriter = new XmlTextWriter(AnomalyConfig.DocRoot + "/resources.xml", Encoding.Unicode);
                resourceWriter.Formatting = Formatting.Indented;
                XmlSaver resourceSaver = new XmlSaver();
                resourceSaver.saveObject(secondaryResources, resourceWriter);
                resourceWriter.Close();
            }
            else
            {
                XmlTextReader resourceReader = new XmlTextReader(AnomalyConfig.DocRoot + "/resources.xml");
                XmlSaver xmlSaver = new XmlSaver();
                secondaryResources = xmlSaver.restoreObject(resourceReader) as ResourceManager;
                resourceReader.Close();
            }

            pluginManager.PrimaryResourceManager.changeResourcesToMatch(secondaryResources);
            pluginManager.PrimaryResourceManager.forceResourceRefresh();
        }

        private void destroyScene()
        {
            splitViewController.destroyCameras(mainTimer);
            if (OnSceneUnloading != null)
            {
                OnSceneUnloading.Invoke(this, scene);
            }
            scene.Dispose();
            scene = null;
            if (OnSceneUnloaded != null)
            {
                OnSceneUnloaded.Invoke(this);
            }
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

        #endregion Properties
    }
}

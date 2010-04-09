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
        private IObjectEditorGUI mainObjectEditor = new ObjectEditorForm();
        private DrawingWindowController drawingWindowController = new DrawingWindowController();
        private MovePanel movePanel = new MovePanel();
        private EulerRotatePanel rotatePanel = new EulerRotatePanel();
        private Dictionary<String, DebugVisualizer> debugVisualizers = new Dictionary<string, DebugVisualizer>();
        private ConsoleWindow consoleWindow = new ConsoleWindow();
        private VerticalObjectEditor verticalObjectEditor = new VerticalObjectEditor();

        //Platform
        private UpdateTimer mainTimer;
        private SystemTimer systemTimer;
        private EventManager eventManager;
        private InputHandler inputHandler;
        private EventUpdateListener eventUpdate;
        private FullSpeedUpdateListener fixedUpdate;

        //Scene
        private SceneController sceneController = new SceneController();
        private ResourceController resourceController = new ResourceController();
        private SimObjectController simObjectController;
        private InstanceBuilder instanceBuilder;

        //Tools
        private ToolInteropController toolInterop = new ToolInteropController();
        private MoveController moveController = new MoveController();
        private SelectionController selectionController = new SelectionController();
        private RotateController rotateController = new RotateController();
        private MovementTool movementTool;
        private RotateTool rotateTool;
        private ToolManager toolManager;

        private Stopwatch stopwatch = new Stopwatch();
        private Solution solution;

        //Solution
        private SolutionController solutionController;
        private SolutionPanel solutionPanel = new SolutionPanel();

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
        public void initialize(Solution solution)
        {
            this.solution = solution;

            //Create the log.
            logListener = new LogFileListener();
            logListener.openLogFile(AnomalyConfig.DocRoot + "/log.log");
            Log.Default.addLogListener(logListener);
            Log.Default.addLogListener(consoleWindow);

            //Initialize the plugins
            hiddenEmbedWindow = new DrawingWindow();
            pluginManager = new PluginManager(AnomalyConfig.ConfigFile);
            pluginManager.OnConfigureDefaultWindow = createWindow;
            DynamicDLLPluginLoader pluginLoader = new DynamicDLLPluginLoader();
            ConfigIterator pluginIterator = solution.PluginSection.PluginIterator;
            pluginIterator.reset();
            while (pluginIterator.hasNext())
            {
                pluginLoader.addPath(pluginIterator.next());
            }
            pluginLoader.loadPlugins(pluginManager);
            pluginManager.initializePlugins();
            pluginManager.RendererPlugin.PrimaryWindow.setEnabled(false);

            //Load the config file and set the resource root up.
            VirtualFileSystem.Instance.addArchive(solution.ResourceSection.ResourceRoot);

            solution.loadExternalFiles(pluginManager);

            //Create the main form
            AnomalyTreeIcons.createIcons();
            mainForm = new AnomalyMain();

            //Intialize the platform
            systemTimer = pluginManager.PlatformPlugin.createTimer();

            Win32UpdateTimer win32Timer = new Win32UpdateTimer(systemTimer);
            win32Timer.MessageReceived += new PumpMessageEvent(win32Timer_MessageReceived);
            mainTimer = win32Timer;

            mainTimer.FramerateCap = AnomalyConfig.EngineConfig.MaxFPS;
            inputHandler = pluginManager.PlatformPlugin.createInputHandler(mainForm, false, false, false);
            eventManager = new EventManager(inputHandler);
            eventUpdate = new EventUpdateListener(eventManager);
            mainTimer.addFixedUpdateListener(eventUpdate);
            pluginManager.setPlatformInfo(mainTimer, eventManager);

            //Initialize controllers
            instanceBuilder = new InstanceBuilder();
            sceneController.initialize(this);
            sceneController.OnSceneLoaded += sceneController_OnSceneLoaded;
            sceneController.OnSceneUnloading += sceneController_OnSceneUnloading;
            resourceController.initialize(this);
            toolInterop.setMoveController(moveController);
            toolInterop.setSelectionController(selectionController);
            toolInterop.setRotateController(rotateController);
            simObjectController = new SimObjectController(this);

            toolManager = new ToolManager(eventManager);
            mainTimer.addFixedUpdateListener(toolManager);
            fixedUpdate = new FullSpeedUpdateListener(sceneController);
            mainTimer.addFullSpeedUpdateListener(fixedUpdate);
            toolInterop.setToolManager(toolManager);
            movementTool = new MovementTool("MovementTool", moveController);
            toolManager.addTool(movementTool);
            rotateTool = new RotateTool("RotateTool", rotateController);
            toolManager.addTool(rotateTool);

            solutionController = new SolutionController(solution, solutionPanel, verticalObjectEditor);

            //Initialize the windows
            mainForm.initialize(this);
            drawingWindowController.initialize(this, eventManager, pluginManager.RendererPlugin, AnomalyConfig.ConfigFile);
            movePanel.initialize(moveController);
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
                mainForm.showDockContent(verticalObjectEditor);
                mainForm.showDockContent(solutionPanel);
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

        void win32Timer_MessageReceived(ref WinMsg message)
        {
            Message msg = Message.Create(message.hwnd, message.message, message.wParam, message.lParam);
            ManualMessagePump.pumpMessage(ref msg);
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
            mainForm.Activate();
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
            mainObjectEditor.setEditInterface(editInterface, null, null);
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
            changeScene(solution.createCurrentProject());
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
            Log.ImportantInfo("Scene saved to {0}.", filename);
        }
        
        /// <summary>
        /// Put the editor into static mode. This allows full editing privlidges
        /// of all objects.
        /// </summary>
        public void setStaticMode()
        {
            movePanel.Enabled = true;
            rotatePanel.Enabled = true;
            sceneController.setDynamicMode(false);
            sceneController.destroyScene();
            sceneController.createScene();
            toolManager.setEnabled(true);
            verticalObjectEditor.Enabled = true;
            solutionPanel.Enabled = true;
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
            sceneController.setDynamicMode(true);
            sceneController.destroyScene();
            sceneController.createScene();
            verticalObjectEditor.Enabled = false;
            solutionPanel.Enabled = false;
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
            throw new NotImplementedException();
            //XmlTextReader textReader = null;
            //try
            //{
            //    textReader = new XmlTextReader(filename);
            //    SimObjectManagerDefinition managerDefintion = simObjectController.getSimObjectManagerDefinition();
            //    instanceBuilder.loadInstances(textReader, managerDefintion);
            //    simObjectController.setSceneManagerDefintion(managerDefintion);
            //    sceneController.destroyScene();
            //    sceneController.createScene();
            //}
            //catch(Exception e)
            //{
            //    Log.Default.printException(e);
            //    MessageBox.Show(mainForm, String.Format("An exception occured when loading the instances:\n{0}.", e.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    if (textReader != null)
            //    {
            //        textReader.Close();
            //    }
            //}
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
            if (persistString == consoleWindow.GetType().ToString())
            {
                return consoleWindow;
            }
            if (persistString == verticalObjectEditor.GetType().ToString())
            {
                return verticalObjectEditor;
            }
            if (persistString == solutionPanel.GetType().ToString())
            {
                return solutionPanel;
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

        public Solution Solution
        {
            get
            {
                return solution;
            }
        }

        #endregion Properties
    }
}

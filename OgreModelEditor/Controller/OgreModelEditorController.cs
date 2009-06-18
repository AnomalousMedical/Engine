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
        private SystemTimer systemTimer;
        private EventManager eventManager;
        private InputHandler inputHandler;
        private EventUpdateListener eventUpdate;

        //GUI
        private DrawingWindow hiddenEmbedWindow;
        private OgreModelEditorMain mainForm;
        private ObjectEditorForm objectEditor = new ObjectEditorForm();
        private ConsoleWindow consoleWindow = new ConsoleWindow();

        //Controller
        private DrawingWindowController drawingWindowController = new DrawingWindowController();
        private ModelController modelController;

        //Scene
        private SimScene scene;
        private String lastFileName = null;

        //Resources
        private ResourceManager resourceManager;
        private XmlSaver xmlSaver = new XmlSaver();
        private ResourceManager emptyResourceManager;

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
                pluginManager.PlatformPlugin.destroyInputHandler(inputHandler);
            }
            if (systemTimer != null)
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

            Engine.Resources.Resource.ResourceRoot = null;

            //Initailize plugins
            hiddenEmbedWindow = new DrawingWindow();
            pluginManager = new PluginManager(OgreModelEditorConfig.ConfigFile);
            pluginManager.OnConfigureDefaultWindow = createWindow;
            pluginManager.addPluginAssembly(typeof(OgreInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(Win32PlatformPlugin).Assembly);
            pluginManager.initializePlugins();
            pluginManager.RendererPlugin.PrimaryWindow.setEnabled(false);

            if (File.Exists("OgreModelEditor.zip"))
            {
                Log.Default.sendMessage("Found OgreModelEditor.zip. Will be able to see debug shaders", LogLevel.ImportantInfo, "OgreModelEditor");
                OgreResourceGroupManager.getInstance().addResourceLocation("OgreModelEditor.zip", "Zip", "ModelEditor", true);
                OgreResourceGroupManager.getInstance().initializeAllResourceGroups();
            }

            emptyResourceManager = pluginManager.createEmptyResourceManager();
            if (!File.Exists(OgreModelEditorConfig.DocRoot + "/resources.xml"))
            {
                resourceManager = pluginManager.createEmptyResourceManager();
            }
            else
            {
                XmlTextReader textReader = new XmlTextReader(OgreModelEditorConfig.DocRoot + "/resources.xml");
                resourceManager = xmlSaver.restoreObject(textReader) as ResourceManager;
                if (resourceManager == null)
                {
                    resourceManager = pluginManager.createEmptyResourceManager();
                }
                pluginManager.PrimaryResourceManager.changeResourcesToMatch(resourceManager);
                pluginManager.PrimaryResourceManager.forceResourceRefresh();
                textReader.Close();
            }

            //Create the GUI
            mainForm = new OgreModelEditorMain();

            //Intialize the platform
            systemTimer = pluginManager.PlatformPlugin.createTimer();
            mainTimer = new UpdateTimer(systemTimer, new WindowsFormsUpdate());
            inputHandler = pluginManager.PlatformPlugin.createInputHandler(mainForm, false, false, false);
            eventManager = new EventManager(inputHandler);
            eventUpdate = new EventUpdateListener(eventManager);
            mainTimer.addFixedUpdateListener(eventUpdate);
            pluginManager.setPlatformInfo(mainTimer, eventManager);

            //Initialize controllers
            drawingWindowController.initialize(this, eventManager, pluginManager.RendererPlugin, OgreModelEditorConfig.ConfigFile);
            modelController = new ModelController(this);

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
        }

        public void start()
        {
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
            
            mainTimer.startLoop();
        }

        public void shutdown()
        {
            mainForm.saveWindows(OgreModelEditorConfig.DocRoot + "/windows.ini");
            mainTimer.stopLoop();
            drawingWindowController.destroyCameras();
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
        }

        public void editExternalResources()
        {
            objectEditor.EditorPanel.setEditInterface(resourceManager.getEditInterface());
            objectEditor.ShowDialog(mainForm);
            objectEditor.EditorPanel.clearEditInterface();
            pluginManager.PrimaryResourceManager.changeResourcesToMatch(resourceManager);
            pluginManager.PrimaryResourceManager.forceResourceRefresh();
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
                pluginManager.PrimaryResourceManager.changeResourcesToMatch(emptyResourceManager);
                pluginManager.PrimaryResourceManager.forceResourceRefresh();
                OgreResourceGroupManager groupManager = OgreResourceGroupManager.getInstance();
                groupManager.destroyResourceGroup(dir);
                groupManager.initializeAllResourceGroups();
                pluginManager.PrimaryResourceManager.changeResourcesToMatch(resourceManager);
                pluginManager.PrimaryResourceManager.forceResourceRefresh();
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
    }
}

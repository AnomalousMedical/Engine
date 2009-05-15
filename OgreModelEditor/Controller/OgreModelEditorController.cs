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

namespace OgreModelEditor
{
    class OgreModelEditorController : IDisposable, UpdateListener, IDockProvider
    {
        #region Fields

        //Engine
        private PluginManager pluginManager;
        private LogFileListener logListener;

        //Platform
        private UpdateTimer mainTimer;
        private EventManager eventManager;
        private InputHandler inputHandler;
        private EventUpdateListener eventUpdate;

        //GUI
        private DrawingWindow hiddenEmbedWindow;
        private OgreModelEditorMain mainForm;
        private ObjectEditorForm objectEditor = new ObjectEditorForm();

        //Controller
        private DrawingWindowController drawingWindowController = new DrawingWindowController();

        //Scene
        private SimScene scene;
        private String lastFileName = null;
        private GenericSimObjectDefinition simObjectDefinition;
        private EntityDefinition entityDefintion;
        private SceneNodeDefinition nodeDefinition;
        private SimObjectBase currentSimObject;
        private String entityMaterialName;
        private Entity entity;

        //Resources
        private ResourceManager resourceManager;
        private XmlSaver xmlSaver = new XmlSaver();
        private ResourceManager emptyResourceManager;

        #endregion Fields

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

            OgreModelEditorConfig.save();
            logListener.closeLogFile();
        }

        public void initialize()
        {
            //Create the log.
            logListener = new LogFileListener();
            logListener.openLogFile(OgreModelEditorConfig.DocRoot + "/log.log");
            Log.Default.addLogListener(logListener);

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

            Engine.Resources.Resource.ResourceRoot = null;
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
            mainTimer = pluginManager.PlatformPlugin.createTimer();
            inputHandler = pluginManager.PlatformPlugin.createInputHandler(mainForm, false, false, false);
            eventManager = new EventManager(inputHandler);
            eventUpdate = new EventUpdateListener(eventManager);
            mainTimer.addFixedUpdateListener(eventUpdate);
            mainTimer.addFullSpeedUpdateListener(this);
            pluginManager.setPlatformInfo(mainTimer, eventManager);

            //Initialize controllers
            drawingWindowController.initialize(this, eventManager, pluginManager.RendererPlugin, OgreModelEditorConfig.ConfigFile);
            drawingWindowController.createOneWaySplit();

            //Initialize GUI
            mainForm.initialize(this);
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

            simObjectDefinition = new GenericSimObjectDefinition("EntitySimObject");
            simObjectDefinition.Enabled = true;
            entityDefintion = new EntityDefinition("Entity");
            nodeDefinition = new SceneNodeDefinition("EntityNode");
            nodeDefinition.addMovableObjectDefinition(entityDefintion);
            simObjectDefinition.addElement(nodeDefinition);

            scene = sceneDefiniton.createScene();
            drawingWindowController.createCameras(mainTimer, scene);
            
            mainTimer.startLoop();
        }

        public void shutdown()
        {
            mainTimer.stopLoop();
            drawingWindowController.destroyCameras();
            if (currentSimObject != null)
            {
                currentSimObject.Dispose();
            }
            scene.Dispose();
        }

        public void openModel(String path)
        {
            OgreResourceGroupManager groupManager = OgreResourceGroupManager.getInstance();
            if (currentSimObject != null)
            {
                currentSimObject.Dispose();
                String lastDir = Path.GetDirectoryName(lastFileName);
                groupManager.removeResourceLocation(lastDir, "LoadedModel");
                groupManager.initializeAllResourceGroups();
            }

            lastFileName = path;
            String dir = Path.GetDirectoryName(path);
            groupManager.addResourceLocation(dir, "FileSystem", "LoadedModel", true);
            groupManager.initializeAllResourceGroups();
            String filename = Path.GetFileName(path);
            entityDefintion.MeshName = filename;
            currentSimObject = simObjectDefinition.register(scene.getDefaultSubScene());
            scene.buildScene();
            entity = ((SceneNodeElement)currentSimObject.getElement("EntityNode")).getEntity(new Identifier("EntitySimObject", "Entity"));
            entityMaterialName = entity.getSubEntity(0).getMaterialName();
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
            if (currentSimObject != null)
            {
                currentSimObject.Dispose();
            }
            pluginManager.PrimaryResourceManager.changeResourcesToMatch(emptyResourceManager);
            pluginManager.PrimaryResourceManager.forceResourceRefresh();
            OgreResourceGroupManager groupManager = OgreResourceGroupManager.getInstance();
            groupManager.destroyResourceGroup("LoadedModel");
            groupManager.initializeAllResourceGroups();
            pluginManager.PrimaryResourceManager.changeResourcesToMatch(resourceManager);
            pluginManager.PrimaryResourceManager.forceResourceRefresh();
            String path = Path.GetDirectoryName(lastFileName);
            groupManager.addResourceLocation(path, "FileSystem", "LoadedModel", true);
            groupManager.initializeAllResourceGroups();
            if (currentSimObject != null)
            {
                currentSimObject = simObjectDefinition.register(scene.getDefaultSubScene());
                scene.buildScene();
                entity = ((SceneNodeElement)currentSimObject.getElement("EntityNode")).getEntity(new Identifier("EntitySimObject", "Entity"));
                entityMaterialName = entity.getSubEntity(0).getMaterialName();
            }
        }

        public void setBinormalDebug()
        {
            entity.setMaterialName("BinormalDebug");
        }

        public void setTangentDebug()
        {
            entity.setMaterialName("TangentDebug");
        }

        public void setNormalDebug()
        {
            entity.setMaterialName("NormalDebug");
        }

        public void setNormalMaterial()
        {
            entity.setMaterialName(entityMaterialName);
        }

        public void buildTangentVectors()
        {
            using (MeshPtr mesh = entity.getMesh())
            {
                mesh.Value.buildTangentVectors();
            }
        }

        public unsafe void buildBinormalVectors()
        {
            using (MeshPtr mesh = entity.getMesh())
            {
                SubMesh subMesh = mesh.Value.getSubMesh(0);
                VertexData vertexData = subMesh.vertexData;
                VertexDeclaration vertexDeclaration = vertexData.vertexDeclaration;
                VertexBufferBinding vertexBinding = vertexData.vertexBufferBinding;
                VertexElement normalElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_NORMAL);
                VertexElement tangentElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_TANGENT);
                VertexElement binormalElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_BINORMAL);

                uint numVertices = vertexData.vertexCount;
                HardwareVertexBufferSharedPtr normalHardwareBuffer = vertexBinding.getBuffer(normalElement.getSource());
                uint normalVertexSize = normalHardwareBuffer.Value.getVertexSize();
                byte* normalBuffer = (byte*)normalHardwareBuffer.Value.@lock(HardwareBuffer.LockOptions.HBL_READ_ONLY);

                HardwareVertexBufferSharedPtr tangentHardwareBuffer = vertexBinding.getBuffer(tangentElement.getSource());
                uint tangetVertexSize = tangentHardwareBuffer.Value.getVertexSize();
                byte* tangentBuffer = (byte*)tangentHardwareBuffer.Value.@lock(HardwareBuffer.LockOptions.HBL_READ_ONLY);

                HardwareVertexBufferSharedPtr binormalHardwareBuffer = vertexBinding.getBuffer(binormalElement.getSource());
                uint binormalVertexSize = binormalHardwareBuffer.Value.getVertexSize();
                byte* binormalBuffer = (byte*)binormalHardwareBuffer.Value.@lock(HardwareBuffer.LockOptions.HBL_NORMAL);

                Vector3* normal;
                Vector3* tangent;
                Vector3* binormal;

                for (int i = 0; i < numVertices; ++i)
                {
                    normalElement.baseVertexPointerToElement(normalBuffer, (float**)&normal);
                    tangentElement.baseVertexPointerToElement(tangentBuffer, (float**)&tangent);
                    binormalElement.baseVertexPointerToElement(binormalBuffer, (float**)&binormal);

                    *binormal = normal->cross(ref *tangent) * -1.0f;

                    normalBuffer += normalVertexSize;
                    tangentBuffer += tangetVertexSize;
                    binormalBuffer += binormalVertexSize;
                }

                normalHardwareBuffer.Value.unlock();
                tangentHardwareBuffer.Value.unlock();
                binormalHardwareBuffer.Value.unlock();

                normalHardwareBuffer.Dispose();
                tangentHardwareBuffer.Dispose();
                binormalHardwareBuffer.Dispose();
            }
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

        #region UpdateListener Members

        public void sendUpdate(Clock clock)
        {
            Application.DoEvents();
        }

        public void loopStarting()
        {

        }

        public void exceededMaxDelta()
        {

        }

        #endregion

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

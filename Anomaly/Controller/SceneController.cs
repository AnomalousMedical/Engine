using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Engine.Saving.XMLSaver;
using Engine.ObjectManagement;
using Engine.Saving;
using System.IO;
using Engine;
using Engine.Resources;
using Editor;

namespace Anomaly
{
    /// <summary>
    /// This delegate is called when a new scene is loaded.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="scene"></param>
    delegate void SceneLoaded(SceneController controller, SimScene scene);

    /// <summary>
    /// This delegate is called when a scene is about to unload.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="scene"></param>
    delegate void SceneUnloading(SceneController controller, SimScene scene);

    /// <summary>
    /// This delegate is called when a scene has unloaded and is destroyed.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="scene"></param>
    delegate void SceneUnloaded(SceneController controller);

    class SceneController
    {
        private SimScene scene;
        private ObjectEditorForm objectEditor = new ObjectEditorForm();
        private AnomalyController controller;

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

        public SceneController()
        {

        }

        public void initialize(AnomalyController controller)
        {
            this.controller = controller;
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

        public void setupScene()
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
            if (OnSceneLoaded != null)
            {
                OnSceneLoaded.Invoke(this, scene);
            }
        }

        public void setupResources()
        {
            ResourceManager secondaryResources;
            if (!File.Exists(AnomalyConfig.DocRoot + "/resources.xml"))
            {
                secondaryResources = controller.PluginManager.createSecondaryResourceManager();

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

            controller.PluginManager.PrimaryResourceManager.changeResourcesToMatch(secondaryResources);
            controller.PluginManager.PrimaryResourceManager.forceResourceRefresh();
        }

        public void destroyScene()
        {
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
    }
}

using Engine;
using Engine.ObjectManagement;
using Engine.Resources;
using Engine.Saving.XMLSaver;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Anomalous.GameApp
{
    /// <summary>
    /// This delegate is called when the SceneController fires an event.
    /// </summary>
    /// <param name="controller">The controller that fired the event.</param>
    /// <param name="scene">The scene for the event.</param>
    public delegate void SceneControllerEvent(SceneController controller, SimScene scene);

    public class SceneController
    {
        private SimScene scene;
        private ScenePackage scenePackage;
        private bool dynamicMode = false;
        private PluginManager pluginManager;
        private XmlSaver xmlSaver = new XmlSaver();
        private ResourceManager sceneResourceManager;
        private SimObjectManager currentSimObjects;

        #region Events

        /// <summary>
        /// This event is fired before a scene loads.
        /// </summary>
        public event SceneControllerEvent OnSceneLoading;

        /// <summary>
        /// This event is fired when a scene is loaded.
        /// </summary>
        public event SceneControllerEvent OnSceneLoaded;

        /// <summary>
        /// This event is fired when a scene starts unloading.
        /// </summary>
        public event SceneControllerEvent OnSceneUnloading;

        /// <summary>
        /// This event is fired when a scene has finished unloading.
        /// </summary>
        public event SceneControllerEvent OnSceneUnloaded;

        #endregion Events

        public SceneController(PluginManager pluginManager)
        {
            this.pluginManager = pluginManager;
            sceneResourceManager = pluginManager.createLiveResourceManager("Scene");
        }

        /// <summary>
        /// Remove all resources from the scene resource manager. This will force anything loaded to unload.
        /// </summary>
        public void clearResources()
        {
            sceneResourceManager.changeResourcesToMatch(pluginManager.createScratchResourceManager());
            sceneResourceManager.initializeResources();
        }

        public SimSceneDefinition getSceneDefinition()
        {
            return scenePackage.SceneDefinition;
        }

        public void loadSceneDefinition(String fileName)
        {
            foreach (var step in loadSceneDefinitionCo(fileName)) { }
        }

        public IEnumerable<SceneBuildStatus> loadSceneDefinitionCo(String fileName)
        {
            ScenePackage scenePackage;
            using (var reader = XmlReader.Create(VirtualFileSystem.Instance.openStream(fileName, Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read)))
            {
                scenePackage = (ScenePackage)xmlSaver.restoreObject(reader);
            }
            return loadSceneCo(scenePackage);
        }

        public void loadScene(ScenePackage scenePackage, SceneBuildOptions options = SceneBuildOptions.SingleUseDefinitions)
        {
            foreach (var step in loadSceneCo(scenePackage, options)) { }
        }

        /// <summary>
        /// Load the scene in the given ScenePackage.
        /// </summary>
        /// <param name="scenePackage">The ScenePackage to load.</param>
        public IEnumerable<SceneBuildStatus> loadSceneCo(ScenePackage scenePackage, SceneBuildOptions options = SceneBuildOptions.SingleUseDefinitions)
        {
            this.scenePackage = scenePackage;
            yield return new SceneBuildStatus()
            {
                Message = "Setting up Resources"
            };
            sceneResourceManager.changeResourcesToMatch(scenePackage.ResourceManager);
            sceneResourceManager.initializeResources();

            scene = scenePackage.SceneDefinition.createScene();
            scene.Scope = pluginManager.ServiceProvider.CreateScope();
            if (OnSceneLoading != null)
            {
                OnSceneLoading.Invoke(this, scene);
            }
            currentSimObjects = scenePackage.SimObjectManagerDefinition.createSimObjectManager(scene.getDefaultSubScene());
            if (dynamicMode)
            {
                foreach (var status in scene.buildSceneStatus(options))
                {
                    yield return status;
                }
            }
            else
            {
                scene.buildStaticScene();
            }
            if (OnSceneLoaded != null)
            {
                OnSceneLoaded.Invoke(this, scene);
            }
            foreach (DebugInterface debugInterface in pluginManager.DebugInterfaces)
            {
                debugInterface.createDebugInterface(pluginManager.RendererPlugin, scene.getDefaultSubScene());
            }
        }

        public void destroyScene()
        {
            if (scene != null)
            {
                foreach (DebugInterface debugInterface in pluginManager.DebugInterfaces)
                {
                    debugInterface.destroyDebugInterface(pluginManager.RendererPlugin, scene.getDefaultSubScene());
                }
                if (OnSceneUnloading != null)
                {
                    OnSceneUnloading.Invoke(this, scene);
                }
                currentSimObjects.Dispose();
                currentSimObjects = null;
                scene.Scope.Dispose();
                scene.Dispose();
                scene = null;
                if (OnSceneUnloaded != null)
                {
                    OnSceneUnloaded.Invoke(this, null);
                }
            }
        }

        /// <summary>
        /// Set the current mode of any scene constructions. Pass true for
        /// dynamic mode and false for static mode.
        /// </summary>
        /// <param name="dynamicMode">True to enable dynamic mode. False to use static mode.</param>
        public void setDynamicMode(bool dynamicMode)
        {
            this.dynamicMode = dynamicMode;
        }

        /// <summary>
        /// Draw the debug information for the current scene.
        /// </summary>
        /// <param name="debugSurface">The DebugDrawingSurface to render onto.</param>
        public void drawDebugInformation()
        {
            if (scene != null && scene.getDefaultSubScene() != null)
            {
                foreach (DebugInterface debugInterface in pluginManager.DebugInterfaces)
                {
                    debugInterface.renderDebug(scene.getDefaultSubScene());
                }
            }
        }

        /// <summary>
        /// Determine if the scene is in dynamic mode.
        /// </summary>
        /// <returns>True if dynamic mode is enabled.</returns>
        public bool isDynamicMode()
        {
            return dynamicMode;
        }

        /// <summary>
        /// Get the current scene.
        /// </summary>
        public SimScene CurrentScene
        {
            get
            {
                return scene;
            }
        }

        public void addSimObject(SimObjectBase simObject)
        {
            currentSimObjects.addSimObject(simObject);
        }

        public SimObject getSimObject(String name)
        {
            SimObjectBase simObject;
            currentSimObjects.tryGetSimObject(name, out simObject);
            return simObject;
        }

        public IEnumerable<SimObjectBase> SimObjects
        {
            get
            {
                return currentSimObjects.SimObjects;
            }
        }
    }
}

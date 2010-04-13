using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Editor;

namespace Anomaly
{
    public class SimObjectController
    {
        private SimObjectManagerDefinition simObjectManagerDefiniton;
        private SimObjectManager simObjectManager;
        private AnomalyController controller;
        private SimSubScene subScene;

        public SimObjectController(AnomalyController controller)
        {
            this.controller = controller;
            controller.SceneController.OnSceneLoading += SceneController_OnSceneLoading;
            controller.SceneController.OnSceneUnloading += SceneController_OnSceneUnloading;
        }

        /// <summary>
        /// Create a new SimObject and add it.
        /// </summary>
        /// <param name="definition">The definition to create.</param>
        /// <returns>A SimObject if one was created. Not returning one does not indicate an error it just means the sim object hasnt been created yet.</returns>
        public SimObject createSimObject(SimObjectDefinition definition)
        {
            simObjectManagerDefiniton.addSimObject(definition);
            if (subScene != null)
            {
                SimObjectBase instance = definition.register(subScene);
                simObjectManager.addSimObject(instance);
                controller.SceneController.createSimObjects();
                return instance;
            }
            return null;
        }

        /// <summary>
        /// Destroy and remove the SimObject defined by definition.
        /// </summary>
        /// <param name="definition">The definition to remove.</param>
        public void destroySimObject(String name)
        {
            if (subScene != null)
            {
                simObjectManager.destroySimObject(name);
            }
            simObjectManagerDefiniton.removeSimObject(name);
        }

        public SimObject getSimObject(String name)
        {
            if (simObjectManager != null)
            {
                return simObjectManager.getSimObject(name);
            }
            return null;
        }

        internal bool hasSimObject(string name)
        {
            return simObjectManagerDefiniton.hasSimObject(name);
        }

        /// <summary>
        /// Set the SimObjectManagerDefintion to be used by this controller.
        /// This will add the definitions to the UI but will not create the
        /// instances. That will happen when the scene is loaded on the
        /// callback.
        /// </summary>
        /// <param name="definition">The SimObjectManagerDefinition to use.</param>
        public void setSceneManagerDefintion(SimObjectManagerDefinition definition)
        {
            controller.SelectionController.clearSelection();
            simObjectManagerDefiniton = definition;
        }

        public SimObjectManagerDefinition getSimObjectManagerDefinition()
        {
            return simObjectManagerDefiniton;
        }

        /// <summary>
        /// Callback for when the scene is unloading. Will clear all SimObject instances.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="scene"></param>
        private void SceneController_OnSceneUnloading(SceneController controller, SimScene scene)
        {
            if (simObjectManager != null)
            {
                simObjectManager.Dispose();
                simObjectManager = null;
                subScene = null;
            }
        }

        /// <summary>
        /// Callback for when the scene is loading. Will create all instances
        /// and add them to their selectables.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="scene"></param>
        private void SceneController_OnSceneLoading(SceneController controller, SimScene scene)
        {
            this.subScene = scene.getDefaultSubScene();
            simObjectManager = simObjectManagerDefiniton.createSimObjectManager(scene.getDefaultSubScene());
        }
    }
}

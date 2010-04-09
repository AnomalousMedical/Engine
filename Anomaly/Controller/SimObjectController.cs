using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace Anomaly
{
    class SimObjectController
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
            //controller.SelectionController.OnSelectionChanged += new ObjectSelected(SelectionController_OnSelectionChanged);
        }

        /// <summary>
        /// Create a new SimObject and add it.
        /// </summary>
        /// <param name="definition">The definition to create.</param>
        public void createSimObject(SimObjectDefinition definition)
        {
            SimObjectBase instance = definition.register(subScene);
            simObjectManager.addSimObject(instance);
            controller.SceneController.createSimObjects();
            simObjectManagerDefiniton.addSimObject(definition);
            //createSelectable(definition, instance);
        }

        /// <summary>
        /// Destroy and remove the SimObject defined by definition.
        /// </summary>
        /// <param name="definition">The definition to remove.</param>
        public void destroySimObject(SimObjectDefinition definition)
        {
            simObjectManager.destroySimObject(definition.Name);
            simObjectManagerDefiniton.removeSimObject(definition);

            //SelectableSimObject selectable = selectables[definition.Name];
            //selectables.Remove(definition.Name);
            //removeSelectableEditInterface(selectable);
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
            //clearEditInterfaces();
            //selectables.Clear();
            controller.SelectionController.clearSelection();
            simObjectManagerDefiniton = definition;
            //foreach (SimObjectDefinition simObject in simObjectManagerDefiniton.getDefinitionIter())
            //{
            //    createSelectable(simObject, null);
            //}
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
            //foreach (SelectableSimObject selectable in selectables.Values)
            //{
            //    String name = selectable.Definition.Name;
            //    if (simObjectManager.hasSimObject(name))
            //    {
            //        selectable.Instance = simObjectManager.getSimObject(name);
            //    }
            //    else
            //    {
            //        selectable.Instance = null;
            //    }
            //}
        }
    }
}

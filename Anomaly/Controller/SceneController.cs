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
    /// This delegate is called when the SceneController fires an event.
    /// </summary>
    /// <param name="controller">The controller that fired the event.</param>
    /// <param name="scene">The scene for the event.</param>
    delegate void SceneControllerEvent(SceneController controller, SimScene scene);

    class SceneController
    {
        private SimScene scene;
        private AnomalyController controller;
        private SimSceneDefinition sceneDefinition;
        private bool dynamicMode = false;

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

        public SceneController()
        {

        }

        public void initialize(AnomalyController controller)
        {
            this.controller = controller;
        }

        public SimSceneDefinition getSceneDefinition()
        {
            return sceneDefinition;
        }

        public void setSceneDefinition(SimSceneDefinition sceneDefinition)
        {
            this.sceneDefinition = sceneDefinition;
        }

        public void editScene()
        {
            controller.showObjectEditor(sceneDefinition.getEditInterface());
            destroyScene();
            createScene();
        }

        public void createScene()
        {
            scene = sceneDefinition.createScene();
            if (OnSceneLoading != null)
            {
                OnSceneLoading.Invoke(this, scene);
            }
            createSimObjects();
            if (OnSceneLoaded != null)
            {
                OnSceneLoaded.Invoke(this, scene);
            }
        }

        public void createSimObjects()
        {
            if (dynamicMode)
            {
                scene.buildScene();
            }
            else
            {
                scene.buildStaticScene();
            }
        }

        public void destroyScene()
        {
            if (scene != null)
            {
                if (OnSceneUnloading != null)
                {
                    OnSceneUnloading.Invoke(this, scene);
                }
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
        public void setMode(bool dynamicMode)
        {
            this.dynamicMode = dynamicMode;
        }
    }
}

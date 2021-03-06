﻿using System;
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
using Engine.Renderer;
using Microsoft.Extensions.DependencyInjection;

namespace Anomaly
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
        private AnomalyController controller;
        private SimSceneDefinition sceneDefinition;
        private bool dynamicMode = false;
        private PluginManager pluginManager;

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

        public void createScene()
        {
            scene = sceneDefinition.createScene();
            scene.Scope = pluginManager.GlobalScope.ServiceProvider.CreateScope(); //Disposed in destroyscene
            if (OnSceneLoading != null)
            {
                OnSceneLoading.Invoke(this, scene);
            }
            createSimObjects();
            if (OnSceneLoaded != null)
            {
                OnSceneLoaded.Invoke(this, scene);
            }
            foreach (DebugInterface debugInterface in controller.PluginManager.DebugInterfaces)
            {
                debugInterface.createDebugInterface(controller.PluginManager.RendererPlugin, scene.getDefaultSubScene());
            }
        }

        public void createSimObjects()
        {
            if (dynamicMode)
            {
                scene.buildScene(SceneBuildOptions.None);
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
                foreach (DebugInterface debugInterface in controller.PluginManager.DebugInterfaces)
                {
                    debugInterface.destroyDebugInterface(controller.PluginManager.RendererPlugin, scene.getDefaultSubScene());
                }
                if (OnSceneUnloading != null)
                {
                    OnSceneUnloading.Invoke(this, scene);
                }
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
                foreach (DebugInterface debugInterface in controller.PluginManager.DebugInterfaces)
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
    }
}

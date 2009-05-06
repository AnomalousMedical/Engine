using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Resources;
using Engine.Saving;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This class provides a SimSceneDefinition, ResourceManager and
    /// SimObjectManagerDefinition in one place that can be easily saved and
    /// restored.
    /// </summary>
    public class ScenePackage : Saveable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ScenePackage()
        {

        }

        /// <summary>
        /// The SimSceneDefinition.
        /// </summary>
        public SimSceneDefinition SceneDefinition { get; set; }

        /// <summary>
        /// The ResourceManager.
        /// </summary>
        public ResourceManager ResourceManager { get; set; }

        /// <summary>
        /// The SimObjectManagerDefinition.
        /// </summary>
        public SimObjectManagerDefinition SimObjectManagerDefinition { get; set; }

        #region Saveable Members

        private const String SCENE = "Scene";
        private const String RESOURCES = "Resources";
        private const String SIM_OBJECTS = "SimObjects";

        /// <summary>
        /// Load Constructor.
        /// </summary>
        /// <param name="info">The load info.</param>
        private ScenePackage(LoadInfo info)
        {
            SceneDefinition = info.GetValue<SimSceneDefinition>(SCENE);
            ResourceManager = info.GetValue<ResourceManager>(RESOURCES);
            SimObjectManagerDefinition = info.GetValue<SimObjectManagerDefinition>(SIM_OBJECTS);
        }

        /// <summary>
        /// Get the info to save for the implementing class.
        /// </summary>
        /// <param name="info">The SaveInfo class to save into.</param>
        public void getInfo(SaveInfo info)
        {
            info.AddValue(SIM_OBJECTS, SimObjectManagerDefinition);
            info.AddValue(SCENE, SceneDefinition);
            info.AddValue(RESOURCES, ResourceManager);
        }

        #endregion
    }
}

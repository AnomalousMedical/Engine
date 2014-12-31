using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgrePlugin
{
    /// <summary>
    /// A container for lights.
    /// </summary>
    class LightContainer : MovableObjectContainerBase<Light>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="defintionName">The definition name.</param>
        /// <param name="light">The light name.</param>
        public LightContainer(String defintionName, Light light)
            :base(light, defintionName)
        {
            
        }

        /// <summary>
        /// Create the appropriate MovableObjectDefintion for this
        /// MovableObject. The definition name to use is avaliable as
        /// definitionName.
        /// </summary>
        /// <returns>A new MovableObjectDefintion configured appropriatly.</returns>
        public override MovableObjectDefinition createDefinition()
        {
            return new LightDefinition(definitionName, movable);
        }

        /// <summary>
        /// Destroy the MovableObject.
        /// </summary>
        /// <param name="sceneManager">The scene to remove the object from.</param>
        public override void destroy(OgreSceneManager sceneManager)
        {
            sceneManager.SceneManager.destroyLight(movable);
        }
    }
}

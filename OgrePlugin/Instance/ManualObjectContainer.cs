using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;

namespace OgrePlugin
{
    /// <summary>
    /// A container for ManualObjects.
    /// </summary>
    class ManualObjectContainer : MovableObjectContainerBase<ManualObject>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="defintionName">The definition name.</param>
        /// <param name="manualObject">The manualObject name.</param>
        public ManualObjectContainer(String defintionName, ManualObject manualObject)
            :base(manualObject, defintionName)
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
            return new ManualObjectDefinition(definitionName, movable);
        }

        /// <summary>
        /// Destroy the MovableObject.
        /// </summary>
        /// <param name="sceneManager">The scene to remove the object from.</param>
        public override void destroy(OgreSceneManager sceneManager)
        {
            sceneManager.SceneManager.destroyManualObject(movable);
        }
    }
}

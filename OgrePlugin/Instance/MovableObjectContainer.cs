using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgrePlugin
{
    /// <summary>
    /// This class provides the bookeeping for a MovableObject within a
    /// SceneNodeElement.
    /// </summary>
    interface MovableObjectContainer
    {
        /// <summary>
        /// Create the appropriate MovableObjectDefintion for this
        /// MovableObject. The definition name to use is avaliable as
        /// definitionName.
        /// </summary>
        /// <returns>A new MovableObjectDefintion configured appropriatly.</returns>
        MovableObjectDefinition createDefinition();

        /// <summary>
        /// Destroy the MovableObject.
        /// </summary>
        /// <param name="sceneManager">The scene to remove the object from.</param>
        void destroy(OgreSceneManager sceneManager);

        /// <summary>
        /// Get the MovableObject in this container.
        /// </summary>
        MovableObject MovableObject
        {
            get;
        }

        /// <summary>
        /// Get the name of the definition of the object.
        /// </summary>
        String DefinitionName
        {
            get;
        }
    }
}

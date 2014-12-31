using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgrePlugin
{
    /// <summary>
    /// This is a container for an entity.
    /// </summary>
    class EntityContainer : MovableObjectContainerBase<Entity>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="defintionName">The definition name.</param>
        /// <param name="entity">The entity name.</param>
        public EntityContainer(String defintionName, Entity entity)
            :base(entity, defintionName)
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
            return new EntityDefinition(definitionName, movable);
        }

        /// <summary>
        /// Destroy the MovableObject.
        /// </summary>
        /// <param name="sceneManager">The scene to remove the object from.</param>
        public override void destroy(OgreSceneManager sceneManager)
        {
            sceneManager.SceneManager.destroyEntity(movable);
        }
    }
}

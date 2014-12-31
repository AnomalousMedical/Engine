using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgrePlugin
{
    abstract class MovableObjectContainerBase<MovableType> : MovableObjectContainer
        where MovableType : MovableObject
    {
        protected String definitionName;
        protected MovableType movable;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="definitionName">The name of the defintion when creating a defintion. Accessible to subclasses.</param>
        public MovableObjectContainerBase(MovableType movable, String definitionName)
        {
            this.definitionName = definitionName;
            this.movable = movable;
        }

        /// <summary>
        /// Create the appropriate MovableObjectDefintion for this
        /// MovableObject. The definition name to use is avaliable as
        /// definitionName.
        /// </summary>
        /// <returns>A new MovableObjectDefintion configured appropriatly.</returns>
        public abstract MovableObjectDefinition createDefinition();

        /// <summary>
        /// Destroy the MovableObject.
        /// </summary>
        /// <param name="sceneManager">The scene to remove the object from.</param>
        public abstract void destroy(OgreSceneManager sceneManager);

        /// <summary>
        /// Get the MovableObject in this container.
        /// </summary>
        public MovableObject MovableObject
        {
            get
            {
                return movable;
            }
        }

        /// <summary>
        /// Get the name of the definition of the object.
        /// </summary>
        public String DefinitionName
        {
            get
            {
                return definitionName;
            }
        }
    }
}

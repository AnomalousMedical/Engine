using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace PhysXPlugin
{
    /// <summary>
    /// This class defines and builds PhysActors.
    /// </summary>
    class PhysActorDefinition : PhysComponentDefinition
    {
        #region Fields

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Normal constructor. Takes a name and a PhysFactory to build with.
        /// </summary>
        /// <param name="name">The name of the actor.</param>
        /// <param name="factory">A factory to build objects with.</param>
        public PhysActorDefinition(String name, PhysFactory factory)
            :base(name, factory)
        {

        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Register this component with its factory so it can be built.
        /// </summary>
        /// <param name="instance">The SimObject that will get the newly created component.</param>
        public override void register(SimObject instance)
        {
            factory.addActorDefinition(instance, this);
        }

        /// <summary>
        /// Create a new component normally as a part of scene and add it to instance.
        /// </summary>
        /// <param name="instance">The SimObject to add the component to.</param>
        /// <param name="scene">The PhysSceneManager to create the product with.</param>
        public override void createProduct(SimObject instance, PhysXSceneManager scene)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a new component staticly as a part of scene and add it to instance.
        /// </summary>
        /// <param name="instance">The SimObject to add the component to.</param>
        /// <param name="scene">The PhysSceneManager to create the product with.</param>
        public override void createStaticProduct(SimObject instance, PhysXSceneManager scene)
        {
            throw new NotImplementedException();
        }

        #endregion Functions
    }
}

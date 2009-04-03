using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace PhysXPlugin
{
    /// <summary>
    /// This is an abstract base class for all components built by the PhysXPlugin
    /// </summary>
    abstract class PhysComponentDefinition : SimComponentDefinition
    {
        #region Fields

        protected PhysFactory factory;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="factory">The factory to use to build the component.</param>
        public PhysComponentDefinition(String name, PhysFactory factory)
            :base(name)
        {
            this.factory = factory;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Create a new component normally as a part of scene and add it to instance.
        /// </summary>
        /// <param name="instance">The SimObject to add the component to.</param>
        /// <param name="scene">The PhysSceneManager to create the product with.</param>
        public abstract void createProduct(SimObject instance, PhysSceneManager scene);

        /// <summary>
        /// Create a new component staticly as a part of scene and add it to instance.
        /// </summary>
        /// <param name="instance">The SimObject to add the component to.</param>
        /// <param name="scene">The PhysSceneManager to create the product with.</param>
        public abstract void createStaticProduct(SimObject instance, PhysSceneManager scene);

        #endregion Functions
    }
}

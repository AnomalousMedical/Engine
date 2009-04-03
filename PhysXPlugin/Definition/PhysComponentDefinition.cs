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
    public abstract class PhysComponentDefinition : SimComponentDefinition
    {
        #region Fields

        internal PhysFactory factory; //Must be internal to shut up compiler

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="factory">The factory to use to build the component.</param>
        internal PhysComponentDefinition(String name, PhysFactory factory)
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
        internal abstract void createProduct(SimObject instance, PhysXSceneManager scene);

        /// <summary>
        /// Create a new component staticly as a part of scene and add it to instance.
        /// </summary>
        /// <param name="instance">The SimObject to add the component to.</param>
        /// <param name="scene">The PhysSceneManager to create the product with.</param>
        internal abstract void createStaticProduct(SimObject instance, PhysXSceneManager scene);

        #endregion Functions
    }
}

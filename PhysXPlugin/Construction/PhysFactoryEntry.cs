using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace PhysXPlugin
{
    /// <summary>
    /// This is an entry in the list of items to construct for the PhysFactory.
    /// </summary>
    /// <typeparam name="T">The type of the entry.</typeparam>
    class PhysFactoryEntry
    {
        #region Fields

        private SimObject instance;
        PhysElementDefinition definition;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="instance">The SimObject that will have the product constructed added to it.</param>
        /// <param name="definition">The definition class that will build part of the sim object.</param>
        public PhysFactoryEntry(SimObject instance, PhysElementDefinition definition)
        {
            this.instance = instance;
            this.definition = definition;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Build the product normally.
        /// </summary>
        /// <param name="scene">The scene to add the product to.</param>
        public void createProduct(PhysXSceneManager scene)
        {
            definition.createProduct(instance, scene);
        }

        /// <summary>
        /// Build the static version of the product.
        /// </summary>
        /// <param name="scene">The scene to add the product to.</param>
        public void createStaticProduct(PhysXSceneManager scene)
        {
            definition.createStaticProduct(instance, scene);
        }

        #endregion Functions
    }
}

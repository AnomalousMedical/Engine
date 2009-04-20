using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace OgrePlugin
{
    /// <summary>
    /// This is a single entry in the OgreFactory.
    /// </summary>
    class OgreFactoryEntry
    {
        #region Fields

        private SimObject instance;
        SceneNodeDefinition definition;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="instance">The SimObject that will have the product constructed added to it.</param>
        /// <param name="definition">The definition class that will build part of the sim object.</param>
        public OgreFactoryEntry(SimObject instance, SceneNodeDefinition definition)
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
        public void createProduct(OgreSceneManager scene)
        {
            definition.createProduct(instance, scene);
        }

        /// <summary>
        /// Build the static version of the product.
        /// </summary>
        /// <param name="scene">The scene to add the product to.</param>
        public void createStaticProduct(OgreSceneManager scene)
        {
            definition.createStaticProduct(instance, scene);
        }

        #endregion Functions
    }
}

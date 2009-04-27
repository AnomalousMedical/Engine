using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace Engine
{
    class BehaviorFactoryEntry
    {
        #region Fields

        private SimObject instance;
        private BehaviorDefinition definition;
        private Behavior createdBehavior;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="instance">The SimObject that will have the product constructed added to it.</param>
        /// <param name="definition">The definition class that will build part of the sim object.</param>
        public BehaviorFactoryEntry(SimObject instance, BehaviorDefinition definition)
        {
            this.instance = instance;
            this.definition = definition;
            this.createdBehavior = null;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Build the product normally.
        /// </summary>
        /// <param name="scene">The scene to add the product to.</param>
        public void createProduct(BehaviorManager scene)
        {
            createdBehavior = definition.createProduct(instance, scene);
        }

        /// <summary>
        /// Build the static version of the product.
        /// </summary>
        /// <param name="scene">The scene to add the product to.</param>
        public void createStaticProduct(BehaviorManager scene)
        {
            definition.createStaticProduct(instance, scene);
        }

        /// <summary>
        /// Called during the linkupProducts phase. This will call the constructed behavior function.
        /// </summary>
        public void linkupProducts()
        {
            if (createdBehavior != null)
            {
                createdBehavior.callConstructed();
            }
        }

        #endregion Functions
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace Engine
{
    /// <summary>
    /// This class is a single entry in the BehaviorFactory. It tracks what
    /// SimObject a behavior will be built into and the created behavior.
    /// </summary>
    class BehaviorFactoryEntry
    {
        private SimObjectBase instance;
        private BehaviorDefinition definition;
        private Behavior createdBehavior;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="instance">The SimObject that will have the product constructed added to it.</param>
        /// <param name="definition">The definition class that will build part of the sim object.</param>
        public BehaviorFactoryEntry(SimObjectBase instance, BehaviorDefinition definition)
        {
            this.instance = instance;
            this.definition = definition;
            this.createdBehavior = null;
        }

        /// <summary>
        /// Build the product normally.
        /// </summary>
        /// <param name="scene">The scene to add the product to.</param>
        public void createProduct(BehaviorManager scene)
        {
            createdBehavior = definition.createProduct(instance, scene);
        }

        /// <summary>
        /// Called during the linkupProducts phase. This will call the constructed behavior function.
        /// </summary>
        public void constructed()
        {
            if (createdBehavior != null)
            {
                createdBehavior.callConstructed();
            }
        }

        /// <summary>
        /// Called during the linkupProducts phase. This will call the link behavior function.
        /// </summary>
        public void linkupProducts()
        {
            if (createdBehavior != null && createdBehavior.Valid)
            {
                createdBehavior.callLink();
            }
        }

        public void _unanticipatedBlacklistError(Exception e)
        {
            if (createdBehavior != null)
            {
                createdBehavior._unanticipatedBlacklistError(e);
            }
        }

        public Behavior CreatedBehavior
        {
            get
            {
                return createdBehavior;
            }
        }
    }
}

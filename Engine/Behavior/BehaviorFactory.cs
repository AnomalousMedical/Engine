using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace Engine
{
    /// <summary>
    /// This is the factory that builds behaviors.
    /// </summary>
    public class BehaviorFactory : SimElementFactory
    {
        private List<BehaviorFactoryEntry> currentBehaviors = new List<BehaviorFactoryEntry>();
        private BehaviorManager manager;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="manager">The BehaviorManager to add behaviors to.</param>
        public BehaviorFactory(BehaviorManager manager)
        {
            this.manager = manager;
        }


        /// <summary>
        /// Create all products for normal operation currently registered for
        /// construction in this factory.
        /// </summary>
        public void createProducts()
        {
            foreach (BehaviorFactoryEntry entry in currentBehaviors)
            {
                entry.createProduct(manager);
            }
        }

        /// <summary>
        /// Create all products for static mode operation currently registered
        /// for construction in this factory.
        /// </summary>
        public void createStaticProducts()
        {
            
        }

        /// <summary>
        /// This function will be called when all subsystems have created their
        /// products. At this time it is safe to discover objects present in
        /// other subsystems.
        /// </summary>
        public void linkProducts()
        {
            foreach (BehaviorFactoryEntry entry in currentBehaviors)
            {
                try
                {
                    entry.constructed();
                }
                catch (BehaviorBlacklistException)
                {

                }
            }
            foreach (BehaviorFactoryEntry entry in currentBehaviors)
            {
                try
                {
                    entry.linkupProducts();
                }
                catch (BehaviorBlacklistException)
                {

                }
            }
        }

        /// <summary>
        /// This function will clear all definitions in the factory. It will be
        /// called after a construction run has completed by executing
        /// createProducts or createStaticProducts.
        /// </summary>
        public void clearDefinitions()
        {
            currentBehaviors.Clear();
        }

        /// <summary>
        /// Add a BehaviorDefinition to be constructed by this factory.
        /// </summary>
        /// <param name="instance">The SimObject instance to add the behavior to.</param>
        /// <param name="behaviorDefinition">The BehaviorDefinition to build.</param>
        internal void addBehaviorDefinition(SimObjectBase instance, BehaviorDefinition behaviorDefinition)
        {
            currentBehaviors.Add(new BehaviorFactoryEntry(instance, behaviorDefinition));
        }
    }
}

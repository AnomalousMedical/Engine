using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace PhysXPlugin
{
    /// <summary>
    /// This is a factory for PhysX objects.
    /// </summary>
    class PhysFactory : SimComponentFactory
    {
        #region Fields

        private LinkedList<PhysFactoryEntry> currentActors = new LinkedList<PhysFactoryEntry>();
        private PhysXSceneManager targetManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public PhysFactory()
        {

        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Add an actor definition to be built when createProducts is run.
        /// </summary>
        /// <param name="def">The PhysActorDefinition to add.</param>
        public void addActorDefinition(SimObject instance, PhysActorDefinition def)
        {
            currentActors.AddLast(new PhysFactoryEntry(instance, def));
        }

        /// <summary>
        /// Set the target PhysComponentManager that the PhysX objects will be
        /// added to.
        /// </summary>
        /// <param name="manager">The manager to add objects to.</param>
        public void setTargetManager(PhysXSceneManager manager)
        {
            targetManager = manager;
        }

        /// <summary>
        /// Set the target manager back to undefined.
        /// </summary>
        public void unsetTargetManager()
        {
            targetManager = null;
        }

        /// <summary>
        /// Create all products for normal operation currently registered for
        /// construction in this factory.
        /// </summary>
        public void createProducts()
        {
            foreach (PhysFactoryEntry actorDef in currentActors)
            {
                actorDef.createProduct(targetManager);
            }
        }

        /// <summary>
        /// Create all products for static mode operation currently registered
        /// for construction in this factory.
        /// </summary>
        public void createStaticProducts()
        {
            foreach (PhysFactoryEntry actorDef in currentActors)
            {
                actorDef.createStaticProduct(targetManager);
            }
        }

        /// <summary>
        /// This function will be called when all subsystems have created their
        /// products. At this time it is safe to discover objects present in
        /// other subsystems.
        /// </summary>
        public void linkProducts()
        {
            
        }

        /// <summary>
        /// This function will clear all definitions in the factory. It will be
        /// called after a construction run has completed by executing
        /// createProducts or createStaticProducts.
        /// </summary>
        public void clearDefinitions()
        {
            currentActors.Clear();
        }

        #endregion Functions
    }
}

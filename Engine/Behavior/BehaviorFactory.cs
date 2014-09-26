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
        public IEnumerable<SceneBuildStatus> createProducts(SceneBuildOptions options)
        {
            SceneBuildStatus status = new SceneBuildStatus()
            {
                Subsystem = BehaviorPluginInterface.Instance.Name
            };
            yield return status;
            bool copyBehavior = (options & SceneBuildOptions.SingleUseDefinitions) == 0;
            foreach (BehaviorFactoryEntry entry in currentBehaviors)
            {
                entry.createProduct(manager, copyBehavior);
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
                catch (BehaviorBlacklistException bbe)
                {
                    SimObjectErrorManager.AddError(createError(bbe));
                }
                catch (Exception e)
                {
                    entry._unanticipatedBlacklistError(e);
                    SimObjectErrorManager.AddError(createError(entry, e));
                }
            }
            foreach (BehaviorFactoryEntry entry in currentBehaviors)
            {
                try
                {
                    entry.linkupProducts();
                }
                catch (BehaviorBlacklistException bbe)
                {
                    SimObjectErrorManager.AddError(createError(bbe));
                }
                catch (Exception e)
                {
                    entry._unanticipatedBlacklistError(e);
                    SimObjectErrorManager.AddError(createError(entry, e));
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

        private static SimObjectError createError(BehaviorBlacklistException bbe)
        {
            return new SimObjectError()
            {
                Subsystem = "Behavior",
                SimObject = bbe.Behavior.Owner != null ? bbe.Behavior.Owner.Name : "NullSimObject",
                Type = bbe.Behavior.GetType().Name,
                ElementName = bbe.Behavior.Name,
                Message = bbe.Message
            };
        }

        private static SimObjectError createError(BehaviorFactoryEntry entry, Exception e)
        {
            var error = new SimObjectError()
            {
                Subsystem = "Behavior",
                Message = e.Message
            };

            if(entry.CreatedBehavior != null)
            {
                error.ElementName = entry.CreatedBehavior.Name;
                error.SimObject = entry.CreatedBehavior.Owner != null ? entry.CreatedBehavior.Owner.Name : "NullSimObject";
                error.Type = entry.CreatedBehavior.GetType().Name;
            }
            else
            {
                error.ElementName = "UnknownName";
                error.SimObject = "UnknownSimObject";
                error.Type = "UnknownType";
            }

            return error;
        }
    }
}

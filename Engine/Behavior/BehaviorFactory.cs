using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace Engine
{
    public class BehaviorFactory : SimElementFactory
    {
        private List<BehaviorFactoryEntry> currentBehaviors = new List<BehaviorFactoryEntry>();
        private BehaviorManager manager;

        public BehaviorFactory(BehaviorManager manager)
        {
            this.manager = manager;
        }

        public void createProducts()
        {
            foreach (BehaviorFactoryEntry entry in currentBehaviors)
            {
                entry.createProduct(manager);
            }
        }

        public void createStaticProducts()
        {
            throw new NotImplementedException();
        }

        public void linkProducts()
        {
            foreach (BehaviorFactoryEntry entry in currentBehaviors)
            {
                entry.linkupProducts();
            }
        }

        public void clearDefinitions()
        {
            currentBehaviors.Clear();
        }

        internal void addBehaviorDefinition(SimObject instance, BehaviorDefinition behaviorDefinition)
        {
            currentBehaviors.Add(new BehaviorFactoryEntry(instance, behaviorDefinition));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;
using Logging;

namespace Engine
{
    public class BehaviorDefinition : SimElementDefinition
    {
        private Type behaviorType;

        public BehaviorDefinition(String name)
            :base(name)
        {

        }

        public override void register(SimSubScene subscene, SimObject instance)
        {
            if (subscene.hasSimElementManagerType(typeof(BehaviorManager)))
            {
                BehaviorManager behaviorManager = subscene.getSimElementManager<BehaviorManager>();
                behaviorManager.getBehaviorFactory().addBehaviorDefinition(instance, this);
            }
            else
            {
                Log.Default.sendMessage("Cannot add BehaviorDefinition {0} to SimSubScene {1} because it does not contain a BehaviorManager.", LogLevel.Warning, "Behavior", Name, subscene.Name);
            }
        }

        public override EditInterface getEditInterface()
        {
            throw new NotImplementedException();
        }

        public void createProduct(SimObject instance, BehaviorManager behaviorManager)
        {
            throw new NotImplementedException();
        }
    }
}

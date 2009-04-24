using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;

namespace Engine
{
    public class BehaviorDefinition : SimElementDefinition
    {
        public BehaviorDefinition(String name)
            :base(name)
        {

        }

        public override void register(SimSubScene subscene, SimObject instance)
        {
            throw new NotImplementedException();
        }

        public override EditInterface getEditInterface()
        {
            throw new NotImplementedException();
        }

        public void createProduct(SimObject instance, BehaviorManager behaviorManager)
        {

        }
    }
}

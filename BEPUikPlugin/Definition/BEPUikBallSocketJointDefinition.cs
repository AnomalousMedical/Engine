using Engine.Attributes;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Saving;
using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikBallSocketJointDefinition : BEPUikJointDefinition
    {
        internal static void Create(string name, EditUICallback callback, CompositeSimObjectDefinition simObjectDefinition)
        {
            simObjectDefinition.addElement(new BEPUikBallSocketJointDefinition(name));
        }

        public BEPUikBallSocketJointDefinition(String name)
            :base(name)
        {
            
        }

        protected override SimElement createConstraint(BEPUikBone connectionA, BEPUikBone connectionB, SimObjectBase instance, BEPUikScene scene)
        {
            return new BEPUikBallSocketJoint(connectionA, connectionB, instance.Translation, this, scene, Name, Subscription);
        }

        protected override string EditInterfaceName
        {
            get
            {
                return "IK Ball Socket Joint";
            }
        }

        protected BEPUikBallSocketJointDefinition(LoadInfo info)
            :base(info)
        {
            
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
        }
    }
}

using Engine;
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
    public class BEPUikTwistJointDefinition : BEPUikJointDefinition
    {
        internal static void Create(string name, EditUICallback callback, CompositeSimObjectDefinition simObjectDefinition)
        {
            simObjectDefinition.addElement(new BEPUikTwistJointDefinition(name));
        }

        public BEPUikTwistJointDefinition(String name)
            :base(name)
        {
            WorldAxisA = Vector3.UnitX;
            WorldAxisB = Vector3.UnitZ;
        }

        [Editable]
        public Vector3 WorldAxisA { get; set; }

        [Editable]
        public Vector3 WorldAxisB { get; set; }

        protected override SimElement createConstraint(BEPUikBone connectionA, BEPUikBone connectionB, SimObjectBase instance)
        {
            return new BEPUikTwistJoint(connectionA, connectionB, this, Name, Subscription);
        }

        protected override string EditInterfaceName
        {
            get
            {
                return "IK Twist Joint";
            }
        }

        protected BEPUikTwistJointDefinition(LoadInfo info)
            :base(info)
        {
            WorldAxisA = info.GetVector3("WorldAxisA");
            WorldAxisB = info.GetVector3("WorldAxisB");
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("WorldAxisA", WorldAxisA);
            info.AddValue("WorldAxisB", WorldAxisB);
        }
    }
}

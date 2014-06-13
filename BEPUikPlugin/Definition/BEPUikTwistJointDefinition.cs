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
            AxisA = Vector3.UnitY;
            AxisB = Vector3.UnitY;
        }

        [Editable]
        public Vector3 AxisA { get; set; }

        [Editable]
        public Vector3 AxisB { get; set; }

        [Editable]
        public Vector3? MeasurementAxisA { get; set; }

        [Editable]
        public Vector3? MeasurementAxisB { get; set; }

        protected override SimElement createConstraint(BEPUikBone connectionA, BEPUikBone connectionB, SimObjectBase instance)
        {
            return new BEPUikTwistJoint(connectionA, connectionB, this, Name, Subscription, instance);
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
            AxisA = info.GetVector3("AxisA");
            AxisB = info.GetVector3("AxisB");
            if (info.hasValue("MeasurementAxisA"))
            {
                MeasurementAxisA = info.GetVector3("MeasurementAxisA");
            }
            if (info.hasValue("MeasurementAxisB"))
            {
                MeasurementAxisB = info.GetVector3("MeasurementAxisB");
            }
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("AxisA", AxisA);
            info.AddValue("AxisB", AxisB);
            if (MeasurementAxisA.HasValue)
            {
                info.AddValue("MeasurementAxisA", MeasurementAxisA.Value);
            }
            if (MeasurementAxisB.HasValue)
            {
                info.AddValue("MeasurementAxisB", MeasurementAxisB.Value);
            }
        }
    }
}

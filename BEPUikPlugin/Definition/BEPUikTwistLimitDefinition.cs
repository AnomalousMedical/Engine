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
    public class BEPUikTwistLimitDefinition : BEPUikLimitDefinition
    {
        internal static void Create(string name, EditUICallback callback, CompositeSimObjectDefinition simObjectDefinition)
        {
            simObjectDefinition.addElement(new BEPUikTwistLimitDefinition(name));
        }

        public BEPUikTwistLimitDefinition(String name)
            :base(name)
        {
            AxisA = Vector3.UnitY;
            MeasurementAxisA = Vector3.UnitX;
            AxisB = Vector3.UnitY;
            MeasurementAxisB = Vector3.UnitX;
        }

        protected override SimElement createConstraint(BEPUikBone connectionA, BEPUikBone connectionB, SimObjectBase instance)
        {
            return new BEPUikTwistLimit(connectionA, connectionB, this, Name, Subscription);
        }

        protected override string EditInterfaceName
        {
            get
            {
                return "IK Twist Limit";
            }
        }

        [Editable]
        public float MaximumAngle { get; set; }

        [Editable]
        public Vector3 AxisA { get; set; }

        [Editable]
        public Vector3 AxisB { get; set; }

        [Editable]
        public Vector3 MeasurementAxisA { get; set; }

        [Editable]
        public Vector3 MeasurementAxisB { get; set; }

        protected BEPUikTwistLimitDefinition(LoadInfo info)
            :base(info)
        {
            MaximumAngle = info.GetFloat("MaximumAngle");
            AxisA = info.GetVector3("AxisA");
            AxisB = info.GetVector3("AxisB");
            MeasurementAxisA = info.GetVector3("MeasurementAxisA");
            MeasurementAxisB = info.GetVector3("MeasurementAxisB");
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("MaximumAngle", MaximumAngle);
            info.AddValue("AxisA", AxisA);
            info.AddValue("AxisB", AxisB);
            info.AddValue("MeasurementAxisA", MeasurementAxisA);
            info.AddValue("MeasurementAxisB", MeasurementAxisB);
        }
    }
}

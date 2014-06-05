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
            LocalAxisA = Vector3.UnitY;
            LocalMeasurementAxisA = Vector3.UnitX;
            LocalAxisB = Vector3.UnitY;
            LocalMeasurementAxisB = Vector3.UnitX;
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
        public Vector3 LocalAxisA { get; set; }

        [Editable]
        public Vector3 LocalAxisB { get; set; }

        [Editable]
        public Vector3 LocalMeasurementAxisA { get; set; }

        [Editable]
        public Vector3 LocalMeasurementAxisB { get; set; }

        protected BEPUikTwistLimitDefinition(LoadInfo info)
            :base(info)
        {
            MaximumAngle = info.GetFloat("MaximumAngle");
            LocalAxisA = info.GetVector3("LocalAxisA");
            LocalAxisB = info.GetVector3("LocalAxisB");
            LocalMeasurementAxisA = info.GetVector3("LocalMeasurementAxisA");
            LocalMeasurementAxisB = info.GetVector3("LocalMeasurementAxisB");
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("MaximumAngle", MaximumAngle);
            info.AddValue("LocalAxisA", LocalAxisA);
            info.AddValue("LocalAxisB", LocalAxisB);
            info.AddValue("LocalMeasurementAxisA", LocalMeasurementAxisA);
            info.AddValue("LocalMeasurementAxisB", LocalMeasurementAxisB);
        }
    }
}

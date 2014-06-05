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
    public class BEPUikSwingLimitDefinition : BEPUikLimitDefinition
    {
        internal static void Create(string name, EditUICallback callback, CompositeSimObjectDefinition simObjectDefinition)
        {
            simObjectDefinition.addElement(new BEPUikSwingLimitDefinition(name));
        }

        public BEPUikSwingLimitDefinition(String name)
            :base(name)
        {
            LocalAxisA = Vector3.UnitY;
            LocalAxisB = Vector3.UnitY;
        }

        protected override SimElement createConstraint(BEPUikBone connectionA, BEPUikBone connectionB, SimObjectBase instance)
        {
            return new BEPUikSwingLimit(connectionA, connectionB, this, Name, Subscription);
        }

        protected override string EditInterfaceName
        {
            get
            {
                return "IK Swing Limit";
            }
        }

        [Editable]
        public float MaximumAngle { get; set; }

        [Editable]
        public Vector3 LocalAxisA { get; set; }

        [Editable]
        public Vector3 LocalAxisB { get; set; }

        protected BEPUikSwingLimitDefinition(LoadInfo info)
            :base(info)
        {
            MaximumAngle = info.GetFloat("MaximumAngle");
            LocalAxisA = info.GetVector3("LocalAxisA");
            LocalAxisB = info.GetVector3("LocalAxisB");
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("MaximumAngle", MaximumAngle);
            info.AddValue("LocalAxisA", LocalAxisA);
            info.AddValue("LocalAxisB", LocalAxisB);
        }
    }
}

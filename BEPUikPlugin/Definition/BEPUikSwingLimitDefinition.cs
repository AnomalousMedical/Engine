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
            AxisA = Vector3.UnitY;
            AxisB = Vector3.UnitY;
        }

        protected override SimElement createConstraint(BEPUikBone connectionA, BEPUikBone connectionB, SimObjectBase instance)
        {
            return new BEPUikSwingLimit(connectionA, connectionB, this, Name, instance);
        }

        protected override void customizeEditInterface(EditInterface editInterface)
        {
            base.customizeEditInterface(editInterface);
            editInterface.Renderer = new SwingLimitRenderer(this);
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
        public Vector3 AxisA { get; set; }

        [Editable]
        public Vector3 AxisB { get; set; }

        protected BEPUikSwingLimitDefinition(LoadInfo info)
            :base(info)
        {
            MaximumAngle = info.GetFloat("MaximumAngle");
            AxisA = info.GetVector3("AxisA");
            AxisB = info.GetVector3("AxisB");
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("MaximumAngle", MaximumAngle);
            info.AddValue("AxisA", AxisA);
            info.AddValue("AxisB", AxisB);
        }
    }
}

using Engine;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikPointOnLineJointDefinition : BEPUikJointDefinition
    {
        internal static void Create(string name, EditUICallback callback, CompositeSimObjectDefinition simObjectDefinition)
        {
            simObjectDefinition.addElement(new BEPUikPointOnLineJointDefinition(name));
        }

        public BEPUikPointOnLineJointDefinition(String name)
            :base(name)
        {
            LineDirection = Vector3.Forward;
        }

        protected override SimElement createConstraint(BEPUikBone connectionA, BEPUikBone connectionB, SimObjectBase instance)
        {
            return new BEPUikPointOnLineJoint(connectionA, connectionB, this, Name, subscription, instance);
        }

        protected override string EditInterfaceName
        {
            get
            {
                return "IK Point On Line Joint";
            }
        }

        protected override void customizeEditInterface(EditInterface editInterface)
        {
            editInterface.Renderer = new PointOnLineJointRenderer(this);
            base.customizeEditInterface(editInterface);
        }

        [Editable]
        public Vector3 LineDirection { get; set; }

        protected BEPUikPointOnLineJointDefinition(LoadInfo info)
            :base(info)
        {
            LineDirection = info.GetVector3("LineDirection");
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("LineDirection", LineDirection);
        }
    }
}

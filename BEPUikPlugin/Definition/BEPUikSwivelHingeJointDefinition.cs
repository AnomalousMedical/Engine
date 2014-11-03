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
    public class BEPUikSwivelHingeJointDefinition : BEPUikJointDefinition
    {
        internal static void Create(string name, EditUICallback callback, CompositeSimObjectDefinition simObjectDefinition)
        {
            simObjectDefinition.addElement(new BEPUikSwivelHingeJointDefinition(name));
        }

        public BEPUikSwivelHingeJointDefinition(String name)
            :base(name)
        {
            WorldHingeAxis = Vector3.UnitX;
            WorldTwistAxis = Vector3.UnitZ;
        }

        [Editable]
        public Vector3 WorldHingeAxis { get; set; }

        [Editable]
        public Vector3 WorldTwistAxis { get; set; }

        protected override SimElement createConstraint(BEPUikBone connectionA, BEPUikBone connectionB, SimObjectBase instance)
        {
            return new BEPUikSwivelHingeJoint(connectionA, connectionB, this, Name, instance);
        }

        protected override void customizeEditInterface(EditInterface editInterface)
        {
            base.customizeEditInterface(editInterface);
            editInterface.Renderer = new SwivelHingeJointRenderer(this);
        }

        protected override string EditInterfaceName
        {
            get
            {
                return "IK Swivel Hinge Joint";
            }
        }

        protected BEPUikSwivelHingeJointDefinition(LoadInfo info)
            :base(info)
        {
            WorldHingeAxis = info.GetVector3("WorldHingeAxis");
            WorldTwistAxis = info.GetVector3("WorldTwistAxis");
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("WorldHingeAxis", WorldHingeAxis);
            info.AddValue("WorldTwistAxis", WorldTwistAxis);
        }
    }
}

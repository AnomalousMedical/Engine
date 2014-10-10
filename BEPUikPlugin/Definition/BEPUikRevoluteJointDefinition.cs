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
    public class BEPUikRevoluteJointDefinition : BEPUikJointDefinition
    {
        internal static void Create(string name, EditUICallback callback, CompositeSimObjectDefinition simObjectDefinition)
        {
            simObjectDefinition.addElement(new BEPUikRevoluteJointDefinition(name));
        }

        public BEPUikRevoluteJointDefinition(String name)
            :base(name)
        {
            WorldFreeAxis = Vector3.UnitX;
        }

        [Editable]
        public Vector3 WorldFreeAxis { get; set; }

        [Editable]
        public Vector3 WorldFreeAxisNormalize
        {
            get
            {
                return WorldFreeAxis;
            }
            set
            {
                WorldFreeAxis = value.normalized();
            }
        }

        protected override SimElement createConstraint(BEPUikBone connectionA, BEPUikBone connectionB, SimObjectBase instance)
        {
            return new BEPUikRevoluteJoint(connectionA, connectionB, this, Name, Subscription, instance);
        }

        protected override void customizeEditInterface(EditInterface editInterface)
        {
            base.customizeEditInterface(editInterface);
            editInterface.Renderer = new RevoluteJointRenderer(this);
        }

        protected override string EditInterfaceName
        {
            get
            {
                return "IK Revolute Joint";
            }
        }

        protected BEPUikRevoluteJointDefinition(LoadInfo info)
            :base(info)
        {
            WorldFreeAxis = info.GetVector3("WorldFreeAxis");
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("WorldFreeAxis", WorldFreeAxis);
        }
    }
}

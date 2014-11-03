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
    public class BEPUikAngularJointDefinition : BEPUikJointDefinition
    {
        internal static void Create(string name, EditUICallback callback, CompositeSimObjectDefinition simObjectDefinition)
        {
            simObjectDefinition.addElement(new BEPUikAngularJointDefinition(name));
        }

        public BEPUikAngularJointDefinition(String name)
            :base(name)
        {
            
        }

        protected override SimElement createConstraint(BEPUikBone connectionA, BEPUikBone connectionB, SimObjectBase instance)
        {
            return new BEPUikAngularJoint(connectionA, connectionB, this, Name, instance);
        }

        protected override string EditInterfaceName
        {
            get
            {
                return "IK Angular Joint";
            }
        }

        protected BEPUikAngularJointDefinition(LoadInfo info)
            :base(info)
        {
            
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
        }
    }
}

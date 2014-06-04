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
    public class BEPUikBallSocketJointDefinition : BEPUikJointDefinition
    {
        [DoNotSave]
        private EditInterface editInterface;

        public BEPUikBallSocketJointDefinition(String name)
            :base(name)
        {
            
        }

        protected override EditInterface createEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, String.Format("{0} - IK Ball Socket Joint", Name));
            }
            return editInterface;
        }

        protected override SimElement createConstraint(BEPUikBone connectionA, BEPUikBone connectionB, SimObjectBase instance, BEPUikScene scene)
        {
            return new BEPUikBallSocketJoint(connectionA, connectionB, instance.Translation, this, scene, Name, Subscription);
        }

        protected BEPUikBallSocketJointDefinition(LoadInfo info)
            :base(info)
        {
            
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
        }

        internal static void Create(string name, EditUICallback callback, CompositeSimObjectDefinition simObjectDefinition)
        {
            simObjectDefinition.addElement(new BEPUikBallSocketJointDefinition(name));
        }
    }
}

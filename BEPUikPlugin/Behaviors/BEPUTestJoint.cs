using BEPUik;
using Engine;
using Engine.Attributes;
using Engine.Editing;
using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BEPUikPlugin.IKTest
{
    class BEPUTestJoint : Interface
    {
        IKBallSocketJoint joint;

        [Editable]
        private String connectionASimObjectName;

        [Editable]
        private String connectionABoneName = "Bone";

        [Editable]
        private String connectionBSimObjectName;

        [Editable]
        private String connectionBBoneName = "Bone";

        protected override void link()
        {
            base.link();

            SimObject connectionASimObject = Owner.getOtherSimObject(connectionASimObjectName);
            if (connectionASimObject == null)
            {
                blacklist("Cannot find connectionASimObject '{0}'", connectionASimObjectName);
            }

            SimObject connectionBSimObject = Owner.getOtherSimObject(connectionBSimObjectName);
            if (connectionBSimObject == null)
            {
                blacklist("Cannot find connectionBSimObject '{0}'", connectionBSimObjectName);
            }

            BEPUTestBone boneA = connectionASimObject.getElement(connectionABoneName) as BEPUTestBone;
            if (boneA == null)
            {
                blacklist("Cannot find connectionASimObject '{0}' bone named '{1}'", connectionASimObjectName, connectionABoneName);
            }

            BEPUTestBone boneB = connectionBSimObject.getElement(connectionBBoneName) as BEPUTestBone;
            if (boneB == null)
            {
                blacklist("Cannot find connectionBSimObject '{0}' bone named '{1}'", connectionBSimObjectName, connectionBBoneName);
            }

            joint = new IKBallSocketJoint(boneA.Bone, boneB.Bone, Owner.Translation.toBepuVec3());
        }

        public IKJoint Joint
        {
            get
            {
                return joint;
            }
        }
    }
}

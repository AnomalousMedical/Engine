using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.Saving;
using Engine.Editing;
using Engine.ObjectManagement;

namespace PhysXPlugin
{
    public class PhysFixedJointDefinition : PhysJointDefinitionBase<PhysFixedJointDesc, PhysFixedJoint>
    {
        public PhysFixedJointDefinition(String name)
            :base(new PhysFixedJointDesc(), name, "Fixed Joint", null)
        {
            
        }

        internal PhysFixedJointDefinition(String name, PhysFixedJointElement joint)
            :base(new PhysFixedJointDesc(), joint, name, "Fixed Joint", null)
        {
            joint.RealJoint.saveToDesc(jointDesc);
        }

        protected override void configureJoint()
        {
            
        }

        protected override void configureEditInterface(EditInterface editInterface)
        {

        }

        internal override PhysJointElement createElement(Identifier jointId, PhysJoint joint, PhysXSceneManager scene)
        {
            return new PhysFixedJointElement(jointId, (PhysFixedJoint)joint, scene, subscription);
        }

        #region Saveable

        private PhysFixedJointDefinition(LoadInfo loadInfo)
            :base(new PhysFixedJointDesc(), loadInfo)
        {

        }

        protected override void getJointInfo(SaveInfo info)
        {

        }

        #endregion Saveable
    }
}

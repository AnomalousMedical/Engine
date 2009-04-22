using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.Saving;

namespace PhysXPlugin
{
    public class PhysFixedJointDefinition : PhysJointDefinition<PhysFixedJointDesc, PhysFixedJoint>
    {
        private PhysFixedJointDesc fixedJointDesc = new PhysFixedJointDesc();

        public PhysFixedJointDefinition(String name)
            :base(name, "Fixed Joint", null)
        {
            this.setJointDesc(fixedJointDesc);
        }

        protected override void configureJoint(PhysFixedJoint joint)
        {
            
        }

        protected override void configureEditInterface(Engine.Editing.EditInterface editInterface)
        {

        }

        #region Saveable

        private PhysFixedJointDefinition(LoadInfo loadInfo)
            :base(loadInfo)
        {
            this.setJointDesc(fixedJointDesc, loadInfo);
        }

        protected override void getJointInfo(SaveInfo info)
        {

        }

        #endregion Saveable
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.Saving;

namespace PhysXPlugin
{
    public class PhysFixedJointDefinition : PhysJointDefinitionBase<PhysFixedJointDesc, PhysFixedJoint>
    {
        public PhysFixedJointDefinition(String name)
            :base(new PhysFixedJointDesc(), name, "Fixed Joint", null)
        {
            
        }

        protected override void configureJoint(PhysFixedJoint joint)
        {
            
        }

        protected override void configureEditInterface(Engine.Editing.EditInterface editInterface)
        {

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

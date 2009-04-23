using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.Saving;
using Engine.ObjectManagement;
using Engine.Editing;

namespace PhysXPlugin
{
    public class PhysCylindricalJointDefinition : PhysJointDefinitionBase<PhysCylindricalJointDesc, PhysCylindricalJoint>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the joint.</param>
        public PhysCylindricalJointDefinition(String name)
            : base(new PhysCylindricalJointDesc(), name, "Cylindrical Joint", null)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the joint.</param>
        /// <param name="joint">An existing PhysJointElement to get values from.</param>
        internal PhysCylindricalJointDefinition(String name, PhysCylindricalJointElement joint)
            : base(new PhysCylindricalJointDesc(), joint, name, "Cylindrical Joint", null)
        {
            joint.RealJoint.saveToDesc(jointDesc);
        }


        /// <summary>
        /// This function is called just before the joint is created. This
        /// allows any subclass configuration of the joint description to take
        /// place.
        /// </summary>
        protected override void configureJoint()
        {

        }

        /// <summary>
        /// Get any specific information for the EditInterface from a subclass.
        /// </summary>
        /// <param name="editInterface">The EditInterface to populate.</param>
        protected override void configureEditInterface(EditInterface editInterface)
        {

        }

        /// <summary>
        /// Create the PhysJointElement specific to this Joint.
        /// </summary>
        /// <param name="jointId">The Identifier of the joint being created.</param>
        /// <param name="joint">The joint that was created.</param>
        /// <param name="scene">The scene that contains the joint.</param>
        /// <returns>A new PhysJointElement for this specific joint type.</returns>
        internal override PhysJointElement createElement(Identifier jointId, PhysJoint joint, PhysXSceneManager scene)
        {
            return new PhysCylindricalJointElement(jointId, (PhysCylindricalJoint)joint, scene, subscription);
        }

        #region Saveable

        /// <summary>
        /// Load constructor.
        /// </summary>
        /// <param name="loadInfo"></param>
        private PhysCylindricalJointDefinition(LoadInfo loadInfo)
            : base(new PhysCylindricalJointDesc(), loadInfo)
        {

        }

        /// <summary>
        /// Save function.
        /// </summary>
        /// <param name="info"></param>
        protected override void getJointInfo(SaveInfo info)
        {

        }

        #endregion Saveable
    }
}

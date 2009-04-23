using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Saving;
using PhysXWrapper;

namespace PhysXPlugin
{
    public class PhysJointDriveDescription : Saveable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PhysJointDriveDescription()
        {

        }

        /// <summary>
        /// The type of the drive.
        /// </summary>
        [Editable]
        public D6JointDriveType DriveType { get; set; }

        /// <summary>
        /// Spring coefficient.
        /// </summary>
        [Editable]
        public float Spring { get; set; }

        /// <summary>
        /// Damper coefficient.
        /// </summary>
        [Editable]
        public float Damping { get; set; }

        /// <summary>
        /// The maximum force (or torque) the drive can exert.
        /// </summary>
        [Editable]
        public float ForceLimit { get; set; }

        /// <summary>
        /// Copy the values from this class to the description.
        /// </summary>
        /// <param name="target">Copy to.</param>
        internal void copyToDescription(PhysJointDriveDesc target)
        {
            target.DriveType = DriveType;
            target.Spring = Spring;
            target.Damping = Damping;
            target.ForceLimit = ForceLimit;
        }

        /// <summary>
        /// Copy the values from the description to this class.
        /// </summary>
        /// <param name="source">Copy from.</param>
        internal void copyFromDescription(PhysJointDriveDesc source)
        {
            DriveType = source.DriveType;
            Spring = source.Spring;
            Damping = source.Damping;
            ForceLimit = source.ForceLimit;
        }

        #region Saveable

        private const String DRIVE_TYPE = "PhysJointDriveDescriptionDriveType";
        private const String SPRING = "PhysJointDriveDescriptionSpring";
        private const String DAMPING = "PhysJointDriveDescriptionDamping";
        private const String FORCE_LIMIT = "PhysJointDriveDescriptionForceLimit";

        private PhysJointDriveDescription(LoadInfo info)
        {
            DriveType = info.GetValue<D6JointDriveType>(DRIVE_TYPE);
            Spring = info.GetFloat(SPRING);
            Damping = info.GetFloat(DAMPING);
            ForceLimit = info.GetFloat(FORCE_LIMIT);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(DRIVE_TYPE, DriveType);
            info.AddValue(SPRING, Spring);
            info.AddValue(DAMPING, Damping);
            info.AddValue(FORCE_LIMIT, ForceLimit);
        }

        #endregion Saveable
    }
}

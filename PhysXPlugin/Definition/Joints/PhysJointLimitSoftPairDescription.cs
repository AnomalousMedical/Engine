using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using PhysXWrapper;
using Engine.Editing;

namespace PhysXPlugin
{
    public class PhysJointLimitSoftPairDescription : Saveable
    {
        [Editable]
        PhysJointLimitSoftDescription low = new PhysJointLimitSoftDescription();
        [Editable]
        PhysJointLimitSoftDescription high = new PhysJointLimitSoftDescription();

        /// <summary>
        /// Constructor.
        /// </summary>
        public PhysJointLimitSoftPairDescription()
        {

        }

        /// <summary>
        /// Copy to target.
        /// </summary>
        /// <param name="target">Copy to.</param>
        internal void copyToDescription(PhysJointLimitSoftPairDesc target)
        {
            low.copyToDescription(target.Low);
            high.copyToDescription(target.High);
        }

        /// <summary>
        /// Copy from target.
        /// </summary>
        /// <param name="source">Copy from.</param>
        internal void copyFromDescription(PhysJointLimitSoftPairDesc source)
        {
            low.copyFromDescription(source.Low);
            high.copyFromDescription(source.High);
        }

        #region Saveable Members

        private const String LOW = "PhysJointLimitSoftPairDescriptionLow";
        private const String HIGH = "PhysJointLimitSoftPairDescriptionHigh";

        private PhysJointLimitSoftPairDescription(LoadInfo info)
        {
            low = info.GetValue<PhysJointLimitSoftDescription>(LOW);
            high = info.GetValue<PhysJointLimitSoftDescription>(HIGH);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(LOW, low);
            info.AddValue(HIGH, high);
        }

        #endregion
    }
}

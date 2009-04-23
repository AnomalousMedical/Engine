using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using PhysXWrapper;
using Engine.Editing;

namespace PhysXPlugin
{
    public class PhysJointLimitPairDescription : Saveable
    {
        [Editable]
        PhysJointLimitDescription low = new PhysJointLimitDescription();
        [Editable]
        PhysJointLimitDescription high = new PhysJointLimitDescription();

        /// <summary>
        /// Constructor.
        /// </summary>
        public PhysJointLimitPairDescription()
        {

        }

        /// <summary>
        /// Copy to the description.
        /// </summary>
        /// <param name="target">Copy to.</param>
        internal void copyToDescription(PhysJointLimitPairDesc target)
        {
            low.copyToDescription(target.Low);
            high.copyToDescription(target.High);
        }

        /// <summary>
        /// Copy from the description.
        /// </summary>
        /// <param name="source">Copy from.</param>
        internal void copyFromDescription(PhysJointLimitPairDesc source)
        {
            low.copyFromDescription(source.Low);
            high.copyFromDescription(source.High);
        }

        #region Saveable Members

        private const String LOW = "PhysJointLimitPairDescriptionLow";
        private const String HIGH = "PhysJointLimitPairDescriptionHigh";

        private PhysJointLimitPairDescription(LoadInfo info)
        {
            low = info.GetValue<PhysJointLimitDescription>(LOW);
            high = info.GetValue<PhysJointLimitDescription>(HIGH);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(LOW, low);
            info.AddValue(HIGH, high);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using Engine.Editing;
using PhysXWrapper;

namespace PhysXPlugin
{
    public class PhysJointLimitSoftDescription : Saveable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PhysJointLimitSoftDescription()
        {

        }

        /// <summary>
        /// The angle / position beyond which the limit is active.
        /// </summary>
        [Editable]
        public float Value { get; set; }

        /// <summary>
        /// Controls the amount of bounce when the joint hits a limit.
        /// </summary>
        [Editable]
        public float Restitution { get; set; }

        /// <summary>
        /// If greater than zero, the limit is soft, i.e. a spring pulls the joint back 
        /// to the limit.
        /// </summary>
        [Editable]
        public float Spring { get; set; }

        /// <summary>
        /// If spring is greater than zero, this is the damping of the spring.
        /// </summary>
        [Editable]
        public float Damping { get; set; }

        /// <summary>
        /// Copy to target.
        /// </summary>
        /// <param name="target">Copy to.</param>
        internal void copyToDescription(PhysJointLimitSoftDesc target)
        {
            target.Value = Value;
            target.Restitution = Restitution;
            target.Spring = Spring;
            target.Damping = Damping;
        }

        /// <summary>
        /// Copy from target.
        /// </summary>
        /// <param name="source">Copy from.</param>
        internal void copyFromDescription(PhysJointLimitSoftDesc source)
        {
            Value = source.Value;
            Restitution = source.Restitution;
            Spring = source.Spring;
            Damping = source.Damping;
        }

        #region Saveable Members

        private const String VALUE = "PhysJointLimitSoftDescriptionValue";
        private const String RESTITUTION = "PhysJointLimitSoftDescriptionRestitution";
        private const String SPRING = "PhysJointLimitSoftDescriptionSpring";
        private const String DAMPING = "PhysJointLimitSoftDescriptionDamping";

        private PhysJointLimitSoftDescription(LoadInfo info)
        {
            Value = info.GetFloat(VALUE);
            Restitution = info.GetFloat(RESTITUTION);
            Spring = info.GetFloat(SPRING);
            Damping = info.GetFloat(DAMPING);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(VALUE, Value);
            info.AddValue(RESTITUTION, Restitution);
            info.AddValue(SPRING, Spring);
            info.AddValue(DAMPING, Damping);
        }

        #endregion
    }
}

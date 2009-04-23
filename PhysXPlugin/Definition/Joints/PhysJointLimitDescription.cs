using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using Engine.Editing;
using PhysXWrapper;

namespace PhysXPlugin
{
    public class PhysJointLimitDescription : Saveable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PhysJointLimitDescription()
        {

        }

        /// <summary>
        /// The angle / position beyond which the limit is active.
        /// </summary>
        [Editable]
        public float Value { get; set; }

        /// <summary>
        /// Limit bounce.
        /// </summary>
        [Editable]
        public float Restitution { get; set; }

        /// <summary>
        /// [not yet implemented!] limit can be made softer by setting this to less than 1.
        /// </summary>
        [Editable]
        public float Hardness { get; set; }

        /// <summary>
        /// Copy to the description.
        /// </summary>
        /// <param name="target">Copy to.</param>
        internal void copyToDescription(PhysJointLimitDesc target)
        {
            target.Value = Value;
            target.Restitution = Restitution;
            target.Hardness = Hardness;
        }

        /// <summary>
        /// Copy from the description.
        /// </summary>
        /// <param name="source">Copy from.</param>
        internal void copyFromDescription(PhysJointLimitDesc source)
        {
            Value = source.Value;
            Restitution = source.Restitution;
            Hardness = source.Hardness;
        }

        #region Saveable Members

        public const String VALUE = "Value";
        public const String RESTITUITON = "Restitution";
        public const String HARDNESS = "Hardness";

        private PhysJointLimitDescription(LoadInfo info)
        {
            Value = info.GetFloat(VALUE);
            Restitution = info.GetFloat(RESTITUITON);
            Hardness = info.GetFloat(HARDNESS);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(VALUE, Value);
            info.AddValue(RESTITUITON, Restitution);
            info.AddValue(HARDNESS, Hardness);
        }

        #endregion
    }
}

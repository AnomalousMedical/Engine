using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using Engine.Editing;
using PhysXWrapper;

namespace PhysXPlugin
{
    public class PhysMotorDescription : Saveable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PhysMotorDescription()
        {

        }

        [Editable]
        public float VelTarget { get; set; }

        [Editable]
        public float MaxForce { get; set; }

        [Editable]
        public bool FreeSpin { get; set; }

        internal void copyToDescription(PhysMotorDesc target)
        {
            target.VelTarget = VelTarget;
            target.MaxForce = MaxForce;
            target.FreeSpin = FreeSpin;
        }

        internal void copyFromDescription(PhysMotorDesc target)
        {
            VelTarget = target.VelTarget;
            MaxForce = target.MaxForce;
            FreeSpin = target.FreeSpin;
        }

        #region Saveable Members

        private const String VEL_TARGET = "VelTarget";
        private const String MAX_FORCE = "MaxForce";
        private const String FREE_SPIN = "FreeSpin";

        private PhysMotorDescription(LoadInfo info)
        {
            VelTarget = info.GetFloat(VEL_TARGET);
            MaxForce = info.GetFloat(MAX_FORCE);
            FreeSpin = info.GetBoolean(FREE_SPIN);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(VEL_TARGET, VelTarget);
            info.AddValue(MAX_FORCE, MaxForce);
            info.AddValue(FREE_SPIN, FreeSpin);
        }

        #endregion
    }
}

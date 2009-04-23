using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using Engine.Editing;
using PhysXWrapper;

namespace PhysXPlugin
{
    public class PhysSpringDescription : Saveable
    {
        public PhysSpringDescription()
        {

        }

        [Editable]
        public float Spring { get; set; }

        [Editable]
        public float Damper { get; set; }

        [Editable]
        public float TargetValue { get; set; }

        internal void copyToDescription(PhysSpringDesc target)
        {
            target.Spring = Spring;
            target.Damper = Damper;
            target.TargetValue = TargetValue;
        }

        internal void copyFromDescription(PhysSpringDesc source)
        {
            Spring = source.Spring;
            Damper = source.Damper;
            TargetValue = source.TargetValue;
        }

        #region Saveable Members

        private const String SPRING = "Spring";
        private const String DAMPER = "Damper";
        private const String TARGET_VALUE = "TargetValue";

        private PhysSpringDescription(LoadInfo info)
        {
            Spring = info.GetFloat(SPRING);
            Damper = info.GetFloat(DAMPER);
            TargetValue = info.GetFloat(TARGET_VALUE);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(SPRING, Spring);
            info.AddValue(DAMPER, Damper);
            info.AddValue(TARGET_VALUE, TargetValue);
        }

        #endregion
    }
}

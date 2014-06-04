using Engine.Editing;
using Engine.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public abstract class BEPUikConstraintDefinition : BEPUikElementDefinition
    {
        public BEPUikConstraintDefinition(String name)
            :base(name)
        {
            Rigidity = 16f;
            MaximumForce = float.MaxValue;
        }

        [Editable]
        public float Rigidity { get; set; }

        [Editable]
        public float MaximumForce { get; set; }

        protected BEPUikConstraintDefinition(LoadInfo info)
            :base(info)
        {
            Rigidity = info.GetFloat("Rigidity", 16f);
            MaximumForce = info.GetFloat("MaximumForce", float.MaxValue);
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("Rigidity", Rigidity);
            info.AddValue("MaximumForce", MaximumForce);
        }
    }
}

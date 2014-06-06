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
        private EditInterface editInterface;

        public BEPUikConstraintDefinition(String name)
            :base(name)
        {
            Rigidity = 16f;
            MaximumForce = float.MaxValue;
        }

        protected override sealed EditInterface createEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, String.Format("{0} - {1}", Name, EditInterfaceName));
                customizeEditInterface(editInterface);
            }
            return editInterface;
        }

        protected virtual void customizeEditInterface(EditInterface editInterface)
        {

        }

        [Editable]
        public float Rigidity { get; set; }

        [Editable]
        public float MaximumForce { get; set; }

        protected abstract String EditInterfaceName { get; }

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

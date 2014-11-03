using Engine;
using Engine.Attributes;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Saving;
using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikDistanceLimitDefinition : BEPUikLimitDefinition
    {
        internal static void Create(string name, EditUICallback callback, CompositeSimObjectDefinition simObjectDefinition)
        {
            simObjectDefinition.addElement(new BEPUikDistanceLimitDefinition(name));
        }

        public BEPUikDistanceLimitDefinition(String name)
            :base(name)
        {
            
        }

        protected override SimElement createConstraint(BEPUikBone connectionA, BEPUikBone connectionB, SimObjectBase instance)
        {
            return new BEPUikDistanceLimit(connectionA, connectionB, instance.Translation, this, Name, instance);
        }

        protected override string EditInterfaceName
        {
            get
            {
                return "IK Distance Limit";
            }
        }

        protected override void customizeEditInterface(EditInterface editInterface)
        {
            editInterface.Renderer = new DistanceLimitRenderer(this);
            base.customizeEditInterface(editInterface);
        }

        [Editable]
        public float MinimumDistance { get; set; }

        [Editable]
        public float MaximumDistance { get; set; }

        protected BEPUikDistanceLimitDefinition(LoadInfo info)
            :base(info)
        {
            MinimumDistance = info.GetFloat("MinimumDistance");
            MaximumDistance = info.GetFloat("MaximumDistance");
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("MinimumDistance", MinimumDistance);
            info.AddValue("MaximumDistance", MaximumDistance);
        }
    }
}

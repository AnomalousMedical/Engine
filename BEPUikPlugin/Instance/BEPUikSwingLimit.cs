using BEPUik;
using Engine;
using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikSwingLimit : BEPUikLimit
    {
        private IKSwingLimit limit;

        public BEPUikSwingLimit(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikSwingLimitDefinition definition, String name, Subscription subscription)
            :base(connectionA, connectionB, name, subscription)
        {
            limit = new IKSwingLimit(connectionA.IkBone, connectionB.IkBone, definition.AxisA.toBepuVec3(), definition.AxisB.toBepuVec3(), definition.MaximumAngle);
            setupLimit(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikSwingLimitDefinition(Name)
                {
                    MaximumAngle = limit.MaximumAngle,
                    AxisA = limit.AxisA.toEngineVec3(),
                    AxisB = limit.AxisB.toEngineVec3()
                };
            setupLimitDefinition(definition);
            return definition;
        }

        public override IKLimit IKLimit
        {
            get
            {
                return limit;
            }
        }
    }
}

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
            limit = new IKSwingLimit(connectionA.IkBone, connectionB.IkBone, definition.MaximumAngle)
            {
                LocalAxisA = definition.LocalAxisA.toBepuVec3(),
                LocalAxisB = definition.LocalAxisB.toBepuVec3()
            };
            setupLimit(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikSwingLimitDefinition(Name)
                {
                    MaximumAngle = limit.MaximumAngle,
                    LocalAxisA = limit.LocalAxisA.toEngineVec3(),
                    LocalAxisB = limit.LocalAxisB.toEngineVec3()
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

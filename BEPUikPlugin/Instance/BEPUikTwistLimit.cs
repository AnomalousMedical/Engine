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
    public class BEPUikTwistLimit : BEPUikLimit
    {
        private IKTwistLimit limit;

        public BEPUikTwistLimit(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikTwistLimitDefinition definition, String name, Subscription subscription)
            :base(connectionA, connectionB, name, subscription)
        {
            limit = new IKTwistLimit(connectionA.IkBone, connectionB.IkBone, definition.MaximumAngle)
            {
                LocalAxisA = definition.LocalAxisA.toBepuVec3(),
                LocalAxisB = definition.LocalAxisB.toBepuVec3(),
                LocalMeasurementAxisA = definition.LocalMeasurementAxisA.toBepuVec3(),
                LocalMeasurementAxisB = definition.LocalMeasurementAxisB.toBepuVec3()
            };
            setupLimit(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikTwistLimitDefinition(Name)
                {
                    MaximumAngle = limit.MaximumAngle,
                    LocalAxisA = limit.LocalAxisA.toEngineVec3(),
                    LocalAxisB = limit.LocalAxisB.toEngineVec3(),
                    LocalMeasurementAxisA = limit.LocalMeasurementAxisA.toEngineVec3(),
                    LocalMeasurementAxisB = limit.LocalMeasurementAxisB.toEngineVec3()
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

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
            limit = new IKTwistLimit(connectionA.IkBone, connectionB.IkBone, definition.AxisA.toBepuVec3(), definition.AxisB.toBepuVec3(), definition.MaximumAngle);
            if(definition.MeasurementAxisA.HasValue)
            {
                limit.MeasurementAxisA = definition.MeasurementAxisA.Value.toBepuVec3();
            }
            if (definition.MeasurementAxisB.HasValue)
            {
                limit.MeasurementAxisB = definition.MeasurementAxisB.Value.toBepuVec3();
            }
            setupLimit(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikTwistLimitDefinition(Name)
                {
                    MaximumAngle = limit.MaximumAngle,
                    AxisA = limit.AxisA.toEngineVec3(),
                    AxisB = limit.AxisB.toEngineVec3(),
                    MeasurementAxisA = limit.LocalMeasurementAxisA.toEngineVec3(),
                    MeasurementAxisB = limit.LocalMeasurementAxisB.toEngineVec3()
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

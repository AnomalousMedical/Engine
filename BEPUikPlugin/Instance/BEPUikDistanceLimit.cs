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
    public class BEPUikDistanceLimit : BEPUikLimit
    {
        private IKDistanceLimit limit;

        public BEPUikDistanceLimit(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikDistanceLimitDefinition definition, String name, Subscription subscription)
            :base(connectionA, connectionB, name, subscription)
        {
            var anchorA = connectionA.Owner.Translation + Quaternion.quatRotate(connectionA.Owner.Rotation, definition.LocalAnchorOffsetA);
            var anchorB = connectionB.Owner.Translation + Quaternion.quatRotate(connectionB.Owner.Rotation, definition.LocalAnchorOffsetB);
            limit = new IKDistanceLimit(connectionA.IkBone, connectionB.IkBone, anchorA.toBepuVec3(), anchorB.toBepuVec3(), definition.MinimumDistance, definition.MaximumDistance);
            setupLimit(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikDistanceLimitDefinition(Name)
            {
                MinimumDistance = limit.MinimumDistance,
                MaximumDistance = limit.MaximumDistance
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

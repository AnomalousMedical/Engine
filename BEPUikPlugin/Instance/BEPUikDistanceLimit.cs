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
        private bool locked = false;
        private float unlockedMinDistance;
        private float unlockedMaxDistance;
        private BEPUutilities.Vector3 unlockedLocalAnchorA;
        private BEPUutilities.Vector3 unlockedLocalAnchorB;

        public BEPUikDistanceLimit(BEPUikBone connectionA, BEPUikBone connectionB, Vector3 anchor, BEPUikDistanceLimitDefinition definition, String name, Subscription subscription, SimObject instance)
            :base(connectionA, connectionB, name, subscription, instance)
        {
            limit = new IKDistanceLimit(connectionA.IkBone, connectionB.IkBone, anchor.toBepuVec3(), anchor.toBepuVec3(), definition.MinimumDistance, definition.MaximumDistance);
            setupLimit(definition);
        }

        public override bool Locked
        {
            get
            {
                return locked;
            }
            set
            {
                if(locked != value)
                {
                    locked = value;
                    if(locked)
                    {
                        //Not 100% sure about this, we haven't used distance limits really.
                        unlockedMinDistance = limit.MinimumDistance;
                        unlockedMaxDistance = limit.MaximumDistance;
                        unlockedLocalAnchorA = limit.LocalAnchorA;
                        unlockedLocalAnchorB = limit.LocalAnchorB;
                        var midpoint = ((ConnectionA.Owner.Translation + ConnectionB.Owner.Translation) / 2f).toBepuVec3();
                        limit.AnchorA = midpoint;
                        limit.AnchorB = midpoint;
                        limit.MinimumDistance = 0f;
                        limit.MaximumDistance = 0f;
                    }
                    else
                    {
                        limit.MinimumDistance = unlockedMinDistance;
                        limit.MaximumDistance = unlockedMaxDistance;
                        limit.LocalAnchorA = unlockedLocalAnchorA;
                        limit.LocalAnchorB = unlockedLocalAnchorB;
                    }
                }
            }
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikDistanceLimitDefinition(Name)
            {
                MinimumDistance = limit.MinimumDistance,
                MaximumDistance = limit.MaximumDistance,
            };
            setupLimitDefinition(definition);
            return definition;
        }

        internal override void draw(Engine.Renderer.DebugDrawingSurface drawingSurface, DebugDrawMode drawMode)
        {
            //TODO: Implement Constraint Drawing
        }

        internal override IKLimit IKLimit
        {
            get
            {
                return limit;
            }
        }
    }
}

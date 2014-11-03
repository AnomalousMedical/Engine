using BEPUik;
using Engine;
using Engine.ObjectManagement;
using Engine.Renderer;
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

        public BEPUikDistanceLimit(BEPUikBone connectionA, BEPUikBone connectionB, Vector3 anchor, BEPUikDistanceLimitDefinition definition, String name, SimObject instance)
            :base(connectionA, connectionB, name, instance)
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
                    fireLockedChanged();
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

        internal override void draw(DebugDrawingSurface drawingSurface, DebugDrawMode drawMode)
        {
            if((drawMode & DebugDrawMode.DistanceLimits) != 0)
            {
                Vector3 origin = VisualizationOrigin;
                drawingSurface.Color = Color.Blue;
                Vector3 minMaxPoint = origin + Vector3.Up * limit.MinimumDistance;
                drawingSurface.drawLine(origin, minMaxPoint);
                drawingSurface.Color = Color.Red;
                drawingSurface.drawLine(minMaxPoint, minMaxPoint + Vector3.Up * (limit.MaximumDistance - limit.MinimumDistance));

                drawingSurface.Color = Color.Blue;
                minMaxPoint = origin + Vector3.Down * limit.MinimumDistance;
                drawingSurface.drawLine(origin, minMaxPoint);
                drawingSurface.Color = Color.Red;
                drawingSurface.drawLine(minMaxPoint, minMaxPoint + Vector3.Down * (limit.MaximumDistance - limit.MinimumDistance));

                drawingSurface.Color = Color.Blue;
                minMaxPoint = origin + Vector3.Left * limit.MinimumDistance;
                drawingSurface.drawLine(origin, minMaxPoint);
                drawingSurface.Color = Color.Red;
                drawingSurface.drawLine(minMaxPoint, minMaxPoint + Vector3.Left * (limit.MaximumDistance - limit.MinimumDistance));

                drawingSurface.Color = Color.Blue;
                minMaxPoint = origin + Vector3.Right * limit.MinimumDistance;
                drawingSurface.drawLine(origin, minMaxPoint);
                drawingSurface.Color = Color.Red;
                drawingSurface.drawLine(minMaxPoint, minMaxPoint + Vector3.Right * (limit.MaximumDistance - limit.MinimumDistance));

                drawingSurface.Color = Color.Blue;
                minMaxPoint = origin + Vector3.Forward * limit.MinimumDistance;
                drawingSurface.drawLine(origin, minMaxPoint);
                drawingSurface.Color = Color.Red;
                drawingSurface.drawLine(minMaxPoint, minMaxPoint + Vector3.Forward * (limit.MaximumDistance - limit.MinimumDistance));

                drawingSurface.Color = Color.Blue;
                minMaxPoint = origin + Vector3.Backward * limit.MinimumDistance;
                drawingSurface.drawLine(origin, minMaxPoint);
                drawingSurface.Color = Color.Red;
                drawingSurface.drawLine(minMaxPoint, minMaxPoint + Vector3.Backward * (limit.MaximumDistance - limit.MinimumDistance));
            }
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

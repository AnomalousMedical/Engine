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
        private bool locked = false;
        private float unlockedMaxAngle;
        private BEPUutilities.Vector3 unlockedLocalAxisB;
        private BEPUutilities.Vector3 unlockedLocalMeasurementAxisB;

        public BEPUikTwistLimit(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikTwistLimitDefinition definition, String name, Subscription subscription, SimObject instance)
            :base(connectionA, connectionB, name, subscription, instance)
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
                    MeasurementAxisA = limit.MeasurementAxisA.toEngineVec3(),
                    MeasurementAxisB = limit.MeasurementAxisB.toEngineVec3()
                };
            setupLimitDefinition(definition);
            return definition;
        }

        public override bool Locked
        {
            get
            {
                return locked;
            }
            set
            {
                if (locked != value)
                {
                    locked = value;
                    if (locked)
                    {
                        unlockedMaxAngle = limit.MaximumAngle;
                        unlockedLocalAxisB = limit.LocalAxisB;
                        unlockedLocalMeasurementAxisB = limit.LocalMeasurementAxisB;
                        //Just set both axes to be axis a and set the max angle to 0. Use the world transforms.
                        limit.AxisB = limit.AxisA;
                        limit.MeasurementAxisB = limit.MeasurementAxisA;
                        limit.MaximumAngle = 0f;
                    }
                    else
                    {
                        limit.LocalAxisB = unlockedLocalAxisB;
                        limit.LocalMeasurementAxisB = unlockedLocalMeasurementAxisB;
                        limit.MaximumAngle = unlockedMaxAngle;
                    }
                }
            }
        }

        internal override void draw(Engine.Renderer.DebugDrawingSurface drawingSurface, DebugDrawMode drawMode)
        {
            if ((drawMode & DebugDrawMode.TwistLimits) != 0)
            {
                Vector3 origin = VisualizationOrigin;
                drawingSurface.Color = Color.Red;
                drawingSurface.drawLine(origin, origin + limit.AxisA.toEngineVec3() * 5.0f);
                drawingSurface.Color = Color.Orange;
                drawingSurface.drawLine(origin, origin + limit.MeasurementAxisA.toEngineVec3() * 5.0f);
                drawingSurface.Color = Color.Blue;
                drawingSurface.drawLine(origin, origin + limit.AxisB.toEngineVec3() * 5.0f);
                drawingSurface.Color = Color.LightBlue;
                drawingSurface.drawLine(origin, origin + limit.MeasurementAxisB.toEngineVec3() * 5.0f);
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

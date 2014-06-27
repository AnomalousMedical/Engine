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
    public class BEPUikTwistJoint : BEPUikJoint
    {
        private IKTwistJoint joint;

        public BEPUikTwistJoint(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikTwistJointDefinition definition, String name, Subscription subscription, SimObject instance)
            :base(connectionA, connectionB, name, subscription, instance)
        {
            joint = new IKTwistJoint(connectionA.IkBone, connectionB.IkBone, definition.AxisA.toBepuVec3(), definition.AxisB.toBepuVec3());
            if (definition.MeasurementAxisA.HasValue)
            {
                joint.MeasurementAxisA = definition.MeasurementAxisA.Value.toBepuVec3();
            }
            if (definition.MeasurementAxisB.HasValue)
            {
                joint.MeasurementAxisB = definition.MeasurementAxisB.Value.toBepuVec3();
            }
            setupJoint(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikTwistJointDefinition(Name)
            {
                AxisA = joint.AxisA.toEngineVec3(),
                AxisB = joint.AxisB.toEngineVec3(),
                MeasurementAxisA = joint.MeasurementAxisA.toEngineVec3(),
                MeasurementAxisB = joint.MeasurementAxisB.toEngineVec3()
            };
            setupJointDefinition(definition);
            return definition;
        }

        internal override void draw(DebugDrawingSurface drawingSurface, DebugDrawMode drawMode)
        {
            if ((drawMode & DebugDrawMode.TwistLimits) != 0)
            {
                Vector3 origin = VisualizationOrigin;
                drawingSurface.Color = Color.Red;
                drawingSurface.drawLine(origin, origin + joint.AxisA.toEngineVec3() * 5.0f);
                drawingSurface.Color = Color.Orange;
                drawingSurface.drawLine(origin, origin + joint.MeasurementAxisA.toEngineVec3() * 5.0f);
                drawingSurface.Color = Color.Blue;
                drawingSurface.drawLine(origin, origin + joint.AxisB.toEngineVec3() * 5.0f);
                drawingSurface.Color = Color.LightBlue;
                drawingSurface.drawLine(origin, origin + joint.MeasurementAxisB.toEngineVec3() * 5.0f);
            }
        }

        internal override IKJoint IKJoint
        {
            get
            {
                return joint;
            }
        }
    }
}

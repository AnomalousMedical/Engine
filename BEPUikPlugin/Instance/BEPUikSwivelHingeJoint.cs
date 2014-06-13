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
    public class BEPUikSwivelHingeJoint : BEPUikJoint
    {
        private IKSwivelHingeJoint joint;

        public BEPUikSwivelHingeJoint(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikSwivelHingeJointDefinition definition, String name, Subscription subscription, SimObject instance)
            :base(connectionA, connectionB, name, subscription, instance)
        {
            joint = new IKSwivelHingeJoint(connectionA.IkBone, connectionB.IkBone, definition.WorldHingeAxis.toBepuVec3(), definition.WorldTwistAxis.toBepuVec3());
            setupJoint(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikSwivelHingeJointDefinition(Name)
            {
                WorldHingeAxis = joint.WorldHingeAxis.toEngineVec3(),
                WorldTwistAxis = joint.WorldTwistAxis.toEngineVec3()
            };
            setupJointDefinition(definition);
            return definition;
        }

        internal override void draw(Engine.Renderer.DebugDrawingSurface drawingSurface, DebugDrawMode drawMode)
        {
            if((drawMode & DebugDrawMode.SwivelHingeJoints) != 0)
            {
                Vector3 origin = VisualizationOrigin;
                drawingSurface.Color = Color.Red;
                drawingSurface.drawLine(origin, origin + joint.WorldHingeAxis.toEngineVec3() * 10.0f);
                drawingSurface.Color = Color.Blue;
                drawingSurface.drawLine(origin, origin + joint.WorldTwistAxis.toEngineVec3() * 10.0f);
            }
        }

        public override IKJoint IKJoint
        {
            get
            {
                return joint;
            }
        }
    }
}

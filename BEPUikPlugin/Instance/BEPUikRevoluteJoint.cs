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
    public class BEPUikRevoluteJoint : BEPUikJoint
    {
        private IKRevoluteJoint joint;

        public BEPUikRevoluteJoint(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikRevoluteJointDefinition definition, String name, Subscription subscription, SimObject instance)
            :base(connectionA, connectionB, name, subscription, instance)
        {
            joint = new IKRevoluteJoint(connectionA.IkBone, connectionB.IkBone, definition.WorldFreeAxis.toBepuVec3());
            setupJoint(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikRevoluteJointDefinition(Name)
            {
                WorldFreeAxis = joint.WorldFreeAxisA.toEngineVec3()
            };
            setupJointDefinition(definition);
            return definition;
        }

        internal override void draw(DebugDrawingSurface drawingSurface, DebugDrawMode drawMode)
        {
            if ((drawMode & DebugDrawMode.RevoluteJoints) != 0)
            {
                Vector3 origin = VisualizationOrigin;
                drawingSurface.Color = Color.Red;
                drawingSurface.drawLine(origin, origin + joint.WorldFreeAxisA.toEngineVec3() * 5.0f);
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

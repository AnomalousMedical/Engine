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
    public class BEPUikBallSocketJoint : BEPUikJoint
    {
        private IKBallSocketJoint joint;

        public BEPUikBallSocketJoint(BEPUikBone connectionA, BEPUikBone connectionB, Vector3 anchor, BEPUikBallSocketJointDefinition definition, String name, Subscription subscription, SimObject instance)
            :base(connectionA, connectionB, name, subscription, instance)
        {
            joint = new IKBallSocketJoint(connectionA.IkBone, connectionB.IkBone, anchor.toBepuVec3());
            setupJoint(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikBallSocketJointDefinition(Name);
            setupJointDefinition(definition);
            return definition;
        }

        internal override void draw(DebugDrawingSurface drawingSurface, DebugDrawMode drawMode)
        {
            if ((drawMode & DebugDrawMode.BallSocketJoints) != 0)
            {
                drawingSurface.drawAxes(VisualizationOrigin, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, 5.0f);
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

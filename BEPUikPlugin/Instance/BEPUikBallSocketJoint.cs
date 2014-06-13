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
    public class BEPUikBallSocketJoint : BEPUikJoint
    {
        private IKBallSocketJoint joint;

        public BEPUikBallSocketJoint(BEPUikBone connectionA, BEPUikBone connectionB, Vector3 anchor, BEPUikBallSocketJointDefinition definition, String name, Subscription subscription)
            :base(connectionA, connectionB, name, subscription)
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

        internal override void draw(Engine.Renderer.DebugDrawingSurface drawingSurface)
        {
            //TODO: Implement Constraint Drawing
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

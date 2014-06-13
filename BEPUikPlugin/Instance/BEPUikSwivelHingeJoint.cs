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

        public BEPUikSwivelHingeJoint(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikSwivelHingeJointDefinition definition, String name, Subscription subscription)
            :base(connectionA, connectionB, name, subscription)
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

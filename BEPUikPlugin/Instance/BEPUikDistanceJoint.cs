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
    public class BEPUikDistanceJoint : BEPUikJoint
    {
        private IKDistanceJoint joint;

        public BEPUikDistanceJoint(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikDistanceJointDefinition definition, String name, Subscription subscription, SimObject instance)
            :base(connectionA, connectionB, name, subscription, instance)
        {
            joint = new IKDistanceJoint(connectionA.IkBone, connectionB.IkBone, connectionA.Owner.Translation.toBepuVec3(), connectionB.Owner.Translation.toBepuVec3());
            setupJoint(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikDistanceJointDefinition(Name);
            setupJointDefinition(definition);
            return definition;
        }

        internal override void draw(Engine.Renderer.DebugDrawingSurface drawingSurface, DebugDrawMode drawMode)
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

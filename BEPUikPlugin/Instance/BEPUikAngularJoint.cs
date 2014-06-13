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
    public class BEPUikAngularJoint : BEPUikJoint
    {
        private IKAngularJoint joint;

        public BEPUikAngularJoint(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikAngularJointDefinition definition, String name, Subscription subscription, SimObject instance)
            :base(connectionA, connectionB, name, subscription, instance)
        {
            joint = new IKAngularJoint(connectionA.IkBone, connectionB.IkBone);
            setupJoint(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikAngularJointDefinition(Name);
            setupJointDefinition(definition);
            return definition;
        }

        internal override void draw(DebugDrawingSurface drawingSurface, DebugDrawMode drawMode)
        {
            //Not really sure how to visualize this one
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

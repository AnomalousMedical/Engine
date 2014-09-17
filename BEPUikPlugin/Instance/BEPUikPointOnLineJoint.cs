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
    public class BEPUikPointOnLineJoint : BEPUikJoint
    {
        private IKPointOnLineJoint joint;

        public BEPUikPointOnLineJoint(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikPointOnLineJointDefinition definition, String name, Subscription subscription, SimObject instance)
            :base(connectionA, connectionB, name, subscription, instance)
        {
            joint = new IKPointOnLineJoint(connectionA.IkBone, connectionB.IkBone, instance.Translation.toBepuVec3(), definition.LineDirection.toBepuVec3(), instance.Translation.toBepuVec3());
            setupJoint(definition);
        }

        internal override IKJoint IKJoint
        {
            get
            {
                return joint;
            }
        }

        internal override void draw(DebugDrawingSurface drawingSurface, DebugDrawMode drawMode)
        {
            if((drawMode & DebugDrawMode.PointOnLineJoints) != 0)
            {
                Vector3 origin = VisualizationOrigin;
                drawingSurface.Color = Color.Red;
                drawingSurface.drawLine(origin, origin + joint.LineDirection.toEngineVec3() * 5.0f);
            }
        }

        public override SimElementDefinition saveToDefinition()
        {
            var def = new BEPUikPointOnLineJointDefinition(Name)
            {
                LineDirection = joint.LineDirection.toEngineVec3()
            };
            setupJointDefinition(def);
            return def;
        }
    }
}

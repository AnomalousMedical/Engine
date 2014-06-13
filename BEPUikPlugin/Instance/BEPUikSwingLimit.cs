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
    public class BEPUikSwingLimit : BEPUikLimit
    {
        private IKSwingLimit limit;

        public BEPUikSwingLimit(BEPUikBone connectionA, BEPUikBone connectionB, BEPUikSwingLimitDefinition definition, String name, Subscription subscription, SimObject instance)
            :base(connectionA, connectionB, name, subscription, instance)
        {
            limit = new IKSwingLimit(connectionA.IkBone, connectionB.IkBone, definition.AxisA.toBepuVec3(), definition.AxisB.toBepuVec3(), definition.MaximumAngle);
            setupLimit(definition);
        }

        public override SimElementDefinition saveToDefinition()
        {
            var definition = new BEPUikSwingLimitDefinition(Name)
                {
                    MaximumAngle = limit.MaximumAngle,
                    AxisA = limit.AxisA.toEngineVec3(),
                    AxisB = limit.AxisB.toEngineVec3()
                };
            setupLimitDefinition(definition);
            return definition;
        }

        internal override void draw(DebugDrawingSurface drawingSurface, DebugDrawMode drawMode)
        {
            if ((drawMode & DebugDrawMode.SwingLimits) != 0)
            {
                Vector3 origin = VisualizationOrigin;
                drawingSurface.Color = Color.Red;
                drawingSurface.drawLine(origin, origin + limit.AxisA.toEngineVec3() * 5.0f);
                drawingSurface.Color = Color.Blue;
                drawingSurface.drawLine(origin, origin + limit.AxisB.toEngineVec3() * 5.0f);
            }
        }

        public override IKLimit IKLimit
        {
            get
            {
                return limit;
            }
        }
    }
}

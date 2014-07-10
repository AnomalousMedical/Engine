using Engine;
using Engine.Editing;
using Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    class TwistJointRenderer : EditInterfaceRenderer
    {
        private BEPUikTwistJointDefinition definition;

        public TwistJointRenderer(BEPUikTwistJointDefinition definition)
        {
            this.definition = definition;
        }

        public void frameUpdate(DebugDrawingSurface drawingSurface)
        {
            
        }

        public void propertiesChanged(DebugDrawingSurface drawingSurface)
        {
            draw(drawingSurface);
        }

        public void interfaceSelected(DebugDrawingSurface drawingSurface)
        {
            draw(drawingSurface);
        }

        public void interfaceDeselected(DebugDrawingSurface drawingSurface)
        {
            
        }

        public void draw(DebugDrawingSurface drawingSurface)
        {
            drawingSurface.begin(String.Format("TwistJointRenderer_{0}", definition.Name), Engine.Renderer.DrawingType.LineList);
            drawingSurface.setDepthTesting(false);

            drawingSurface.Color = Color.Red;
            drawingSurface.drawLine(Vector3.Zero, definition.AxisA.normalized() * 10.0f);
            drawingSurface.Color = Color.Blue;
            drawingSurface.drawLine(Vector3.Zero, definition.AxisB.normalized() * 10.0f);

            if (definition.MeasurementAxisA.HasValue)
            {
                drawingSurface.Color = Color.Orange;
                drawingSurface.drawLine(Vector3.Zero, definition.MeasurementAxisA.Value.normalized() * 10.0f);
            }

            if (definition.MeasurementAxisB.HasValue)
            {
                drawingSurface.Color = Color.LightBlue;
                drawingSurface.drawLine(Vector3.Zero, definition.MeasurementAxisB.Value.normalized() * 10.0f);
            }

            drawingSurface.end();
        }
    }
}

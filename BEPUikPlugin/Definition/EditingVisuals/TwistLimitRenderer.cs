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
    class TwistLimitRenderer : EditInterfaceRenderer
    {
        private BEPUikTwistLimitDefinition definition;

        public TwistLimitRenderer(BEPUikTwistLimitDefinition definition)
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
            drawingSurface.begin(String.Format("TwistLimitRenderer_{0}", definition.Name), Engine.Renderer.DrawingType.LineList);
            drawingSurface.setDepthTesting(false);

            drawingSurface.Color = Color.Red;
            drawingSurface.drawLine(Vector3.Zero, definition.AxisA.normalized() * definition.AxisRenderLength);
            drawingSurface.Color = Color.Blue;
            drawingSurface.drawLine(Vector3.Zero, definition.AxisB.normalized() * definition.AxisRenderLength);

            if(definition.MeasurementAxisA.HasValue)
            {
                drawingSurface.Color = Color.Orange;
                drawingSurface.drawLine(Vector3.Zero, definition.MeasurementAxisA.Value.normalized() * definition.AxisRenderLength);
            }

            if (definition.MeasurementAxisB.HasValue)
            {
                drawingSurface.Color = Color.LightBlue;
                drawingSurface.drawLine(Vector3.Zero, definition.MeasurementAxisB.Value.normalized() * definition.AxisRenderLength);
            }

            drawingSurface.end();
        }
    }
}

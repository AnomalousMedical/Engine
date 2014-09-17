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
    class PointOnLineJointRenderer : EditInterfaceRenderer
    {
        private BEPUikPointOnLineJointDefinition definition;

        public PointOnLineJointRenderer(BEPUikPointOnLineJointDefinition definition)
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
            drawingSurface.begin(String.Format("PointOnLineJointRenderer_{0}", definition.Name), Engine.Renderer.DrawingType.LineList);
            drawingSurface.setDepthTesting(false);

            //Origin
            drawingSurface.Color = Color.Red;
            drawingSurface.drawLine(Vector3.Zero, definition.LineDirection.normalized() * definition.AxisRenderLength);
            drawingSurface.end();
        }
    }
}

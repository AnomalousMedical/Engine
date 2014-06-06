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
    class RevoluteJointRenderer : EditInterfaceRenderer
    {
        private BEPUikRevoluteJointDefinition definition;

        public RevoluteJointRenderer(BEPUikRevoluteJointDefinition definition)
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
            drawingSurface.begin(String.Format("RevoluteJointRenderer_{0}", definition.Name), Engine.Renderer.DrawingType.LineList);
            drawingSurface.setDepthTesting(false);

            //Origin
            drawingSurface.setColor(Color.Red);
            drawingSurface.drawLine(Vector3.Zero, definition.WorldFreeAxis.normalized() * 10.0f);
            drawingSurface.end();
        }
    }
}

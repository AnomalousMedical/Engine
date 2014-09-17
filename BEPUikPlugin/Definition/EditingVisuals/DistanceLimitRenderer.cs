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
    class DistanceLimitRenderer : EditInterfaceRenderer
    {
        private BEPUikDistanceLimitDefinition definition;

        public DistanceLimitRenderer(BEPUikDistanceLimitDefinition definition)
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
            drawingSurface.begin(String.Format("DistanceLimitRenderer_{0}", definition.Name), Engine.Renderer.DrawingType.LineList);
            drawingSurface.setDepthTesting(false);

            //Origin
            drawingSurface.Color = Color.Blue;
            Vector3 minMaxPoint = Vector3.Up * definition.MinimumDistance;
            drawingSurface.drawLine(Vector3.Zero, minMaxPoint);
            drawingSurface.Color = Color.Red;
            drawingSurface.drawLine(minMaxPoint, minMaxPoint + Vector3.Up * (definition.MaximumDistance - definition.MinimumDistance));

            drawingSurface.Color = Color.Blue;
            minMaxPoint = Vector3.Down * definition.MinimumDistance;
            drawingSurface.drawLine(Vector3.Zero, minMaxPoint);
            drawingSurface.Color = Color.Red;
            drawingSurface.drawLine(minMaxPoint, minMaxPoint + Vector3.Down * (definition.MaximumDistance - definition.MinimumDistance));

            drawingSurface.Color = Color.Blue;
            minMaxPoint = Vector3.Left * definition.MinimumDistance;
            drawingSurface.drawLine(Vector3.Zero, minMaxPoint);
            drawingSurface.Color = Color.Red;
            drawingSurface.drawLine(minMaxPoint, minMaxPoint + Vector3.Left * (definition.MaximumDistance - definition.MinimumDistance));

            drawingSurface.Color = Color.Blue;
            minMaxPoint = Vector3.Right * definition.MinimumDistance;
            drawingSurface.drawLine(Vector3.Zero, minMaxPoint);
            drawingSurface.Color = Color.Red;
            drawingSurface.drawLine(minMaxPoint, minMaxPoint + Vector3.Right * (definition.MaximumDistance - definition.MinimumDistance));

            drawingSurface.Color = Color.Blue;
            minMaxPoint = Vector3.Forward * definition.MinimumDistance;
            drawingSurface.drawLine(Vector3.Zero, minMaxPoint);
            drawingSurface.Color = Color.Red;
            drawingSurface.drawLine(minMaxPoint, minMaxPoint + Vector3.Forward * (definition.MaximumDistance - definition.MinimumDistance));

            drawingSurface.Color = Color.Blue;
            minMaxPoint = Vector3.Backward * definition.MinimumDistance;
            drawingSurface.drawLine(Vector3.Zero, minMaxPoint);
            drawingSurface.Color = Color.Red;
            drawingSurface.drawLine(minMaxPoint, minMaxPoint + Vector3.Backward * (definition.MaximumDistance - definition.MinimumDistance));

            drawingSurface.end();
        }
    }
}

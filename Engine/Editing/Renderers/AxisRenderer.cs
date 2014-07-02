using Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editing.Renderers
{
    public class AxisRenderer : EditInterfaceRenderer
    {
        private Func<Vector3> getOriginCallback;
        private String renderName;

        public AxisRenderer(String renderName, Func<Vector3> getOrigin)
        {
            this.renderName = renderName;
            getOriginCallback = getOrigin;
            Size = 1.0f;
        }

        public float Size { get; set; }

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
            drawingSurface.begin(renderName, Engine.Renderer.DrawingType.LineList);
            drawingSurface.setDepthTesting(false);

            Vector3 origin = getOriginCallback();

            //Origin
            drawingSurface.drawAxes(origin, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, Size);
            drawingSurface.end();
        }
    }
}

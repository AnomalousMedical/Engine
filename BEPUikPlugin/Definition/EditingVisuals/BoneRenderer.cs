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
    class BoneRenderer : EditInterfaceRenderer
    {
        private BEPUikBoneDefinition definition;

        public BoneRenderer(BEPUikBoneDefinition definition)
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
            drawingSurface.begin(String.Format("BoneRenderer_{0}", definition.Name), Engine.Renderer.DrawingType.LineList);
            drawingSurface.setDepthTesting(false);
            //Origin
            drawingSurface.setColor(Color.Red);
            drawingSurface.drawCircle(Vector3.Zero, Vector3.UnitX, Vector3.UnitZ, definition.Radius);
            float halfHeight = definition.Height;
            drawingSurface.drawCircle(Vector3.Up * halfHeight, Vector3.UnitX, Vector3.UnitZ, definition.Radius);
            drawingSurface.drawCircle(Vector3.Down * halfHeight, Vector3.UnitX, Vector3.UnitZ, definition.Radius);
            drawingSurface.end();
        }
    }
}

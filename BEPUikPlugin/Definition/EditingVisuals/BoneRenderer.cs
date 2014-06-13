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

        private void draw(DebugDrawingSurface drawingSurface)
        {
            drawingSurface.begin(String.Format("BoneRenderer_{0}", definition.Name), Engine.Renderer.DrawingType.LineList);
            drawingSurface.setDepthTesting(false);
            //Origin
            drawingSurface.Color = Color.Purple;

            Vector3 radiusAxis = Vector3.UnitX;
            Vector3 heightAxis = Vector3.UnitY;

            if(definition.LocalRotQuat.HasValue)
            {
                radiusAxis = Quaternion.quatRotate(definition.LocalRotQuat.Value, radiusAxis);
                heightAxis = Quaternion.quatRotate(definition.LocalRotQuat.Value, heightAxis);
            }

            drawingSurface.drawCylinder(Vector3.Zero, radiusAxis, heightAxis, definition.Radius, definition.Height);
            drawingSurface.end();
        }
    }
}

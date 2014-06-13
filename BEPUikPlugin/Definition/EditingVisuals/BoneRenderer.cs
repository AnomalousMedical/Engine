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
            drawingSurface.setColor(Color.Red);

            Vector3 localUnitX = Vector3.UnitX;
            Vector3 localUnitY = Vector3.UnitY;
            Vector3 localUnitZ = Vector3.UnitZ;

            if(definition.LocalRotQuat.HasValue)
            {
                localUnitX = Quaternion.quatRotate(definition.LocalRotQuat.Value, localUnitX);
                localUnitY = Quaternion.quatRotate(definition.LocalRotQuat.Value, localUnitY);
                localUnitZ = Quaternion.quatRotate(definition.LocalRotQuat.Value, localUnitZ);
            }

            drawingSurface.drawCircle(Vector3.Zero, localUnitX, localUnitZ, definition.Radius);
            float halfHeight = definition.Height;
            drawingSurface.drawCircle(localUnitY * halfHeight, localUnitX, localUnitZ, definition.Radius);
            drawingSurface.drawCircle(localUnitY * -halfHeight, localUnitX, localUnitZ, definition.Radius);
            drawingSurface.end();
        }
    }
}

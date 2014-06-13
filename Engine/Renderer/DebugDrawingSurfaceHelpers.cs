using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Renderer
{
    /// <summary>
    /// This class provides more drawing types that do not need to be implemented in every debug drawer.
    /// </summary>
    public static class DebugDrawingSurfaceHelpers
    {
        /// <summary>
        /// Draw a cylinder where the radius and height axis are known.
        /// </summary>
        /// <param name="drawingSurface">The DebugDrawingSurface to draw on.</param>
        /// <param name="origin">The origin of the cylinder, will be in the center of the cylinder.</param>
        /// <param name="radiusAxis">The axis along the radius of the cylinder.</param>
        /// <param name="heightAxis">The axis along the height of the cylinder.</param>
        /// <param name="radius">The raidus of the cylinder.</param>
        /// <param name="height">The height of the cylinder.</param>
        public static void drawCylinder(this DebugDrawingSurface drawingSurface, Vector3 origin, Vector3 radiusAxis, Vector3 heightAxis, float radius, float height)
        {
            drawCylinder(drawingSurface, origin, radiusAxis, heightAxis, radiusAxis.cross(ref heightAxis), radius, height);
        }

        /// <summary>
        /// Draw a cylinder where all three axes are known, saves a cross product vs the other draw cylinder function.
        /// </summary>
        /// <param name="drawingSurface">The DebugDrawingSurface to draw on.</param>
        /// <param name="origin">The origin of the cylinder, will be in the center of the cylinder.</param>
        /// <param name="radiusAxis">The axis along the radius of the cylinder.</param>
        /// <param name="heightAxis">The axis along the height of the cylinder.</param>
        /// <param name="otherAxis">The axis that is the cross product of the radius and height axis.</param>
        /// <param name="radius">The raidus of the cylinder.</param>
        /// <param name="height">The height of the cylinder.</param>
        public static void drawCylinder(this DebugDrawingSurface drawingSurface, Vector3 origin, Vector3 radiusAxis, Vector3 heightAxis, Vector3 otherAxis, float radius, float height)
        {
            drawingSurface.drawCircle(origin, radiusAxis, otherAxis, radius);
            float halfHeight = height / 2.0f;
            Vector3 start = origin + (heightAxis * halfHeight);
            Vector3 end = origin + (heightAxis * -halfHeight);
            drawingSurface.drawCircle(start, radiusAxis, otherAxis, radius);
            drawingSurface.drawCircle(end, radiusAxis, otherAxis, radius);
            drawingSurface.drawLine(start, end);
        }
    }
}

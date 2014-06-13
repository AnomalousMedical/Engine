using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Renderer
{
    /// <summary>
    /// How the DebugDrawingSurface will draw.
    /// </summary>
    public enum DrawingType
    {
        /// <summary>
        /// A list of points 1 vertex per point.
        /// </summary>
        PointList,
        /// <summary>
        /// A list of lines 2 vertices per line.
        /// </summary>
	    LineList,
        /// <summary>
        /// A strip of connected lines, 1 vertex per line plus 1 start vertex.
        /// </summary>
        LineStrip,
        /// <summary>
        /// A list of triangles 3 vertices per triangle.
        /// </summary>
	    TriangleList,
        /// <summary>
        /// A strip of triangles. 3 vertices for the first triangle and 1 per triangle after that.
        /// </summary>
        TriangleStrip,
        /// <summary>
        /// A fan of triangles, 3 vertices for the first triangle and 1 per triangle after that.
        /// </summary>
        TriangleFan
    }

    /// <summary>
    /// This exposes an OpenGL style drawing surface that can easily draw simple
    /// primitives. The specifics of how this is implemented are up the
    /// Rendering Plugin, however, users should always call the begin function
    /// before any drawing and call the end function when finished. It may also
    /// be possible to optimize the drawing a bit if the number of vertices is
    /// estimated. This only needs to be done once and is not required, but
    /// reccomended.
    /// </summary>
    public interface DebugDrawingSurface
    {
        /// <summary>
        /// Start defining drawing surface. Call before any drawing is done.
        /// </summary>
        /// <param name="sectionName">The name of the section. If it has not yet been created a new one will be made with the specified DrawingType.</param>
        /// <param name="drawingType">The DrawingType to use when a new section is created. This is ignored if the section is already defined.</param>
        void begin(String sectionName, DrawingType drawingType);

        /// <summary>
        /// Finish defining the drawing surface. Call when all drawing is finished.
        /// </summary>
        void end();

        /// <summary>
        /// Clear everything in this drawing surface.
        /// </summary>
        void clearAll();

        /// <summary>
        /// Tell the debug drawer to draw itself with depth testing enabled or disabled.
        /// </summary>
        /// <param name="depthCheckEnabled">True to enable depth checking.</param>
        void setDepthTesting(bool depthCheckEnabled);

        /// <summary>
        /// Check to see if depth testing is enabled.
        /// </summary>
        /// <returns>True if depth testing is enabled.</returns>
        bool isDepthTestingEnabled();

        /// <summary>
        /// Move the origin to a new location.
        /// </summary>
        /// <param name="newOrigin">The new origin.</param>
        void moveOrigin(Vector3 newOrigin);

        /// <summary>
        /// Get the origin of this surface.
        /// </summary>
        /// <returns>The origin of the surface.</returns>
        Vector3 getOrigin();

        /// <summary>
        /// Change the orientation of the surface.
        /// </summary>
        /// <param name="orientation">The orientation to set.</param>
        void setOrientation(Quaternion orientation);

        /// <summary>
        /// Get the orientation of the surface.
        /// </summary>
        /// <returns>The orientation of the surface.</returns>
        Quaternion getOrientation();

        /// <summary>
        /// Set the visibility of the DebugDrawingSurface.
        /// </summary>
        /// <param name="visible">True to be visible false to be invisible.</param>
        void setVisible(bool visible);

        /// <summary>
        /// Set the current color that will be used when drawing.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Draw a single point. For simplest results use PointList.
        /// </summary>
        /// <param name="p">The point to draw.</param>
        void drawPoint(Vector3 p);

        /// <summary>
        /// Draw a line between p1 and p2. How this line is represented depends
        /// on the DrawingType. For simplest results use LineList.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        void drawLine(Vector3 p1, Vector3 p2);

        /// <summary>
        /// Draw a single triangle. For simplest results use TriangleList.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <param name="p3">The third point.</param>
        void drawTriangle(Vector3 p1, Vector3 p2, Vector3 p3);

        /// <summary>
        /// Draw a circle. For easiest results use LineList.
        /// </summary>
        /// <param name="origin">The origin of the circle.</param>
        /// <param name="xAxis">A vector along the X-Axis of the circle.</param>
        /// <param name="yAxis">A vector along the Y-Axis of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        void drawCircle(Vector3 origin, Vector3 xAxis, Vector3 yAxis, float radius);
    }
}

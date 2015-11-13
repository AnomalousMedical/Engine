using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class Geometry
    {
        /// <summary>
        /// Calculate area from any 3 arbitrary points. Will be positive if clockwise and negative
        /// if counter-clockwise
        /// </summary>
        /// <param name="p0">1st point</param>
        /// <param name="p1">2nd point</param>
        /// <param name="p2">3rd point</param>
        /// <returns></returns>
        public static float SignedAreaOfTriangle(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            //p0 is our origin point
            return ((p1.x - p0.x) * (p2.y - p0.y) - (p2.x - p0.x) * (p1.y - p0.y)) / 2.0f;
        }

        /// <summary>
        /// Calculate area from two vectors describing the two length of the legs 
        /// of the triangle coming from the origin. Will be positive if clockwise and
        /// negative if counter-clockwise.
        /// </summary>
        /// <param name="s0">The first side</param>
        /// <param name="s1">The second side</param>
        /// <returns></returns>
        public static float SignedAreaOfTriangle(Vector2 s0, Vector2 s1)
        {
            return (-s1.x * s0.y + s0.x * s1.y) / 2.0f;
        }

        /// <summary>
        /// Calculate the area of a polygon specified by points. The first point in points
        /// will be automatically closed with the last point. Will be positive if clockwise and
        /// negative if counter-clockwise.
        /// </summary>
        /// <param name="points">An enumerator over the points to calculate the area of.</param>
        /// <returns></returns>
        public static float SignedAreaOfPolygon(IEnumerable<Vector2> points)
        {
            float area = 0.0f;
            var ptEnum = points.GetEnumerator();
            if (ptEnum.MoveNext())
            {
                Vector2 first = ptEnum.Current;
                Vector2 previous = first;
                if (ptEnum.MoveNext())
                {
                    Vector2 current;
                    do
                    {
                        current = ptEnum.Current;
                        area += SignedAreaOfTriangle(previous, current);
                        previous = current;
                    }
                    while (ptEnum.MoveNext());

                    //Calc the last to first poly
                    area += SignedAreaOfTriangle(current, first);
                }
            }
            return area;
        }

        /// <summary>
        /// Calculate the area of a polygon specified by points. The first point in points
        /// will be automatically closed with the last point. Always positive.
        /// </summary>
        /// <param name="points">An enumerator over the points to calculate the area of.</param>
        /// <returns></returns>
        public static float AreaOfPolygon(IEnumerable<Vector2> points)
        {
            return Math.Abs(SignedAreaOfPolygon(points));
        }

        public static float SignedVolumeOfTetrahedron(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float v321 = p3.x * p2.y * p1.z;
            float v231 = p2.x * p3.y * p1.z;
            float v312 = p3.x * p1.y * p2.z;
            float v132 = p1.x * p3.y * p2.z;
            float v213 = p2.x * p1.y * p3.z;
            float v123 = p1.x * p2.y * p3.z;
            return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
        }
        public static float VolumeOfMesh(Vector3[] vertices, int[] indices)
        {
            float volume = 0;
            int numTriangles = indices.Length;
            for (int i = 0; i < numTriangles; i += 3)
         {
                Vector3 p1 = vertices[indices[i + 0]];
                Vector3 p2 = vertices[indices[i + 1]];
                Vector3 p3 = vertices[indices[i + 2]];
                volume += SignedVolumeOfTetrahedron(p1, p2, p3);
            }
            return Math.Abs(volume);
        }



    }
}

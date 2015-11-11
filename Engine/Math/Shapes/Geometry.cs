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
        /// Calculate area from any 3 arbitrary points
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
        /// of the triangle coming from the origin
        /// </summary>
        /// <param name="s0">The first side</param>
        /// <param name="s1">The second side</param>
        /// <returns></returns>
        public static float SignedAreaOfTriangle(Vector2 s0, Vector2 s1)
        {
            return (-s1.x * s0.y + s0.x * s1.y) / 2.0f;
        }

        /// <summary>
        /// Calculate the area of a polygon specified by points, 
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static float AreaOfPolygon(IEnumerable<Vector2> points)
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Engine
{
    /// <summary>
    /// This class defines a box that is specified by a center, three
    /// orthonormal axes that form a right handed coordinate system and three
    /// extents that represent the distance from the center to the edge (half
    /// extents).
    /// </summary>
    public class Box3
    {
        private Vector3 center;
	    private Vector3[] axis = new Vector3[3];
        private Vector3 extents;

        /// <summary>
	    /// Constructor.
	    /// </summary>
        public Box3()
        {

        }

	    /// <summary>
	    /// Get the center of the box.
	    /// </summary>
	    /// <returns>The center of the box.</returns>
        public Vector3 getCenter()
        {
            return center;
        }

	    /// <summary>
	    /// Set the center of the box.
	    /// </summary>
	    /// <param name="center">The new center for the box.</param>
        public void setCenter(Vector3 center)
        {
            this.center = center;
        }

	    /// <summary>
	    /// Get the specified axis in the box.
	    /// </summary>
	    /// <param name="i">The axis at index i.  Valid between 0 and 2.</param>
	    /// <returns>The axis at the given index.</returns>
        public Vector3 getAxis(int i)
        {
            return axis[i];
        }

	    /// <summary>
	    /// Get the axes of the box.  The returned array can also be used to set the individual axes.
	    /// </summary>
	    /// <returns>The axes array for this box.</returns>
        public Vector3[] getAxes()
        {
            return axis;
        }

	    /// <summary>
	    /// Set the extents for this box.  The extents are the distance from the center of the box 
	    /// to the edge (half extents).
	    /// </summary>
	    /// <param name="extents">The extents to set.</param>
        public void setExtents(Vector3 extents)
        {
            this.extents = extents;
        }

	    /// <summary>
	    /// Get the extents for this box.
	    /// </summary>
	    /// <returns>The box's extents.</returns>
        public Vector3 getExtents()
        {
            return extents;
        }

	    /// <summary>
	    /// Compute the vertices of this box and put them in the vertices array.
        /// 0-3 are in the -z direction
        /// 4-7 are in the +z direction
        /// The points make up the sides in the + and - z plane counter clockwise starting from the -x, -y point.
	    /// </summary>
	    /// <param name="vertices">An 8 element array of vertices.</param>
        public void computeVertices(Vector3[] vertices)
        {
            Vector3[] extentAxis =
            {
                extents.x*axis[0],
                extents.y*axis[1],
                extents.z*axis[2]
            };

            vertices[0] = center - extentAxis[0] - extentAxis[1] - extentAxis[2];
            vertices[1] = center + extentAxis[0] - extentAxis[1] - extentAxis[2];
            vertices[2] = center + extentAxis[0] + extentAxis[1] - extentAxis[2];
            vertices[3] = center - extentAxis[0] + extentAxis[1] - extentAxis[2];
            vertices[4] = center - extentAxis[0] - extentAxis[1] + extentAxis[2];
            vertices[5] = center + extentAxis[0] - extentAxis[1] + extentAxis[2];
            vertices[6] = center + extentAxis[0] + extentAxis[1] + extentAxis[2];
            vertices[7] = center - extentAxis[0] + extentAxis[1] + extentAxis[2];
        }

        public bool isInside(Vector3 point)
        {
            point -= center;
            point = point.absolute();
            if (point.x <= extents.x && point.y <= extents.y && point.z <= extents.z)
            {
                return true;
            }
            return false;
        }

	    /// <summary>
	    /// Test to see if the given ray intersects this box.  This passes the ray by value.
	    /// </summary>
	    /// <param name="ray">The ray to test for intersection.</param>
	    /// <returns>True if they ray intersects the box.</returns>
        public bool testIntersection(Ray3 ray)
        {
            return testIntersection(ref ray);
        }

	    /// <summary>
	    /// Test to see if the given ray intersects this box.
	    /// </summary>
	    /// <param name="ray">The ray to test for intersection.</param>
	    /// <returns>True if they ray intersects the box.</returns>
        public bool testIntersection(ref Ray3 ray)
        {
            float[] fWdU = new float[3];
	        float[] fAWdU = new float[3];
	        float[] fDdU = new float[3];
	        float[] fADdU = new float[3];
	        float[] fAWxDdU = new float[3];
	        float fRhs;

            Vector3 kDiff = ray.Origin - center;

            fWdU[0] = ray.Direction.dot(ref axis[0]);
	        fAWdU[0] = System.Math.Abs(fWdU[0]);
            fDdU[0] = kDiff.dot(ref axis[0]);
            fADdU[0] = System.Math.Abs(fDdU[0]);
            if ( fADdU[0] > extents.x && fDdU[0]*fWdU[0] >= 0.0f )
	        {
                return false;
	        }

            fWdU[1] = ray.Direction.dot(ref axis[1]);
            fAWdU[1] = System.Math.Abs(fWdU[1]);
            fDdU[1] = kDiff.dot(ref axis[1]);
            fADdU[1] = System.Math.Abs(fDdU[1]);
            if ( fADdU[1] > extents.y && fDdU[1]*fWdU[1] >= 0.0f )
	        {
                return false;
	        }

            fWdU[2] = ray.Direction.dot(ref axis[2]);
            fAWdU[2] = System.Math.Abs(fWdU[2]);
            fDdU[2] = kDiff.dot(ref axis[2]);
            fADdU[2] = System.Math.Abs(fDdU[2]);
            if ( fADdU[2] > extents.z && fDdU[2]*fWdU[2] >= 0.0f )
	        {
                return false;
	        }

            Vector3 kWxD = ray.Direction.cross(ref kDiff);

            fAWxDdU[0] = System.Math.Abs(kWxD.dot(ref axis[0]));
            fRhs = extents.y*fAWdU[2] + extents.z*fAWdU[1];
            if ( fAWxDdU[0] > fRhs )
	        {
                return false;
	        }

            fAWxDdU[1] = System.Math.Abs(kWxD.dot(ref axis[1]));
            fRhs = extents.x*fAWdU[2] + extents.z*fAWdU[0];
            if ( fAWxDdU[1] > fRhs )
	        {
                return false;
	        }

            fAWxDdU[2] = System.Math.Abs(kWxD.dot(ref axis[2]));
            fRhs = extents.x*fAWdU[1] + extents.y*fAWdU[0];
            if ( fAWxDdU[2] > fRhs )
	        {
                return false;
	        }

            return true;
        }

	    /// <summary>
	    /// <para>
	    /// Clipping of a linear component 'origin'+t*'direction' against an
	    /// this axis aligned box.
	    /// </para>
	    /// <para>
	    /// The values of t0 and t1 must be set by the caller.  If the component is a
	    /// segment, set t0 = 0 and t1 = 1.  If the component is a ray, set t0 = 0 and
	    /// t1 = INFINITY.  If the component is a line, set t0 = -INFINITY and
	    /// t1 = INFINITY.  The values are (possibly) modified by the clipper.
	    /// </para>
	    /// </summary>
	    /// <param name="origin">The origin of the line.</param>
	    /// <param name="direction">The direction of the line.</param>
	    /// <param name="t0">Set to 0 for a ray and -INFINITY for a line.  This value will be modified by the clipper if it intersects the box.</param>
	    /// <param name="t1">Set to INFINITY for both a line and a ray.  This value will be modified by the clipper if it intersects the box.</param>
	    /// <returns>True if the line was not completely clipped by the box.</returns>
        public bool clipLine(ref Vector3 origin, ref Vector3 direction, ref float t0, ref float t1)
        {
            float saveT0 = t0, saveT1 = t1;

            bool bNotEntirelyClipped =
                Clip(+direction.x, -origin.x - extents.x, ref t0, ref t1) &&
                Clip(-direction.x, +origin.x - extents.x, ref t0, ref t1) &&
                Clip(+direction.y, -origin.y - extents.y, ref t0, ref t1) &&
                Clip(-direction.y, +origin.y - extents.y, ref t0, ref t1) &&
                Clip(+direction.z, -origin.z - extents.z, ref t0, ref t1) &&
                Clip(-direction.z, +origin.z - extents.z, ref t0, ref t1);

            return bNotEntirelyClipped && (t0 != saveT0 || t1 != saveT1);
        }

	    /// <summary>
	    /// <para>
	    /// Clipping of a linear component 'origin'+t*'direction' against an
	    /// this axis aligned box.
	    /// </para>
	    /// <para>
	    /// The values of t0 and t1 must be set by the caller.  If the component is a
	    /// segment, set t0 = 0 and t1 = 1.  If the component is a ray, set t0 = 0 and
	    /// t1 = float.MaxValue.  If the component is a line, set t0 = float.MinValue and
	    /// t1 = float.MaxValue.  The values are (possibly) modified by the clipper.
	    /// </para>
	    /// </summary>
	    /// <param name="origin">The origin of the line.</param>
	    /// <param name="direction">The direction of the line.</param>
	    /// <param name="t0">Set to 0 for a ray and float.MinValue for a line.  This value will be modified by the clipper if it intersects the box.</param>
	    /// <param name="t1">Set to float.MaxValue for both a line and a ray.  This value will be modified by the clipper if it intersects the box.</param>
	    /// <returns>True if the line was not completely clipped by the box.</returns>
        public bool clipLine(Vector3 origin, Vector3 direction, ref float t0, ref float t1)
        {
            return clipLine(ref origin, ref direction, ref t0, ref t1);
        }

	    /// <summary>
	    /// Find the intersection of the given ray and this box.  If this funciton returns
	    /// true then the ray intersected the box and the vectors will be populated with
	    /// the intersection points.  The quantity variable will tell how many of the vectors
	    /// are filled.  If it is 1 then point0 has an intersection point if it is 2 then point0
	    /// and point1 have an intersection point.
	    /// </summary>
	    /// <param name="ray">The ray to check for intersection.</param>
	    /// <param name="quantity">The number of points of intersection.</param>
	    /// <param name="point0">The first point of intersection.</param>
	    /// <param name="point1">The second point of intersection.</param>
	    /// <returns>True if an intersection occured.  The quantity tells how many of the points have an intersection value.  If this returns false no intersection occured.</returns>
        public bool findIntersection(ref Ray3 ray, out int quantity, out Vector3 point0, out Vector3 point1)
        {
            // convert ray to box coordinates
            point0 = Vector3.Zero;
            point1 = Vector3.Zero;
            Vector3 kDiff = ray.Origin - center;
            Vector3 kOrigin = new Vector3(kDiff.dot(ref axis[0]), kDiff.dot(ref axis[1]), kDiff.dot(ref axis[2]));
            Vector3 kDirection = new Vector3(ray.Direction.dot(ref axis[0]), ray.Direction.dot(ref axis[1]), ray.Direction.dot(ref axis[2]));

	        float fT0 = 0.0f, fT1 = float.MaxValue;
            bool bIntersects = clipLine(ref kOrigin, ref kDirection, ref fT0, ref fT1);

            if ( bIntersects )
            {
                if ( fT0 > 0.0 )
                {
                    if ( fT1 < float.MaxValue )
                    {
                        quantity = 2;
                        point0 = ray.Origin + fT0*ray.Direction;
                        point1 = ray.Origin + fT1*ray.Direction;
                    }
                    else
                    {
                        quantity = 1;
                        point0 = ray.Origin + fT0*ray.Direction;
                    }
                }
                else  // fT0 == 0.0
                {
                    if ( fT1 < float.MaxValue )
                    {
                        quantity = 1;
                        point0 = ray.Origin + fT1 * ray.Direction;
                    }
                    else  // fT1 == INFINITY
                    {
                        // assert:  should not get here
                        quantity = 0;
                    }
                }
            }
            else
            {
                quantity = 0;
            }

            return bIntersects;
        }

	    /// <summary>
	    /// Find the intersection of the given ray and this box.  If this funciton returns
	    /// true then the ray intersected the box and the vectors will be populated with
	    /// the intersection points.  The quantity variable will tell how many of the vectors
	    /// are filled.  If it is 1 then point0 has an intersection point if it is 2 then point0
	    /// and point1 have an intersection point.
	    /// </summary>
	    /// <param name="ray">The ray to check for intersection.</param>
	    /// <param name="quantity">Out variable.  The number of points of intersection.</param>
	    /// <param name="point0">The first point of intersection.</param>
	    /// <param name="point1">Out variable.  The second point of intersection.</param>
	    /// <returns>True if an intersection occured.  The quantity tells how many of the points have an intersection value.  If this returns false no intersection occured.</returns>
        public bool findIntersection(Ray3 ray, out int quantity, out Vector3 point0, out Vector3 point1)
        {
            return findIntersection(ref ray, out quantity, out point0, out point1);
        }

	    /// <summary>
	    /// Clip the line segment against a plane.
	    /// </summary>
	    /// <param name="denom"></param>
	    /// <param name="numer"></param>
	    /// <param name="t0"></param>
	    /// <param name="t1"></param>
	    /// <returns></returns>
        public static bool Clip(float denom, float numer, ref float t0, ref float t1)
        {
            // Return value is 'true' if line segment intersects the current test
            // plane.  Otherwise 'false' is returned in which case the line segment
            // is entirely clipped.

            if (denom > 0.0)
            {
                if (numer > denom * t1)
                    return false;
                if (numer > denom * t0)
                    t0 = numer / denom;
                return true;
            }
            else if (denom < 0.0)
            {
                if (numer > denom * t0)
                    return false;
                if (numer > denom * t1)
                    t1 = numer / denom;
                return true;
            }
            else
            {
                return numer <= 0.0;
            }
        }
    }
}

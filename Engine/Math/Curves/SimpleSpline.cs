using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// The Ogre SimpleSpline class, but we have renamed it to CatmullRomSpline since that is what it is.
    /// </summary>
    /// <remarks>
    /// Note that this version has not automatic calculation.
    /// </remarks>
    public class CatmullRomSpline : Spline3d
    {
        List<Vector3> mPoints = new List<Vector3>();
        List<Vector3> mTangents = new List<Vector3>();
        Matrix4x4 mCoeffs; // Matrix of coefficients 

        public CatmullRomSpline()
        {
            // Set up matrix
            // Hermite polynomial
            mCoeffs = new Matrix4x4(
                    2, -2, 1, 1,
                    -3, 3, -2, -1,
                    0, 0, 1, 0,
                    1, 0, 0, 0
                );
        }

        /// <summary>
        /// Adds a control point to the end of the spline.
        /// </summary>
        /// <param name="p"></param>
        public void addControlPoint(Vector3 p)
        {
            mPoints.Add(p);
        }

        /// <summary>
        /// Remove a control point from the set. You must call computeSplines to
        /// update the changes.
        /// </summary>
        /// <param name="index">The index of the control point to remove.</param>
        public void removeControlPoint(int index)
        {
            mPoints.RemoveAt(index);
        }

        /// <summary>
        /// Gets the detail of one of the control points of the spline.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 getControlPoint(int index)
        {
            return mPoints[index];
        }

        /// <summary>
        /// Clears all the points in the spline.
        /// </summary>
        public void clear()
        {
            mPoints.Clear();
            mTangents.Clear();
        }

        /// <summary>
        /// Updates a single point in the spline.
        /// </summary>
        /// <remarks>
        /// This point must already exist in the spline.
        /// </remarks>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void updateControlPoint(int index, Vector3 value)
        {
            mPoints[index] = value;
        }

        /// <summary>
        /// Returns an interpolated point based on a parametric value over the whole series.
        /// </summary>
        /// <remarks>
        /// Given a t value between 0 and 1 representing the parametric distance along the
        /// whole length of the spline, this method returns an interpolated point.
        /// </remarks>
        /// <param name="t">Parametric value.</param>
        /// <returns></returns>
        public Vector3 interpolate(float t)
        {
            // Currently assumes points are evenly spaced, will cause velocity
            // change where this is not the case, you can use GetEqualDistanceSpline to
            // make a new spline that has evenly spaced points


            // Work out which segment this is in
            float fSeg = t * (mPoints.Count - 1);
            int segIdx = (int)fSeg;
            // Apportion t 
            t = fSeg - segIdx;

            return interpolate(segIdx, t);
        }

        /// <summary>
        /// Interpolates a single segment of the spline given a parametric value.
        /// </summary>
        /// <param name="fromIndex">The point index to treat as t=0. fromIndex + 1 is deemed to be t=1</param>
        /// <param name="t">Parametric value</param>
        /// <returns></returns>
        public Vector3 interpolate(int fromIndex, float t)
        {
            // Bounds check
            //assert (fromIndex < mPoints.size() &&
            //    "fromIndex out of bounds");

            if ((fromIndex + 1) == mPoints.Count)
            {
                // Duff request, cannot blend to nothing
                // Just return source
                return mPoints[fromIndex];

            }

            // Fast special cases
            if (t == 0.0f)
            {
                return mPoints[fromIndex];
            }
            else if (t == 1.0f)
            {
                return mPoints[fromIndex + 1];
            }

            // Real interpolation
            // Form a vector of powers of t
            float t2, t3;
            t2 = t * t;
            t3 = t2 * t;
            Vector4 powers = new Vector4(t3, t2, t, 1);


            // Algorithm is ret = powers * mCoeffs * Matrix4(point1, point2, tangent1, tangent2)
            Vector3 point1 = mPoints[fromIndex];
            Vector3 point2 = mPoints[fromIndex + 1];
            Vector3 tan1 = mTangents[fromIndex];
            Vector3 tan2 = mTangents[fromIndex + 1];
            Matrix4x4 pt = new Matrix4x4(
                    point1.x, point1.y, point1.z, 1.0f,
                    point2.x, point2.y, point2.z, 1.0f,
                    tan1.x, tan1.y, tan1.z, 1.0f,
                    tan2.x, tan2.y, tan2.z, 1.0f
                );

            Vector4 ret = powers * mCoeffs * pt;


            return new Vector3(ret.x, ret.y, ret.z);
        }

        /// <summary>
        /// Figure out the rotation at a given point on the curve.
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="offset"></param>
        /// <param name="startingDirection"></param>
        /// <returns></returns>
        public Quaternion interpolateRotation(float percent, float offset, Vector3 startingDirection)
        {
            //Fix the numbers
            if (percent == 1.0f)
            {
                percent = 1.0f - offset;
            }
            else if (percent + offset > 1.0f)
            {
                offset = 1.0f - percent;
            }
            Vector3 curvePosition = interpolate(percent + offset) - interpolate(percent);
            return Quaternion.shortestArcQuatNormalize2(ref startingDirection, ref curvePosition);
        }

        /// <summary>
        /// An enumeration over the control points.
        /// </summary>
        public IEnumerable<Vector3> ControlPoints
        {
            get
            {
                return mPoints;
            }
        }

        /// <summary>
        /// Gets the number of control points in the spline.
        /// </summary>
        public int NumControlPoints
        {
            get
            {
                return mPoints.Count;
            }
        }

        /// <summary>
        /// Recalculates the tangents associated with this spline. 
        /// </summary>
        /// <remarks>
        /// If you tell the spline not to update on demand by calling setAutoCalculate(false)
        /// then you must call this after completing your updates to the spline points.
        /// </remarks>
        public void computeSplines()
        {
            // Catmull-Rom approach
            // 
            // tangent[i] = 0.5 * (point[i+1] - point[i-1])
            //
            // Assume endpoint tangents are parallel with line with neighbour

            int i, numPoints;
            bool isClosed;

            numPoints = mPoints.Count;
            if (numPoints < 2)
            {
                // Can't do anything yet
                return;
            }

            // Closed or open?
            if (mPoints[0] == mPoints[numPoints - 1])
            {
                isClosed = true;
            }
            else
            {
                isClosed = false;
            }

            mTangents = new List<Vector3>(numPoints);


            for (i = 0; i < numPoints; ++i)
            {
                if (i == 0)
                {
                    // Special case start
                    if (isClosed)
                    {
                        // Use numPoints-2 since numPoints-1 is the last point and == [0]
                        mTangents.Add(0.5f * (mPoints[1] - mPoints[numPoints - 2]));
                    }
                    else
                    {
                        mTangents.Add(0.5f * (mPoints[1] - mPoints[0]));
                    }
                }
                else if (i == numPoints - 1)
                {
                    // Special case end
                    if (isClosed)
                    {
                        // Use same tangent as already calculated for [0]
                        mTangents.Add(mTangents[0]);
                    }
                    else
                    {
                        mTangents.Add(0.5f * (mPoints[i] - mPoints[i - 1]));
                    }
                }
                else
                {
                    mTangents.Add(0.5f * (mPoints[i + 1] - mPoints[i - 1]));
                }

            }
        }

        /// <summary>
        /// Calculate a new spline where the control points are equal distant from each other. This makes interpolate
        /// work better.
        /// </summary>
        /// <remarks>
        /// From http://www.ogre3d.org/forums/viewtopic.php?p=224334, magnumpc's post that fixes issues, 
        /// although it used an undeclared variable.
        /// </remarks>
        /// <param name="splineSrc">The source spline.</param>
        /// <param name="splineDest">The spline to save the results into.</param>
        /// <param name="wantedDistance">The desired distance between two nodes.</param>
        public static void GetEqualDistanceSpline(CatmullRomSpline splineSrc, CatmullRomSpline splineDest, float wantedDistance)
        {
            float lastInterpPoint = 0.0f;
            float length;
            Vector3 start = splineSrc.getControlPoint(0);
            Vector3 end;
            float wantedDistanceSquared = wantedDistance * wantedDistance;

            splineDest.addControlPoint(start);

            for (int j = 1; j < splineSrc.NumControlPoints; )
            {
                // first find the points where the length exceed wanted length..
                end = splineSrc.getControlPoint(j);
                length = (end - start).length2();

                while (length < wantedDistanceSquared && j < splineSrc.NumControlPoints - 1)
                {
                    end = splineSrc.getControlPoint(++j);
                    length = (end - start).length2();
                    // if enter the loops then we have to reset lastInterPoint..
                    lastInterpPoint = 0.0f;
                }

                // Moved to the end of the "for" loop and changed (see below)
                //      if (j == splineSrc.getNumPoints() -1)
                //         break;

                // okay found it.. lets refine
                float partStart = lastInterpPoint;
                float partEnd = 1.0f;
                float partMid;
                Vector3 partPoint;
                float partLen;
                Vector3 refPoint = splineSrc.interpolate(j - 1, lastInterpPoint);
                float squaredDist = wantedDistance - (start - refPoint).length();
                squaredDist *= squaredDist;

                do
                {
                    partMid = (partStart + partEnd) / 2;
                    partPoint = splineSrc.interpolate(j - 1, partMid);
                    partLen = (partPoint - refPoint).length2();
                    if (Math.Abs(partLen - squaredDist) < 1 || Math.Abs(partStart - partEnd) < 1e-5)
                        break;
                    if (partLen > squaredDist)
                        partEnd = partMid;
                    else
                        partStart = partMid;
                } while (true);

                // once we reach here.. the exact point has been discovered..
                start = splineSrc.interpolate(j - 1, partMid);

                // and remember the last interpolation point
                lastInterpPoint = partMid;

                splineDest.addControlPoint(start);

                //
                // Moved from above; this was exiting too soon and 
                // not including nodes for the last leg of a spline.
                //
                if ((length < wantedDistanceSquared) && (j == splineSrc.NumControlPoints - 1))
                {
                    break;
                }
            }
            splineDest.computeSplines();
        }
    }
}

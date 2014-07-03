using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// The Ogre SimpleSpline class.
    /// </summary>
    public class SimpleSpline
    {
        public SimpleSpline()
        {
            // Set up matrix
            // Hermite polynomial
            mCoeffs = new Matrix4x4(
                    2, -2, 1, 1,
                    -3, 3, -2, -1,
                    0, 0, 1, 0,
                    1, 0, 0, 0
                );

            mAutoCalc = true;
        }

        /** Adds a control point to the end of the spline. */
        public void addPoint(Vector3 p)
        {
            mPoints.Add(p);
            if(mAutoCalc)
            {
                recalcTangents();
            }
        }

        /** Gets the detail of one of the control points of the spline. */
        public Vector3 getPoint(int index)
        {
            return mPoints[index];
        }

        /** Gets the number of control points in the spline. */
        public int getNumPoints()
        {
            return mPoints.Count;
        }

        /** Clears all the points in the spline. */
        public void clear()
        {
            mPoints.Clear();
            mTangents.Clear();
        }

        /** Updates a single point in the spline. 
        @remarks
            This point must already exist in the spline.
        */
        public void updatePoint(int index, Vector3 value)
        {
            mPoints[index] = value;
        }

        /** Returns an interpolated point based on a parametric value over the whole series.
        @remarks
            Given a t value between 0 and 1 representing the parametric distance along the
            whole length of the spline, this method returns an interpolated point.
        @param t Parametric value.
        */
        public Vector3 interpolate(float t)
        {
            // Currently assumes points are evenly spaced, will cause velocity
            // change where this is not the case
            // TODO: base on arclength?

        
            // Work out which segment this is in
            float fSeg = t * (mPoints.Count - 1);
            int segIdx = (int)fSeg;
            // Apportion t 
            t = fSeg - segIdx;

            return interpolate(segIdx, t);
        }

        /** Interpolates a single segment of the spline given a parametric value.
        @param fromIndex The point index to treat as t=0. fromIndex + 1 is deemed to be t=1
        @param t Parametric value
        */
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
            else if(t == 1.0f)
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
            Vector3 point2 = mPoints[fromIndex+1];
            Vector3 tan1 = mTangents[fromIndex];
            Vector3 tan2 = mTangents[fromIndex+1];
            Matrix4x4 pt = new Matrix4x4(
                    point1.x, point1.y, point1.z, 1.0f,
                    point2.x, point2.y, point2.z, 1.0f,
                    tan1.x, tan1.y, tan1.z, 1.0f,
                    tan2.x, tan2.y, tan2.z, 1.0f
                );

            Vector4 ret = powers * mCoeffs * pt;


            return new Vector3(ret.x, ret.y, ret.z);
        }


        /** Tells the spline whether it should automatically calculate tangents on demand
            as points are added.
        @remarks
            The spline calculates tangents at each point automatically based on the input points.
            Normally it does this every time a point changes. However, if you have a lot of points
            to add in one go, you probably don't want to incur this overhead and would prefer to 
            defer the calculation until you are finished setting all the points. You can do this
            by calling this method with a parameter of 'false'. Just remember to manually call 
            the recalcTangents method when you are done.
        @param autoCalc If true, tangents are calculated for you whenever a point changes. If false, 
            you must call reclacTangents to recalculate them when it best suits.
        */
        public bool AutoCalculate
        {
            get
            {
                return mAutoCalc;
            }
            set
            {
                mAutoCalc = value;
            }
        }

        public IEnumerable<Vector3> ControlPoints
        {
            get
            {
                return mPoints;
            }
        }

        /** Recalculates the tangents associated with this spline. 
        @remarks
            If you tell the spline not to update on demand by calling setAutoCalculate(false)
            then you must call this after completing your updates to the spline points.
        */
        public void recalcTangents()
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

        bool mAutoCalc;
        List<Vector3> mPoints = new List<Vector3>();
        List<Vector3> mTangents = new List<Vector3>();
        /// Matrix of coefficients 
        Matrix4x4 mCoeffs;
    }
}

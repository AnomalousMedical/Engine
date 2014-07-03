using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This is a set of cubic NaturalSplines that connect a series of control points.
    /// </summary>
    public class NaturalSplineSet3d
    {
        List<Vector3> controlPoints = new List<Vector3>();
        List<CubicSpline> xSpline = new List<CubicSpline>();
        List<CubicSpline> ySpline = new List<CubicSpline>();
        List<CubicSpline> zSpline = new List<CubicSpline>();

        /// <summary>
        /// Add a control point to the set. You must call computeSplines to
        /// update the changes.
        /// </summary>
        /// <param name="controlPoint">The control point to add.</param>
        public void addControlPoint(Vector3 controlPoint)
        {
            controlPoints.Add(controlPoint);
        }

        /// <summary>
        /// Remove a control point from the set. You must call computeSplines to
        /// update the changes.
        /// </summary>
        /// <param name="index">The index of the control point to remove.</param>
        public void removeControlPoint(int index)
        {
            controlPoints.RemoveAt(index);
        }

        /// <summary>
        /// Update the control point at a given index. You must call
        /// computeSplines to update the changes.
        /// </summary>
        /// <param name="index">The index of the control point to update.</param>
        /// <param name="value">The value to set at index.</param>
        public void updateControlPoint(int index, Vector3 value)
        {
            controlPoints[index] = value;
        }

        /// <summary>
        /// Get the number of control points in the spline set.
        /// </summary>
        /// <returns>The number of control points.</returns>
        public int getNumControlPoints()
        {
            return controlPoints.Count;
        }

        /// <summary>
        /// Get a specific control point in the spline set.
        /// </summary>
        /// <param name="index">The index of the control point to retrieve.</param>
        /// <returns>The index of the control point.</returns>
        public Vector3 getControlPoint(int index)
        {
            return controlPoints[index];
        }

        /// <summary>
        /// An enumerator over all the control points.
        /// </summary>
        public IEnumerable<Vector3> ControlPoints
        {
            get
            {
                return controlPoints;
            }
        }

        /// <summary>
        /// Get the value of the spline at a certain percentage.
        /// </summary>
        /// <param name="percent">A value between 0 and 1 for the position on the spline.</param>
        /// <returns>A Vector3 with the location of the spline.</returns>
        public Vector3 interpolate(float percent)
        {
            int i;
            if (percent < xSpline[xSpline.Count - 1].xt)
            {
                for (i = 0; percent >= xSpline[i].xt; ++i)
                {

                }
                i = i - 1;
            }
            else
            {
                i = xSpline.Count - 1;
            }
            return new Vector3(xSpline[i].interpolate(percent), ySpline[i].interpolate(percent), zSpline[i].interpolate(percent));
        }

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
        /// Recompute the splines in the set. This must be called every time the
        /// control points are changed.
        /// </summary>
        public void computeSplines()
        {
            xSpline.Clear();
            ySpline.Clear();
            zSpline.Clear();
            if (controlPoints.Count > 0)
            {
                int n = controlPoints.Count - 1; //n is the number of splines.
                //Compute s values between 0 and 1 for the parametric splines
                float totalLength = 0;
                float[] s = new float[n + 1];
                s[0] = 0f;
                for (int i = 0; i < n; ++i)
                {
                    totalLength += (controlPoints[i + 1] - controlPoints[i]).length();
                    s[i + 1] = totalLength;
                }
                for (int i = 1; i < n + 1; ++i)
                {
                    s[i] /= totalLength;
                }

                //allocate
                float[] a = new float[n + 1];
                float[] b = new float[n];
                float[] d = new float[n];
                float[] h = new float[n];
                float[] alpha = new float[n];
                float[] c = new float[n + 1];
                float[] l = new float[n + 1];
                float[] u = new float[n + 1];
                float[] z = new float[n + 1];

                //Compute the splines for each dimension
                //X dimension
                for (int i = 0; i <= n; ++i)
                {
                    a[i] = controlPoints[i].x;
                }
                //common
                for (int i = 0; i < n; ++i)
                {
                    h[i] = s[i + 1] - s[i];
                }
                for (int i = 1; i < n; ++i)
                {
                    alpha[i] = 3 / h[i] * (a[i + 1] - a[i]) - 3 / h[i - 1] * (a[i] - a[i - 1]);
                }
                l[0] = 1;
                u[0] = z[0] = 0;
                for (int i = 1; i < n; ++i)
                {
                    l[i] = 2 * (s[i + 1] - s[i - 1]) - h[i - 1] * u[i - 1];
                    u[i] = h[i] / l[i];
                    z[i] = (alpha[i] - h[i - 1] * z[i - 1]) / l[i];
                }
                l[n] = 1;
                z[n] = c[n] = 0;
                for (int j = n - 1; j >= 0; --j)
                {
                    c[j] = z[j] - u[j] * c[j + 1];
                    b[j] = (a[j + 1] - a[j]) / h[j] - (h[j] * (c[j + 1] + 2 * c[j])) / 3;
                    d[j] = (c[j + 1] - c[j]) / (3 * h[j]);
                }
                //endcommon
                for (int i = 0; i < n; ++i)
                {
                    xSpline.Add(new CubicSpline(a[i], b[i], c[i], d[i], s[i]));
                }

                //------------------------------------------------
                //Y dimension
                for (int i = 0; i <= n; ++i)
                {
                    a[i] = controlPoints[i].y;
                }
                //common
                for (int i = 0; i < n; ++i)
                {
                    h[i] = s[i + 1] - s[i];
                }
                for (int i = 1; i < n; ++i)
                {
                    alpha[i] = 3 / h[i] * (a[i + 1] - a[i]) - 3 / h[i - 1] * (a[i] - a[i - 1]);
                }
                l[0] = 1;
                u[0] = z[0] = 0;
                for (int i = 1; i < n; ++i)
                {
                    l[i] = 2 * (s[i + 1] - s[i - 1]) - h[i - 1] * u[i - 1];
                    u[i] = h[i] / l[i];
                    z[i] = (alpha[i] - h[i - 1] * z[i - 1]) / l[i];
                }
                l[n] = 1;
                z[n] = c[n] = 0;
                for (int j = n - 1; j >= 0; --j)
                {
                    c[j] = z[j] - u[j] * c[j + 1];
                    b[j] = (a[j + 1] - a[j]) / h[j] - (h[j] * (c[j + 1] + 2 * c[j])) / 3;
                    d[j] = (c[j + 1] - c[j]) / (3 * h[j]);
                }
                //endcommon
                for (int i = 0; i < n; ++i)
                {
                    ySpline.Add(new CubicSpline(a[i], b[i], c[i], d[i], s[i]));
                }

                //------------------------------------------------
                //Z dimension
                for (int i = 0; i <= n; ++i)
                {
                    a[i] = controlPoints[i].z;
                }
                //common
                for (int i = 0; i < n; ++i)
                {
                    h[i] = s[i + 1] - s[i];
                }
                for (int i = 1; i < n; ++i)
                {
                    alpha[i] = 3 / h[i] * (a[i + 1] - a[i]) - 3 / h[i - 1] * (a[i] - a[i - 1]);
                }
                l[0] = 1;
                u[0] = z[0] = 0;
                for (int i = 1; i < n; ++i)
                {
                    l[i] = 2 * (s[i + 1] - s[i - 1]) - h[i - 1] * u[i - 1];
                    u[i] = h[i] / l[i];
                    z[i] = (alpha[i] - h[i - 1] * z[i - 1]) / l[i];
                }
                l[n] = 1;
                z[n] = c[n] = 0;
                for (int j = n - 1; j >= 0; --j)
                {
                    c[j] = z[j] - u[j] * c[j + 1];
                    b[j] = (a[j + 1] - a[j]) / h[j] - (h[j] * (c[j + 1] + 2 * c[j])) / 3;
                    d[j] = (c[j + 1] - c[j]) / (3 * h[j]);
                }
                //endcommon
                for (int i = 0; i < n; ++i)
                {
                    zSpline.Add(new CubicSpline(a[i], b[i], c[i], d[i], s[i]));
                }
            }
        }
    }
}

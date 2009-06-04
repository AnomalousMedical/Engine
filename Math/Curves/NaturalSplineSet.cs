using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class NaturalSplineSet
    {
        class Spline
        {
            public float a, b, c, d, xt;

            public Spline(float a, float b, float c, float d, float xt)
            {
                this.a = a;
                this.b = b;
                this.c = c;
                this.d = d;
                this.xt = xt;
            }

            public float interpolate(float value)
            {
                float xminusxt = value - xt;
                return a + b * xminusxt + c * xminusxt * xminusxt + d * xminusxt * xminusxt * xminusxt;
            }
        }

        List<Vector3> controlPoints = new List<Vector3>();
        List<Spline> xSpline = new List<Spline>();
        List<Spline> ySpline = new List<Spline>();
        List<Spline> zSpline = new List<Spline>();
        float[] s;

        public void addControlPoint(Vector3 controlPoint)
        {
            controlPoints.Add(controlPoint);
        }

        public Vector3 interpolate(float percent)
        {
            int i;
            if (percent < 1.0f)
            {
                for (i = 0; percent >= s[i]; ++i)
                {

                }
                i = i - 1;
            }
            i = xSpline.Count - 1;
            return new Vector3(xSpline[i].interpolate(percent), ySpline[i].interpolate(percent), zSpline[i].interpolate(percent));
        }

        public void computeSplines()
        {
            int n = controlPoints.Count - 1; //n is the number of splines.
            //Compute s values between 0 and 1 for the parametric splines
            float totalLength = 0;
            s = new float[n + 1];
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
                xSpline.Add(new Spline(a[i], b[i], c[i], d[i], s[i]));
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
                ySpline.Add(new Spline(a[i], b[i], c[i], d[i], s[i]));
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
                zSpline.Add(new Spline(a[i], b[i], c[i], d[i], s[i]));
            }
        }
    }
}

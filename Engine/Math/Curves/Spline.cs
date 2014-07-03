using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// This is a spline representation.
    /// </summary>
    class Spline
    {
        public float a, b, c, d, xt;

        public Spline()
        {

        }

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
}

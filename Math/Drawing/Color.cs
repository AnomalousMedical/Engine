using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineMath
{
    /// <summary>
    /// This is a color struct. Color values are between 0 and 1.
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// Red value.
        /// </summary>
        public float r;

        /// <summary>
        /// Green value.
        /// </summary>
        public float g;

        /// <summary>
        /// Blue value.
        /// </summary>
        public float b;

        /// <summary>
        /// Alpha value.
        /// </summary>
        public float a;

        /// <summary>
        /// Constructor. Will have alpha of 1.0f.
        /// </summary>
        /// <param name="r">Red.</param>
        /// <param name="g">Green.</param>
        /// <param name="b">Blue.</param>
        public Color(float r, float g, float b)
            :this(r, g, b, 1.0f)
        {
            
        }

        /// <summary>
        /// Constructor takes explicit alpha.
        /// </summary>
        /// <param name="r">Red.</param>
        /// <param name="g">Green.</param>
        /// <param name="b">Blue.</param>
        /// <param name="a">Alpha.</param>
        public Color(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This is a color struct. Color values are between 0 and 1.
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// String format in rgba
        /// </summary>
        private const String FORMAT = "{0}, {1}, {2}, {3}";

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

        /// <summary>
        /// Constructor, gets value from a string.
        /// </summary>
        /// <param name="value">A string in RGBA format R, G, B, A.</param>
        public Color(String value)
        {
            setValue(value, out r, out g, out b, out a);
        }

        /// <summary>
        /// Output as a string in RGBA format R, G, B, A.
        /// </summary>
        /// <returns>A string</returns>
        public override string ToString()
        {
            return String.Format(FORMAT, r, g, b, a);
        }

        public bool FromString(String value)
        {
            return setValue(value, out r, out g, out b, out a);
        }

        #region Static Helpers

        private static char[] SEPS = { ',' };

        /// <summary>
        /// Parse a string and set the value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private static bool setValue(String value, out float r, out float g, out float b, out float a)
        {
            String[] nums = value.Split(SEPS);
            bool success = false;
            if (nums.Length == 4)
            {
                success = float.TryParse(nums[0], out r);
                success &= float.TryParse(nums[1], out g);
                success &= float.TryParse(nums[2], out b);
                success &= float.TryParse(nums[3], out a);
            }
            else
            {
                r = 0f;
                g = 0f;
                b = 0f;
                a = 0f;
            }
            return success;
        }

        #endregion
    }
}

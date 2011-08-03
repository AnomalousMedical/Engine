using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

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

        public static readonly Color White = new Color(1.0f, 1.0f, 1.0f);
        public static readonly Color Black = new Color(0.0f, 0.0f, 0.0f);
        public static readonly Color Red = new Color(1.0f, 0.0f, 0.0f);
        public static readonly Color Green = new Color(0.0f, 1.0f, 0.0f);
        public static readonly Color Blue = new Color(0.0f, 0.0f, 1.0f);

        public static Color FromARGB(byte a, byte r, byte g, byte b)
        {
            return new Color((float)r / 255.0f,
                            (float)g / 255.0f,
                            (float)b / 255.0f,
                            (float)a / 255.0f);
        }

        public static Color FromARGB(uint color)
        {
            return FromARGB((byte)((color >> 24) & 0xFF),
                            (byte)((color >> 16) & 0xFF),
                            (byte)((color >> 8) & 0xFF),
                            (byte)(color & 0xFF));
        }

        public static Color FromARGB(int color)
        {
            return FromARGB((byte)((color >> 24) & 0xFF),
                            (byte)((color >> 16) & 0xFF),
                            (byte)((color >> 8) & 0xFF),
                            (byte)(color & 0xFF));
        }

        public static Color FromRGB(uint color)
        {
            return FromARGB((byte)(0xFF),
                            (byte)((color >> 16) & 0xFF),
                            (byte)((color >> 8) & 0xFF),
                            (byte)(color & 0xFF));
        }

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
            return String.Format(CultureInfo.InvariantCulture, FORMAT, r, g, b, a);
        }

        public bool FromString(String value)
        {
            return setValue(value, out r, out g, out b, out a);
        }

        public String ToHexString()
        {
            return String.Format("{0:X2}{1:X2}{2:X2}", (int)(r * 0xff), (int)(g * 0xff), (int)(b * 0xff));
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
                success = NumberParser.TryParse(nums[0], out r);
                success &= NumberParser.TryParse(nums[1], out g);
                success &= NumberParser.TryParse(nums[2], out b);
                success &= NumberParser.TryParse(nums[3], out a);
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

        public int toARGB()
        {
            int argb = 0;
            byte comp;

            comp = (byte)(a * 255.0f);
            argb = comp << 24;

            comp = (byte)(r * 255.0f);
            argb += comp << 16;

            comp = (byte)(g * 255.0f);
            argb += comp << 8;

            comp = (byte)(b * 255.0f);
            argb += comp;

            return argb;
        }

        #endregion
    }
}

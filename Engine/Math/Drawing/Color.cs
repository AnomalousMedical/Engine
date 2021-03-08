using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Engine
{
    /// <summary>
    /// This is a color struct. Color values are between 0 and 1.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 16)]
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
        public static readonly Color LightBlue = new Color(0.188f, 0.894f, 1.0f);
        public static readonly Color Purple = new Color(0.8f, 0.0f, 1.0f);
        public static readonly Color HotPink = new Color(1.0f, 0.0f, 1.0f);
        public static readonly Color Yellow = new Color(1.0f, 0.98f, 0.0f);
        public static readonly Color Orange = new Color(1.0f, 0.4f, 0.0f);

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

        public static Color FromRGBA(int color)
        {
            return FromARGB((byte)(color & 0xFF),
                            (byte)((color >> 24) & 0xFF),
                            (byte)((color >> 16) & 0xFF),
                            (byte)((color >> 8) & 0xFF));
        }

        public static Color FromRGBA(uint color)
        {
            return FromARGB((byte)(color & 0xFF),
                            (byte)((color >> 24) & 0xFF),
                            (byte)((color >> 16) & 0xFF),
                            (byte)((color >> 8) & 0xFF));
        }

        public static Color FromRGB(uint color)
        {
            return FromARGB((byte)(0xFF),
                            (byte)((color >> 16) & 0xFF),
                            (byte)((color >> 8) & 0xFF),
                            (byte)(color & 0xFF));
        }

        public static Color FromRGB(int color)
        {
            return FromARGB((byte)(0xFF),
                            (byte)((color >> 16) & 0xFF),
                            (byte)((color >> 8) & 0xFF),
                            (byte)(color & 0xFF));
        }

        /// <summary>
        /// Red value.
        /// </summary>
        [FieldOffset(0)]
        public float r;

        /// <summary>
        /// Green value.
        /// </summary>
        [FieldOffset(4)]
        public float g;

        /// <summary>
        /// Blue value.
        /// </summary>
        [FieldOffset(8)]
        public float b;

        /// <summary>
        /// Alpha value.
        /// </summary>
        [FieldOffset(12)]
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

        public Color ToSrgb()
        {
            //This is the lame conversion
            return new Color
            (
                (float)Math.Pow(r, 2.2),
                (float)Math.Pow(g, 2.2),
                (float)Math.Pow(b, 2.2),
                a
            );
        }

        /// <summary>
        /// Output as a string in RGBA format R, G, B, A.
        /// </summary>
        /// <returns>A string</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, FORMAT, r, g, b, a);
        }

        /// <summary>
        /// Scale rgb components into new color.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Color ScaleRgb(float s)
        {
            return new Color(r * s, g * s, b * s, a);
        }

        /// <summary>
        /// Add rgb components together into new color
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public Color AddRgb(in Color c2)
        {
            return new Color(
                r + c2.r,
                g + c2.g,
                b + c2.b,
                a);
        }

        public bool FromString(String value)
        {
            return setValue(value, out r, out g, out b, out a);
        }

        public String ToHexString()
        {
            return String.Format("#{0:X2}{1:X2}{2:X2}", (int)(r * 0xff), (int)(g * 0xff), (int)(b * 0xff));
        }

        public String ToHexRGBAString()
        {
            return String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", (int)(r * 0xff), (int)(g * 0xff), (int)(b * 0xff), (int)(a * 0xff));
        }

        #region Static Helpers

        const float maxValue = (float)0xff;

        /// <summary>
        /// Blend 2 colors. The amount to blend is between 0.0 and 1.0
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        /// <returns></returns>
        public static Color FadeColors(float amount, in Color startColor, in Color endColor)
        {
            //Nicola Pezzotti
            //https://stackoverflow.com/questions/20461691/c-fade-between-colors-arduino
            //Modified to blend between 0 and 1
            float cosArg = amount * MathF.PI / 2;
            float fade = MathF.Abs(MathF.Cos(cosArg));

            var finalColor = startColor.ScaleRgb(fade).AddRgb(endColor.ScaleRgb(1 - fade));

            return finalColor;
        }

        /// <summary>
        /// Compute the color value from a #RGBA string, the A part can be ommitted.
        /// So valid values would be:
        /// #FF0000 - Red, opaque
        /// #FF0000FF - Red, opaque specified by alpha
        /// #FF000066 - Red, transparent specified by alpha
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Color FromRGBAString(String hex)
        {
            if (hex.Length == 7)
            {
                return new Color(int.Parse(hex.Substring(1, 2), NumberStyles.HexNumber) / maxValue,
                                 int.Parse(hex.Substring(3, 2), NumberStyles.HexNumber) / maxValue,
                                 int.Parse(hex.Substring(5, 2), NumberStyles.HexNumber) / maxValue);
            }
            else if (hex.Length == 9)
            {
                return new Color(int.Parse(hex.Substring(1, 2), NumberStyles.HexNumber) / maxValue,
                                 int.Parse(hex.Substring(3, 2), NumberStyles.HexNumber) / maxValue,
                                 int.Parse(hex.Substring(5, 2), NumberStyles.HexNumber) / maxValue,
                                 int.Parse(hex.Substring(7, 2), NumberStyles.HexNumber) / maxValue);
            }
            else
            {
                throw new FormatException(String.Format("String {0} is not valid. Input must be #RGB or #RGBA specified as hex values, for example #00FF00 for Green"));
            }
        }

        public static bool TryFromRGBAString(String hex, out Color color)
        {
            return TryFromRGBAString(hex, out color, Color.White);
        }

        public static bool TryFromRGBAString(String hex, out Color color, Color errorColor)
        {
            if (hex.Length == 7)
            {
                int r, g, b;
                if (int.TryParse(hex.Substring(1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out r) &&
                    int.TryParse(hex.Substring(3, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out g) &&
                    int.TryParse(hex.Substring(5, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out b))
                {
                    color = new Color(r / maxValue, g / maxValue, b / maxValue);
                    return true;
                }
            }
            else if (hex.Length == 9)
            {
                int r, g, b, a;
                if (int.TryParse(hex.Substring(1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out r) &&
                    int.TryParse(hex.Substring(3, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out g) &&
                    int.TryParse(hex.Substring(5, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out b) &&
                    int.TryParse(hex.Substring(7, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out a))
                {
                    color = new Color(r / maxValue, g / maxValue, b / maxValue, a / maxValue);
                    return true;
                }
            }
            color = errorColor;
            return false;
        }

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

        public int toRGBA()
        {
            int rgba = 0;
            byte comp;

            comp = (byte)(r * 255.0f);
            rgba = comp << 24;

            comp = (byte)(g * 255.0f);
            rgba += comp << 16;

            comp = (byte)(b * 255.0f);
            rgba += comp << 8;

            comp = (byte)(a * 255.0f);
            rgba += comp;

            return rgba;
        }

        public int toRGB()
        {
            int rgb = 0;
            byte comp;

            comp = (byte)(r * 255.0f);
            rgb += comp << 16;

            comp = (byte)(g * 255.0f);
            rgb += comp << 8;

            comp = (byte)(b * 255.0f);
            rgb += comp;

            return rgb;
        }

        /// <summary>
        /// Equals function.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the objects are equal.</returns>
        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(Color) && this == (Color)obj;
        }

        /// <summary>
        /// Hash code function.
        /// </summary>
        /// <returns>A hash code for this Vector3.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Color p1, Color p2)
        {
            return p1.r == p2.r && p1.g == p2.g && p1.b == p2.b && p1.a == p2.a;
        }

        public static bool operator !=(Color p1, Color p2)
        {
            return !(p1.r == p2.r && p1.g == p2.g && p1.b == p2.b && p1.a == p2.a);
        }

        #endregion
    }
}

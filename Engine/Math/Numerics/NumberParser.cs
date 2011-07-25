using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Engine
{
    public sealed class NumberParser
    {
        private NumberParser()
        {

        }

        //sbyte
        public static sbyte ParseSbyte(String s)
        {
            return sbyte.Parse(s, CultureInfo.InvariantCulture);
        }

        public static bool TryParse(String s, out sbyte value)
        {
            return sbyte.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        public static String ToString(sbyte value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        //byte
        public static byte ParseByte(String s)
        {
            return byte.Parse(s, CultureInfo.InvariantCulture);
        }

        public static bool TryParse(String s, out byte value)
        {
            return byte.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        public static String ToString(byte value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        //short
        public static short ParseShort(String s)
        {
            return short.Parse(s, CultureInfo.InvariantCulture);
        }

        public static bool TryParse(String s, out short value)
        {
            return short.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        public static String ToString(short value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        //ushort
        public static ushort ParseUshort(String s)
        {
            return ushort.Parse(s, CultureInfo.InvariantCulture);
        }

        public static bool TryParse(String s, out ushort value)
        {
            return ushort.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        public static String ToString(ushort value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        //int
        public static int ParseInt(String s)
        {
            return int.Parse(s, CultureInfo.InvariantCulture);
        }

        public static bool TryParse(String s, out int value)
        {
            return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        public static String ToString(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        //uint
        public static uint ParseUint(String s)
        {
            return uint.Parse(s, CultureInfo.InvariantCulture);
        }

        public static bool TryParse(String s, out uint value)
        {
            return uint.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        public static String ToString(uint value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        //long
        public static long ParseLong(String s)
        {
            return long.Parse(s, CultureInfo.InvariantCulture);
        }

        public static bool TryParse(String s, out long value)
        {
            return long.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        public static String ToString(long value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        //ulong
        public static ulong ParseUlong(String s)
        {
            return ulong.Parse(s, CultureInfo.InvariantCulture);
        }

        public static bool TryParse(String s, out ulong value)
        {
            return ulong.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        public static String ToString(ulong value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        //char
        public static char ParseChar(String s)
        {
            return char.Parse(s);
        }

        public static bool TryParse(String s, out char value)
        {
            return char.TryParse(s, out value);
        }

        public static String ToString(char value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        //Float
        public static float ParseFloat(String s)
        {
            return float.Parse(s, CultureInfo.InvariantCulture);
        }

        public static bool TryParse(String s, out float value)
        {
            return float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value);
        }

        public static String ToString(float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        //Double
        public static double ParseDouble(String s)
        {
            return double.Parse(s, CultureInfo.InvariantCulture);
        }

        public static bool TryParse(String s, out double value)
        {
            return double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value);
        }

        public static String ToString(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        //Decimal
        public static decimal ParseDecimal(String s)
        {
            return decimal.Parse(s, CultureInfo.InvariantCulture);
        }

        public static bool TryParse(String s, out decimal value)
        {
            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }

        public static String ToString(decimal value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}

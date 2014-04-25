using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// Store a floating point value as a radian. Allows conversion to degrees.
    /// </summary>
    public struct Radian
    {
        private const float CONVERSION = (float)Math.PI / 180.0f;

        public float Value;

        public Radian(float value)
        {
            this.Value = value;
        }

        public static implicit operator Radian(float value)
        {
            return new Radian(value);
        }

        public static implicit operator float(Radian value)
        {
            return value.Value;
        }

        public static implicit operator Radian(Degree value)
        {
            return new Radian(value * CONVERSION);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

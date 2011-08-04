using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// Represents a floating point value as a degree. Allows conversion to radians.
    /// </summary>
    public struct Degree
    {
        private const float CONVERSION = 180.0f / (float)Math.PI;

        public float Value;

        public Degree(float value)
        {
            this.Value = value;
        }

        public static implicit operator Degree(float value)
        {
            return new Degree(value);
        }

        public static implicit operator float(Degree value)
        {
            return value.Value;
        }

        public static implicit operator Degree(Radian value)
        {
            return new Degree(value * CONVERSION);
        }
    }
}

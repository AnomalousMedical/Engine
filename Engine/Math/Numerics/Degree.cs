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
        /// <summary>
        /// Conversion factor from radians to degrees.
        /// </summary>
        public const float FromRadian = 180.0f / MathFloat.PI;

        public float Value;

        public Degree(float value)
        {
            this.Value = value;
        }

        public float Radians
        {
            get
            {
                return Value * Radian.FromDegrees;
            }
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
            return new Degree(value * FromRadian);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

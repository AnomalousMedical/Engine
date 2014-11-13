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
        /// <summary>
        /// Conversion factor from degrees to radians.
        /// </summary>
        public const float FromDegrees = (float)Math.PI / 180.0f;

        public float Value;

        public Radian(float value)
        {
            this.Value = value;
        }

        public float Degrees
        {
            get
            {
                return Value * Degree.FromRadian;
            }
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
            return new Radian(value * FromDegrees);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

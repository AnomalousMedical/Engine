using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
    /// <summary>
    /// A class for a box aligned on the x, y and z axis.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct AxisAlignedBox
    {
        [FieldOffset(0)]
        private Vector3 minimum;

        [FieldOffset(12)]
        private Vector3 maximum;

        /// <summary>
        /// The minimum point.
        /// </summary>
        public Vector3 Minimum
        {
            get
            {
                return minimum;
            }
            set
            {
                minimum = value;
            }
        }

        /// <summary>
        /// The maximum point.
        /// </summary>
        public Vector3 Maximum
        {
            get
            {
                return maximum;
            }
            set
            {
                maximum = value;
            }
        }

        /// <summary>
        /// The distance between maximum and minimum.
        /// </summary>
        public float DiagonalDistance
        {
            get
            {
                return (maximum - minimum).length();
            }
        }

        /// <summary>
        /// The distance squared between maximum and minimum.
        /// </summary>
        public float DiagonalDistance2
        {
            get
            {
                return (maximum - minimum).length2();
            }
        }

        /// <summary>
        /// The center point of the box.
        /// </summary>
        public Vector3 Center
        {
            get
            {
                return (minimum + maximum) * 0.5f;
            }
        }

        /// <summary>
        /// Merge this box with another.
        /// </summary>
        /// <param name="box">The box to merge.</param>
        public void merge(AxisAlignedBox box)
        {
            maximum.makeCeiling(box.maximum);
            minimum.makeFloor(box.minimum);
        }

        /// <summary>
        /// Add the point to the box. This will update the bounds if point exceeds any of them.
        /// </summary>
        /// <param name="point">The point to add.</param>
        public void merge(Vector3 point)
        {
            maximum.makeCeiling(point);
            minimum.makeFloor(point);
        }

        public override string ToString()
        {
            return String.Format("Min {0} : Max {1}", minimum, maximum);
        }
    }
}

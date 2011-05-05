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

        public float DiagonalDistance
        {
            get
            {
                return (maximum - minimum).length();
            }
        }

        public override string ToString()
        {
            return String.Format("Min {0} : Max {1}", minimum, maximum);
        }
    }
}

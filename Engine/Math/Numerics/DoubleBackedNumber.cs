using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This struct provides a number that is backed by a double that can be easily
    /// and safely converted to several types. If the number is too large or small
    /// to be represented it will be set to the min and max values accordingly.
    /// </summary>
    public struct DoubleBackedNumber
    {
        private double value;

        public DoubleBackedNumber(double value)
        {
            this.value = value;
        }

        public Int16 AsInt16
        {
            get
            {
                try
                {
                    return Convert.ToInt16(value);
                }
                catch (OverflowException)
                {
                    if (value < 0)
                    {
                        return Int16.MinValue;
                    }
                    return Int16.MaxValue;
                }
            }
            set
            {
                this.value = value;
            }
        }

        public Int32 AsInt32
        {
            get
            {
                try
                {
                    return Convert.ToInt32(value);
                }
                catch (OverflowException)
                {
                    if (value < 0)
                    {
                        return Int32.MinValue;
                    }
                    return Int32.MaxValue;
                }
            }
            set
            {
                this.value = value;
            }
        }

        public Int64 AsInt64
        {
            get
            {
                try
                {
                    return Convert.ToInt64(value);
                }
                catch (OverflowException)
                {
                    if (value < 0)
                    {
                        return Int64.MinValue;
                    }
                    return Int64.MaxValue;
                }
            }
            set
            {
                this.value = value;
            }
        }

        public UInt16 AsUInt16
        {
            get
            {
                try
                {
                    return Convert.ToUInt16(value);
                }
                catch (OverflowException)
                {
                    if (value < 0)
                    {
                        return UInt16.MinValue;
                    }
                    return UInt16.MaxValue;
                }
            }
            set
            {
                this.value = value;
            }
        }

        public UInt32 AsUInt32
        {
            get
            {
                try
                {
                    return Convert.ToUInt32(value);
                }
                catch (OverflowException)
                {
                    if (value < 0)
                    {
                        return UInt32.MinValue;
                    }
                    return UInt32.MaxValue;
                }
            }
            set
            {
                this.value = value;
            }
        }

        public UInt64 AsUInt64
        {
            get
            {
                try
                {
                    return Convert.ToUInt64(value);
                }
                catch (OverflowException)
                {
                    if (value < 0)
                    {
                        return UInt64.MinValue;
                    }
                    return UInt64.MaxValue;
                }
            }
            set
            {
                this.value = value;
            }
        }

        public byte AsByte
        {
            get
            {
                try
                {
                    return Convert.ToByte(value);
                }
                catch (OverflowException)
                {
                    if (value < 0)
                    {
                        return byte.MinValue;
                    }
                    return byte.MaxValue;
                }
            }
            set
            {
                this.value = value;
            }
        }

        public SByte AsSByte
        {
            get
            {
                try
                {
                    return Convert.ToSByte(value);
                }
                catch (OverflowException)
                {
                    if (value < 0)
                    {
                        return SByte.MinValue;
                    }
                    return SByte.MaxValue;
                }
            }
            set
            {
                this.value = value;
            }
        }

        public float AsSingle
        {
            get
            {
                return Convert.ToSingle(value);
            }
            set
            {
                this.value = value;
            }
        }

        public double AsDouble
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public decimal AsDecimal
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(value);
                }
                catch (OverflowException)
                {
                    if (value < 0)
                    {
                        return decimal.MinValue;
                    }
                    return decimal.MaxValue;
                }
            }
        }

        public static implicit operator DoubleBackedNumber(double num)
        {
            return new DoubleBackedNumber(num);
        }
    }
}

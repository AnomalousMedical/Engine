using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class IntColor
    {
        private const int AShift = 24;
        private const int RShift = 16;
        private const int GShift = 8;
        private const int BShift = 0;

        private const uint AMask = 0xff000000;
        private const uint RMask = 0x00ff0000;
        private const uint GMask = 0x0000ff00;
        private const uint BMask = 0x000000ff;

        private const uint ARemove = ~AMask;
        private const uint RRemove = ~RMask;
        private const uint GRemove = ~GMask;
        private const uint BRemove = ~BMask;

        private UInt32 argb;

        public IntColor()
        {

        }

        public IntColor(UInt32 argb)
        {
            this.argb = argb;
        }

        public IntColor(byte a, byte r, byte g, byte b)
        {
            setValues(a, r, g, b);
        }

        public byte A
        {
            get
            {
                return (byte)((argb & AMask) >> AShift);
            }
            set
            {
                argb = (argb & ARemove) + ((uint)value << AShift);
            }
        }

        public byte R
        {
            get
            {
                return (byte)((argb & RMask) >> RShift);
            }
            set
            {
                argb = (argb & RRemove) + ((uint)value << RShift);
            }
        }

        public byte G
        {
            get
            {
                return (byte)((argb & GMask) >> GShift);
            }
            set
            {
                argb = (argb & GRemove) + ((uint)value << GShift);
            }
        }

        public byte B
        {
            get
            {
                return (byte)((argb & BMask) >> BShift);
            }
            set
            {
                argb = (argb & BRemove) + ((uint)value << BShift);
            }
        }

        public UInt32 ARGB
        {
            get
            {
                return argb;
            }
            set
            {
                argb = value;
            }
        }

        public void setValues(byte a, byte r, byte g, byte b)
        {
            argb = (uint)(a << AShift) + (uint)(r << RShift) + (uint)(g << GShift) + (uint)(b << BShift);
        }
    }
}

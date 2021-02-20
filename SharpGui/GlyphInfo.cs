using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpGui
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct GlyphRect
    {
        [FieldOffset(0)]
        public float Left;
        [FieldOffset(4)]
        public float Top;
        [FieldOffset(8)]
        public float Right;
        [FieldOffset(12)]
        public float Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct GlyphInfoEntryPassStruct
    {
        public uint codePoint;
        public float width;
        public float height;
        public float advance;
        public float bearingX;
        public float bearingY;
        public GlyphRect uvRect;

        public GlyphInfo ToGlyphInfo()
        {
            return new GlyphInfo()
            {
                //Handle all typecasting here instead of per frame
                codePoint = this.codePoint,
                width = (int)this.width,
                height = (int)this.height,
                advance = (int)this.advance,
                bearingX = (int)this.bearingX,
                bearingY = (int)this.bearingY,
                uvRect = this.uvRect,
            };
        }
    }

    public class GlyphInfo
    {
        public uint codePoint;
        public int width;
        public int height;
        public int advance;
        public int bearingX;
        public int bearingY;
        public GlyphRect uvRect;
    }
}

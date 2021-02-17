using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
	[StructLayout(LayoutKind.Sequential)]
    struct GlyphInfo
    {
		public uint codePoint;
		public float width;
		public float height;
		public float advance;
		public float bearingX;
		public float bearingY;
		public Rect uvRect;
	}
}

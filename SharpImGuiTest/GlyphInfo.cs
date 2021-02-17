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
		uint codePoint;
		float width;
		float height;
		float advance;
		float bearingX;
		float bearingY;
		Rect uvRect;
	}
}

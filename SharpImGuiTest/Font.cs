using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    public class Font
    {
        public Dictionary<char, uint> CharMap { get; internal set; }
        public Dictionary<uint, GlyphInfo> GlyphInfo { get; internal set; }
    }
}

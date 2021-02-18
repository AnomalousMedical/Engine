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

        public bool TryGetGlyphInfo(char c, out GlyphInfo glyphInfo)
        {
            glyphInfo = null;
            return CharMap.TryGetValue(c, out var cMap) && GlyphInfo.TryGetValue(cMap, out glyphInfo);
        }

        public int MeasureText(String text)
        {
            int width = 0;
            foreach (var c in text)
            {
                uint charCode = c;
                if (TryGetGlyphInfo(c, out var glyphInfo))
                {
                    int fullAdvance = (int)glyphInfo.advance + (int)glyphInfo.bearingX;
                    width += fullAdvance;
                }
            }
            return width;
        }
    }
}

using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGui
{
    public class Font
    {
        public Dictionary<char, uint> CharMap { get; internal set; }
        public Dictionary<uint, GlyphInfo> GlyphInfo { get; internal set; }

        const String TallEnglishLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        float? smallestBearingY;
        public float SmallestBearingY
        {
            get
            {
                if(smallestBearingY == null)
                {
                    smallestBearingY = float.MaxValue;
                    foreach(var c in TallEnglishLetters)
                    {
                        if (TryGetGlyphInfo(c, out var g))
                        {
                            smallestBearingY = Math.Min(SmallestBearingY, g.bearingY);
                        }
                    }
                }
                return smallestBearingY.Value;
            }
        }

        public bool TryGetGlyphInfo(char c, out GlyphInfo glyphInfo)
        {
            glyphInfo = null;
            return CharMap.TryGetValue(c, out var cMap) && GlyphInfo.TryGetValue(cMap, out glyphInfo);
        }

        public IntSize2 MeasureText(String text)
        {
            int xOffset = 0;
            int yOffset = 0;
            int widest = 0;
            int tallestLineChar = 0;
            foreach (var c in text)
            {
                uint charCode = c;
                if (TryGetGlyphInfo(c, out var glyphInfo))
                {
                    int fullAdvance = (int)glyphInfo.advance + (int)glyphInfo.bearingX;
                    xOffset += fullAdvance;
                    tallestLineChar = Math.Max((int)glyphInfo.height + (int)glyphInfo.bearingY, tallestLineChar);
                }
                if (c == '\n')
                {
                    widest = Math.Max(widest, xOffset);
                    yOffset += tallestLineChar;
                    tallestLineChar = 0;
                    xOffset = 0;
                }
            }

            widest = Math.Max(widest, xOffset);

            return new IntSize2(widest, yOffset + tallestLineChar - (int)SmallestBearingY);
        }
    }
}

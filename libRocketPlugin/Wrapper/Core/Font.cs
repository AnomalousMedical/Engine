using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    public class Font
    {
        public enum Style
        {
            STYLE_NORMAL = 0,
            STYLE_ITALIC = 1,
            NUM_STYLES = 2
        };

        public enum Weight
        {
            WEIGHT_NORMAL = 0,
            WEIGHT_BOLD = 1,
            NUM_WEIGHTS = 2
        };

        public enum Line
        {
            UNDERLINE = 0,
            OVERLINE = 1,
            STRIKE_THROUGH = 2
        };

        private Font()
        {

        }
    }
}

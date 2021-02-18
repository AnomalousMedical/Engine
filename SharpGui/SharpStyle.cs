using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpGui
{
    public class SharpStyle
    {
        public SharpLook Normal { get; set; }

        public SharpLook Hover { get; set; }

        public SharpLook Active { get; set; }

        public SharpLook HoverAndActive { get; set; }

        public SharpLook Focus { get; set; }

        public SharpLook HoverAndFocus { get; set; }

        public SharpLook HoverAndActiveAndFocus { get; set; }

        public static SharpStyle CreateComplete()
        {
            return new SharpStyle()
            {
                Normal = new SharpLook()
                {
                    Background = Color.FromARGB(0xffaaaaaa),
                    Color = Color.FromARGB(0xff000000),
                    ShadowColor = Color.FromARGB(0xff000000),
                    ShadowOffset = new IntVector2(8, 8),
                    FocusHighlightColor = Color.FromARGB(0xff0000ff),
                    FocusSize = new IntSize2(12, 12)
                },
                Hover = new SharpLook()
                {
                    Background = Color.FromARGB(0xffffffff),
                    Color = Color.FromARGB(0xff000000),
                    ShadowColor = Color.FromARGB(0xff000000),
                    ShadowOffset = new IntVector2(8, 8),
                    FocusHighlightColor = Color.FromARGB(0xff0000ff),
                    FocusSize = new IntSize2(12, 12)
                },
                Active = new SharpLook()
                {
                    Background = Color.FromARGB(0xffaaaaaa),
                    Color = Color.FromARGB(0xff000000),
                    ShadowColor = Color.FromARGB(0xff000000),
                    ShadowOffset = new IntVector2(8, 8),
                    FocusHighlightColor = Color.FromARGB(0xff0000ff),
                    FocusSize = new IntSize2(12, 12)
                },
                HoverAndActive = new SharpLook()
                {
                    Background = Color.FromARGB(0xffff0000),
                    Color = Color.FromARGB(0xff000000),
                    ShadowColor = Color.FromARGB(0xff000000),
                    ShadowOffset = new IntVector2(8, 8),
                    FocusHighlightColor = Color.FromARGB(0xff0000ff),
                    FocusSize = new IntSize2(12, 12)
                },
                Focus = new SharpLook()
                {
                    Background = Color.FromARGB(0xffaaaaaa),
                    Color = Color.FromARGB(0xff000000),
                    ShadowColor = Color.FromARGB(0xff000000),
                    ShadowOffset = new IntVector2(8, 8),
                    FocusHighlightColor = Color.FromARGB(0xff0000ff),
                    FocusSize = new IntSize2(12, 12)
                },
                HoverAndFocus = new SharpLook()
                {
                    Background = Color.FromARGB(0xffaaaaaa),
                    Color = Color.FromARGB(0xff000000),
                    ShadowColor = Color.FromARGB(0xff000000),
                    ShadowOffset = new IntVector2(8, 8),
                    FocusHighlightColor = Color.FromARGB(0xff0000ff),
                    FocusSize = new IntSize2(12, 12)
                },
                HoverAndActiveAndFocus = new SharpLook()
                {
                    Background = Color.FromARGB(0xffff0000),
                    Color = Color.FromARGB(0xff000000),
                    ShadowColor = Color.FromARGB(0xff000000),
                    ShadowOffset = new IntVector2(8, 8),
                    FocusHighlightColor = Color.FromARGB(0xff0000ff),
                    FocusSize = new IntSize2(12, 12)
                }
            };
        }
    }
}

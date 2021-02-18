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
                    Background = Color.FromARGB(0xffdedede).ToSrgb(),
                    Color = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowColor = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowOffset = new IntVector2(8, 8),
                    FocusHighlightColor = Color.FromARGB(0xff4376a9).ToSrgb(),
                    FocusSize = new IntSize2(12, 12)
                },
                Hover = new SharpLook()
                {
                    Background = Color.FromARGB(0xffd2d2d2).ToSrgb(),
                    Color = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowColor = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowOffset = new IntVector2(8, 8),
                    FocusHighlightColor = Color.FromARGB(0xff4376a9).ToSrgb(),
                    FocusSize = new IntSize2(12, 12)
                },
                Active = new SharpLook()
                {
                    Background = Color.FromARGB(0xffdedede).ToSrgb(),
                    Color = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowColor = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowOffset = new IntVector2(8, 8),
                    FocusHighlightColor = Color.FromARGB(0xff4376a9).ToSrgb(),
                    FocusSize = new IntSize2(12, 12)
                },
                HoverAndActive = new SharpLook()
                {
                    Background = Color.FromARGB(0xffdadada).ToSrgb(),
                    Color = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowColor = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowOffset = new IntVector2(8, 8),
                    FocusHighlightColor = Color.FromARGB(0xff4376a9).ToSrgb(),
                    FocusSize = new IntSize2(12, 12)
                },
                Focus = new SharpLook()
                {
                    Background = Color.FromARGB(0xffdedede).ToSrgb(),
                    Color = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowColor = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowOffset = new IntVector2(8, 8),
                    FocusHighlightColor = Color.FromARGB(0xff4376a9).ToSrgb(),
                    FocusSize = new IntSize2(12, 12)
                },
                HoverAndFocus = new SharpLook()
                {
                    Background = Color.FromARGB(0xffd2d2d2).ToSrgb(),
                    Color = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowColor = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowOffset = new IntVector2(8, 8),
                    FocusHighlightColor = Color.FromARGB(0xff4376a9).ToSrgb(),
                    FocusSize = new IntSize2(12, 12)
                },
                HoverAndActiveAndFocus = new SharpLook()
                {
                    Background = Color.FromARGB(0xffdadada).ToSrgb(),
                    Color = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowColor = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowOffset = new IntVector2(8, 8),
                    FocusHighlightColor = Color.FromARGB(0xff4376a9).ToSrgb(),
                    FocusSize = new IntSize2(12, 12)
                }
            };
        }
    }
}

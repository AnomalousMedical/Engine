using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpGui
{
    public class SharpStyle
    {
        public SharpLook Normal { get; set; } = new SharpLook();

        public SharpLook Hover { get; set; } = new SharpLook();

        public SharpLook Active { get; set; } = new SharpLook();

        public SharpLook HoverAndActive { get; set; } = new SharpLook();

        public SharpLook Focus { get; set; } = new SharpLook();

        public SharpLook HoverAndFocus { get; set; } = new SharpLook();

        public SharpLook HoverAndActiveAndFocus { get; set; } = new SharpLook();

        public Color Background
        {
            set
            {
                Normal.Background = value;
                Hover.Background = value;
                Active.Background = value;
                HoverAndActive.Background = value;
                Focus.Background = value;
                HoverAndFocus.Background = value;
                HoverAndActiveAndFocus.Background = value;
            }
        }
        public Color Color
        {
            set
            {
                Normal.Color = value;
                Hover.Color = value;
                Active.Color = value;
                HoverAndActive.Color = value;
                Focus.Color = value;
                HoverAndFocus.Color = value;
                HoverAndActiveAndFocus.Color = value;
            }
        }
        public Color ShadowColor
        {
            set
            {
                Normal.ShadowColor = value;
                Hover.ShadowColor = value;
                Active.ShadowColor = value;
                HoverAndActive.ShadowColor = value;
                Focus.ShadowColor = value;
                HoverAndFocus.ShadowColor = value;
                HoverAndActiveAndFocus.ShadowColor = value;
            }
        }
        public IntVector2 ShadowOffset
        {
            set
            {
                Normal.ShadowOffset = value;
                Hover.ShadowOffset = value;
                Active.ShadowOffset = value;
                HoverAndActive.ShadowOffset = value;
                Focus.ShadowOffset = value;
                HoverAndFocus.ShadowOffset = value;
                HoverAndActiveAndFocus.ShadowOffset = value;
            }
        }
        public Color BorderColor
        {
            set
            {
                Normal.BorderColor = value;
                Hover.BorderColor = value;
                Active.BorderColor = value;
                HoverAndActive.BorderColor = value;
                Focus.BorderColor = value;
                HoverAndFocus.BorderColor = value;
                HoverAndActiveAndFocus.BorderColor = value;
            }
        }
        public IntPad Margin
        {
            set
            {
                Normal.Margin = value;
                Hover.Margin = value;
                Active.Margin = value;
                HoverAndActive.Margin = value;
                Focus.Margin = value;
                HoverAndFocus.Margin = value;
                HoverAndActiveAndFocus.Margin = value;
            }
        }
        public IntPad Border
        {
            set
            {
                Normal.Border = value;
                Hover.Border = value;
                Active.Border = value;
                HoverAndActive.Border = value;
                Focus.Border = value;
                HoverAndFocus.Border = value;
                HoverAndActiveAndFocus.Border = value;
            }
        }
        public IntPad Padding
        {
            set
            {
                Normal.Padding = value;
                Hover.Padding = value;
                Active.Padding = value;
                HoverAndActive.Padding = value;
                Focus.Padding = value;
                HoverAndFocus.Padding = value;
                HoverAndActiveAndFocus.Padding = value;
            }
        }

        public static SharpStyle CreateComplete(IScaleHelper scaleHelper)
        {
            var style = new SharpStyle()
            {
                Background = Color.FromARGB(0xffdedede).ToSrgb(),
                Color = Color.FromARGB(0xff000000).ToSrgb(),
                BorderColor = Color.FromARGB(0xff000000).ToSrgb(),
                ShadowColor = Color.FromARGB(0x80000000).ToSrgb(),
                ShadowOffset = scaleHelper.Scaled(new IntVector2(8, 8)),
                Padding = scaleHelper.Scaled(new IntPad(40)),
                Border = scaleHelper.Scaled(new IntPad(5)),
                Margin = scaleHelper.Scaled(new IntPad(40)),
                Hover =
                {
                    Background = Color.FromARGB(0xffd2d2d2).ToSrgb(),
                },
                Active =
                {
                    Background = Color.FromARGB(0xffdedede).ToSrgb(),
                },
                HoverAndActive =
                {
                    Background = Color.FromARGB(0xffdadada).ToSrgb(),
                },
                Focus =
                {
                    Background = Color.FromARGB(0xffdedede).ToSrgb(),
                    BorderColor = Color.FromARGB(0xff4376a9).ToSrgb(),
                },
                HoverAndFocus =
                {
                    Background = Color.FromARGB(0xffd2d2d2).ToSrgb(),
                    BorderColor = Color.FromARGB(0xff4376a9).ToSrgb(),
                },
                HoverAndActiveAndFocus =
                {
                    Background = Color.FromARGB(0xffdadada).ToSrgb(),
                    BorderColor = Color.FromARGB(0xff4376a9).ToSrgb(),
                }
            };
            return style;
        }
    }
}

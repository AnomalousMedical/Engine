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

        public static SharpStyle CreateComplete(IScaleHelper scaleHelper)
        {
            return new SharpStyle()
            {
                Normal = new SharpLook()
                {
                    Background = Color.FromARGB(0xffdedede).ToSrgb(),
                    Color = Color.FromARGB(0xff000000).ToSrgb(),
                    BorderColor = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowColor = Color.FromARGB(0x80000000).ToSrgb(),
                    ShadowOffset = scaleHelper.Scaled(new IntVector2(8, 8)),
                    Padding = scaleHelper.Scaled(new IntPad(40)),
                    Border = scaleHelper.Scaled(new IntPad(5)),
                    Margin = scaleHelper.Scaled(new IntPad(40)),
                },
                Hover = new SharpLook()
                {
                    Background = Color.FromARGB(0xffd2d2d2).ToSrgb(),
                    Color = Color.FromARGB(0xff000000).ToSrgb(),
                    BorderColor = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowColor = Color.FromARGB(0x80000000).ToSrgb(),
                    ShadowOffset = scaleHelper.Scaled(new IntVector2(8, 8)),
                    Padding = scaleHelper.Scaled(new IntPad(40)),
                    Border = scaleHelper.Scaled(new IntPad(5)),
                    Margin = scaleHelper.Scaled(new IntPad(40)),
                },
                Active = new SharpLook()
                {
                    Background = Color.FromARGB(0xffdedede).ToSrgb(),
                    Color = Color.FromARGB(0xff000000).ToSrgb(),
                    BorderColor = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowColor = Color.FromARGB(0x80000000).ToSrgb(),
                    ShadowOffset = scaleHelper.Scaled(new IntVector2(8, 8)),
                    Padding = scaleHelper.Scaled(new IntPad(40)),
                    Border = scaleHelper.Scaled(new IntPad(5)),
                    Margin = scaleHelper.Scaled(new IntPad(40)),
                },
                HoverAndActive = new SharpLook()
                {
                    Background = Color.FromARGB(0xffdadada).ToSrgb(),
                    Color = Color.FromARGB(0xff000000).ToSrgb(),
                    BorderColor = Color.FromARGB(0xff000000).ToSrgb(),
                    ShadowColor = Color.FromARGB(0x80000000).ToSrgb(),
                    ShadowOffset = scaleHelper.Scaled(new IntVector2(8, 8)),
                    Padding = scaleHelper.Scaled(new IntPad(40)),
                    Border = scaleHelper.Scaled(new IntPad(5)),
                    Margin = scaleHelper.Scaled(new IntPad(40)),
                },
                Focus = new SharpLook()
                {
                    Background = Color.FromARGB(0xffdedede).ToSrgb(),
                    Color = Color.FromARGB(0xff000000).ToSrgb(),
                    BorderColor = Color.FromARGB(0xff4376a9).ToSrgb(),
                    ShadowColor = Color.FromARGB(0x80000000).ToSrgb(),
                    ShadowOffset = scaleHelper.Scaled(new IntVector2(8, 8)),
                    Padding = scaleHelper.Scaled(new IntPad(40)),
                    Border = scaleHelper.Scaled(new IntPad(5)),
                    Margin = scaleHelper.Scaled(new IntPad(40)),
                },
                HoverAndFocus = new SharpLook()
                {
                    Background = Color.FromARGB(0xffd2d2d2).ToSrgb(),
                    Color = Color.FromARGB(0xff000000).ToSrgb(),
                    BorderColor = Color.FromARGB(0xff4376a9).ToSrgb(),
                    ShadowColor = Color.FromARGB(0x80000000).ToSrgb(),
                    ShadowOffset = scaleHelper.Scaled(new IntVector2(8, 8)),
                    Padding = scaleHelper.Scaled(new IntPad(40)),
                    Border = scaleHelper.Scaled(new IntPad(5)),
                    Margin = scaleHelper.Scaled(new IntPad(40)),
                },
                HoverAndActiveAndFocus = new SharpLook()
                {
                    Background = Color.FromARGB(0xffdadada).ToSrgb(),
                    Color = Color.FromARGB(0xff000000).ToSrgb(),
                    BorderColor = Color.FromARGB(0xff4376a9).ToSrgb(),
                    ShadowColor = Color.FromARGB(0x80000000).ToSrgb(),
                    ShadowOffset = scaleHelper.Scaled(new IntVector2(8, 8)),
                    Padding = scaleHelper.Scaled(new IntPad(40)),
                    Border = scaleHelper.Scaled(new IntPad(5)),
                    Margin = scaleHelper.Scaled(new IntPad(40)),
                }
            };
        }
    }
}

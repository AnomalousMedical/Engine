using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGui
{
    static class SharpProgressExtensions
    {
        public static void Process(this SharpProgressHorizontal slider, float percent, SharpGuiState state, SharpGuiBuffer buffer, SharpStyle style)
        {
            Guid id = slider.Id;
            var rect = slider.Rect;
            int left = rect.Left;
            int top = rect.Top;
            int right = rect.Right;
            int bottom = rect.Bottom;

            //Draw
            var look = state.GetLookForId(id, style);

            //Draw shadow
            if (look.ShadowOffset.x > 0 && look.ShadowOffset.y > 0)
            {
                var shadowOffset = look.ShadowOffset;
                var shadowLeft = left + shadowOffset.x;
                var shadowTop = top + shadowOffset.y;
                var shadowRight = right + shadowOffset.x;
                var shadowBottom = bottom + shadowOffset.y;

                buffer.DrawQuad(shadowLeft, shadowTop, shadowRight, shadowBottom, look.ShadowColor);
            }

            //Draw border
            buffer.DrawQuad(left, top, right, bottom, look.BorderColor);

            // Render the scrollbar
            var mainLeft = left + look.Border.Left;
            var mainTop = top + look.Border.Top;
            var mainRight = right - look.Border.Right;
            var mainBottom = bottom - look.Border.Bottom;
            buffer.DrawQuad(mainLeft, mainTop, mainRight, mainBottom, look.Background);

            // Render scroll button
            var buttonAreaLeft = mainLeft + look.Padding.Left;
            var withinMarginWidth = mainRight - buttonAreaLeft - look.Padding.Right;
            int buttonWidth = (int)(withinMarginWidth * percent);// Math.Max(withinMarginWidth / max, 1);
            var buttonLeft = buttonAreaLeft;
            var buttonTop = mainTop + look.Padding.Top;
            int buttonRight = buttonLeft + buttonWidth;
            int buttonBottom = mainBottom - look.Padding.Bottom;
            buffer.DrawQuad(buttonLeft, buttonTop, buttonRight, buttonBottom, look.Color);
        }

        //public static bool Process(this SharpSliderVertical slider, ref int value, SharpGuiState state, SharpGuiBuffer buffer, SharpStyle style, Guid? navLeft, Guid? navRight)
        //{
        //    Guid id = slider.Id;
        //    var rect = slider.Rect;
        //    int left = rect.Left;
        //    int top = rect.Top;
        //    int right = rect.Right;
        //    int bottom = rect.Bottom;
        //    int max = slider.Max;

        //    state.GrabFocus(id);

        //    // Check for mouse activation
        //    if (state.RegionHitByMouse(left, top, right, bottom))
        //    {
        //        state.TrySetActiveItem(id, state.MouseDown);
        //    }

        //    //Draw
        //    var look = state.GetLookForId(id, style);

        //    //Draw shadow
        //    if (look.ShadowOffset.x > 0 && look.ShadowOffset.y > 0)
        //    {
        //        var shadowOffset = look.ShadowOffset;
        //        var shadowLeft = left + shadowOffset.x;
        //        var shadowTop = top + shadowOffset.y;
        //        var shadowRight = right + shadowOffset.x;
        //        var shadowBottom = bottom + shadowOffset.y;

        //        buffer.DrawQuad(shadowLeft, shadowTop, shadowRight, shadowBottom, look.ShadowColor);
        //    }

        //    //Draw border
        //    buffer.DrawQuad(left, top, right, bottom, look.BorderColor);

        //    // Render the scrollbar
        //    var mainLeft = left + look.Border.Left;
        //    var mainTop = top + look.Border.Top;
        //    var mainRight = right - look.Border.Right;
        //    var mainBottom = bottom - look.Border.Bottom;
        //    buffer.DrawQuad(mainLeft, mainTop, mainRight, mainBottom, look.Background);

        //    // Render scroll button
        //    //var buttonAreaTop = ;
        //    var buttonAreaBottom = mainBottom - look.Padding.Bottom;
        //    var withinMarginHeight = buttonAreaBottom - mainTop + look.Padding.Top;
        //    int buttonHeight = withinMarginHeight / max;
        //    var buttonLeft = mainLeft + look.Padding.Left;
        //    var buttonTop = buttonAreaBottom - value * buttonHeight - buttonHeight;
        //    int buttonRight = mainRight - look.Padding.Right;
        //    int buttonBottom = buttonTop + buttonHeight;
        //    buffer.DrawQuad(buttonLeft, buttonTop, buttonRight, buttonBottom, look.Color);

        //    // Update widget value
        //    bool stealFocus = false;
        //    bool returnVal = false;
        //    int v = value;
        //    if (state.ActiveItem == id)
        //    {
        //        int mousepos = buttonAreaBottom - state.MouseY;
        //        if (mousepos < 0) { mousepos = 0; }
        //        if (mousepos > withinMarginHeight) { mousepos = withinMarginHeight; }

        //        v = mousepos / buttonHeight;
        //        stealFocus = true;
        //    }

        //    if (state.ProcessFocus(id, navLeft: navLeft, navRight: navRight))
        //    {
        //        switch (state.KeyEntered)
        //        {
        //            case Engine.Platform.KeyboardButtonCode.KC_DOWN:
        //                --v;
        //                break;
        //            case Engine.Platform.KeyboardButtonCode.KC_UP:
        //                ++v;
        //                break;
        //        }

        //        switch (state.GamepadButtonEntered)
        //        {
        //            case Engine.Platform.GamepadButtonCode.XInput_DPadDown:
        //                --v;
        //                break;
        //            case Engine.Platform.GamepadButtonCode.XInput_DPadUp:
        //                ++v;
        //                break;
        //        }
        //    }

        //    if (v < 0)
        //    {
        //        v = 0;
        //    }
        //    else if (v >= max)
        //    {
        //        v = max - 1;
        //    }

        //    if (v != value)
        //    {
        //        value = v;
        //        returnVal = true;
        //    }

        //    if (stealFocus)
        //    {
        //        state.StealFocus(id);
        //    }

        //    return returnVal;
        //}
    }
}

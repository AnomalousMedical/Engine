using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGui
{
    static class SharpButtonExtensions
    {
        public static bool Process(this SharpButton button, SharpGuiState state, SharpGuiBuffer buffer, Font font, SharpStyle style, Guid? navUp, Guid? navDown, Guid? navLeft, Guid? navRight)
        {
            Guid id = button.Id;

            var rect = button.Rect;
            int left = rect.Left;
            int top = rect.Top;
            int right = rect.Right;
            int bottom = rect.Bottom;

            state.GrabFocus(id);

            // Check whether the button should be active
            bool regionHit = state.RegionHitByMouse(left, top, right, bottom);
            if (regionHit)
            {
                state.TrySetActiveItem(id, state.MouseDown);
            }

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

            //Draw main button
            var mainLeft = left + look.Border.Left;
            var mainTop = top + look.Border.Top;
            var mainRight = right - look.Border.Right;
            var mainBottom = bottom - look.Border.Bottom;

            buffer.DrawQuad(mainLeft, mainTop, mainRight, mainBottom, look.Background);

            //Draw text
            if(button.Text != null)
            {
                var textLeft = mainLeft + look.Padding.Left;
                var textTop = mainTop + look.Padding.Right;

                buffer.DrawText(textLeft, textTop, look.Color, button.Text, font);
            }

            //Determine clicked
            bool result = false;
            if (regionHit && !state.MouseDown && state.ActiveItem == id)
            {
                state.StealFocus(id);
                result = true;
            }
            
            if (state.ProcessFocus(id, navUp: navUp, navDown: navDown, navLeft: navLeft, navRight: navRight))
            {
                switch (state.KeyEntered)
                {
                    case Engine.Platform.KeyboardButtonCode.KC_RETURN:
                        result = true;
                        break;
                }

                switch (state.GamepadButtonEntered)
                {
                    case Engine.Platform.GamepadButtonCode.XInput_A:
                        result = true;
                        break;
                }
            }
            return result;
        }

        public static IntSize2 GetDesiredSize(this SharpButton button, Font font, SharpGuiState state, SharpStyle style)
        {
            var look = state.GetLookForId(button.Id, style);

            IntSize2 result = look.Border.ToSize() + look.Padding.ToSize();
            if (button.Text != null)
            {
                result += font.MeasureText(button.Text);
            }
            return result;
        }
    }
}

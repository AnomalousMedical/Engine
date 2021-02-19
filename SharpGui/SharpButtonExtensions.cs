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
        public static bool Process(this SharpButton button, SharpGuiState state, SharpGuiBuffer buffer, Font font, SharpStyle style)
        {
            Guid id = button.Id;
            state.GrabKeyboardFocus(id);
            var look = state.GetLookForId(id, style);

            var rect = button.Rect;
            int left = rect.Left + look.Margin.Left;
            int top = rect.Top + look.Margin.Top;
            int right = rect.Right - look.Margin.Right;
            int bottom = rect.Bottom - look.Margin.Bottom;

            // Check whether the button should be active
            bool regionHit = state.RegionHitByMouse(left, top, right, bottom);
            if (regionHit)
            {
                state.TrySetActiveItem(id, state.MouseDown);
            }

            //Draw
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
            bool clicked = false;
            if (regionHit && !state.MouseDown && state.ActiveItem == id)
            {
                state.StealKeyboardFocus(id);
                clicked = true;
            }
            if (state.ProcessKeyboardFocus(id))
            {
                switch (state.KeyEntered)
                {
                    case Engine.Platform.KeyboardButtonCode.KC_RETURN:
                        clicked = true;
                        break;
                }
            }
            return clicked;
        }

        public static IntSize2 GetDesiredSize(this SharpButton button, Font font, SharpGuiState state, SharpStyle style)
        {
            var look = state.GetLookForId(button.Id, style);

            IntSize2 result = look.Margin.ToSize() + look.Border.ToSize() + look.Padding.ToSize();
            if (button.Text != null)
            {
                result += font.MeasureText(button.Text);
            }
            return result;
        }
    }
}

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
        private static SharpStyle style = SharpStyle.CreateComplete();

        public static bool Process(this SharpButton button, SharpGuiState state, SharpGuiBuffer buffer, Font font)
        {
            Guid id = button.Id;
            var rect = button.Rect;
            int x = rect.Left;
            int y = rect.Top;
            int width = rect.Width;
            int height = rect.Height;

            state.GrabKeyboardFocus(id);

            // Check whether the button should be active
            bool regionHit = state.RegionHitByMouse(x, y, width, height);
            if (regionHit)
            {
                state.TrySetActiveItem(id, state.MouseDown);
            }

            //Draw
            var look = state.GetLookForId(id, style);

            //Draw shadow
            if (look.ShadowOffset.x > 0 && look.ShadowOffset.y > 0)
            {
                var shadowX = x + look.ShadowOffset.x;
                var shadowY = y + look.ShadowOffset.y;
                var shadowWidth = width;
                var shadowHeight = height;

                //if (state.FocusedItem == button.Id)
                //{
                //    shadowWidth += look.FocusSize.Width;
                //    shadowHeight += look.FocusSize.Height;
                //}

                buffer.DrawQuad(shadowX, shadowY, shadowWidth, shadowHeight, look.ShadowColor);
            }

            //Draw keyboard focus
            //if (state.FocusedItem == button.Id && look.FocusSize.Width > 0 && look.FocusSize.Height > 0)
            //{
            //    buffer.DrawQuad
            //    (
            //        x - look.FocusSize.Width / 2, 
            //        y - look.FocusSize.Height / 2, 
            //        width + look.FocusSize.Width, 
            //        height + look.FocusSize.Height, 
            //        look.FocusHighlightColor
            //    );
            //}

            //Draw main button
            buffer.DrawQuad(x, y, width, height, look.Background);

            //Draw text
            if(button.Text != null)
            {
                buffer.DrawText(x, y, look.Color, button.Text, font);
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

        public static IntSize2 GetDesiredSize(this SharpButton button, Font font, SharpGuiState state)
        {
            var look = state.GetLookForId(button.Id, style);

            IntSize2 result = new IntSize2();
            if (button.Text != null)
            {
                result += font.MeasureText(button.Text);
            }
            return result;
        }
    }
}

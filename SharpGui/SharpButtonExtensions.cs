using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    static class SharpButtonExtensions
    {
        private static Color HoverAndActive = Color.FromARGB(0xffff0000);
        private static Color Hover = Color.FromARGB(0xffffffff);
        private static Color Normal = Color.FromARGB(0xffaaaaaa);
        private static Color ShadowColor = Color.FromARGB(0xff000000);
        private static Color FocusColor = Color.FromARGB(0xff0000ff);
        private static Color TextColor = Color.FromARGB(0xff000000);

        public static bool Process(this SharpButton button, SharpGuiState state, SharpGuiBuffer buffer, Font font)
        {
            Guid id = button.Id;
            int x = button.X;
            int y = button.Y;
            int width = button.Width;
            int height = button.Height;

            state.GrabKeyboardFocus(id);

            // Check whether the button should be hot
            bool regionHit = state.RegionHitByMouse(x, y, width, height);
            if (regionHit)
            {
                state.MouseHoverItem = id;
                if (state.ActiveItem == Guid.Empty && state.MouseDown)
                {
                    state.ActiveItem = id;
                }
            }

            var shadowX = x + 8;
            var shadowY = y + 8;
            var shadowWidth = width;
            var shadowHeight = height;
            //Make shadow bigger if keyboard focused
            if (state.FocusedItem == button.Id)
            {
                shadowWidth += 12;
                shadowHeight += 12;
            }
            buffer.DrawQuad(shadowX, shadowY, shadowWidth, shadowHeight, ShadowColor);
            //Draw keyboard focus
            if (state.FocusedItem == button.Id)
            {
                buffer.DrawQuad(x - 6, y - 6, width + 12, height + 12, FocusColor);
            }

            //Draw button
            var color = Normal;
            if (state.MouseHoverItem == id)
            {
                if (state.ActiveItem == id)
                {
                    color = HoverAndActive;
                }
                else
                {
                    color = Hover;
                }
            }
            buffer.DrawQuad(x, y, width, height, color);

            //Draw text
            if(button.Text != null)
            {
                buffer.DrawText(x, y, TextColor, button.Text, font);
            }

            //Determine clicked
            bool clicked = false;
            if (regionHit && !state.MouseDown && state.ActiveItem == id)
            {
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
    }
}

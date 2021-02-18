using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGui
{
    static class SharpSliderExtensions
    {
        private static int ButtonMargin = 8;
        private static Color ButtonBackgroundColor = Color.FromARGB(0xff777777);
        private static Color HoverAndActive = Color.FromARGB(0xffff0000);
        private static Color Hover = Color.FromARGB(0xffffffff);
        private static Color Normal = Color.FromARGB(0xffaaaaaa);
        private static Color FocusColor = Color.FromARGB(0xff0000ff);

        public static bool Process(this SharpSlider slider, ref int value, SharpGuiState state, SharpGuiBuffer buffer)
        {
            Guid id = slider.Id;
            int x = slider.X;
            int y = slider.Y;
            int width = slider.Width;
            int height = slider.Height;
            int max = slider.Max;

            state.GrabKeyboardFocus(id);

            var doubleMargin = ButtonMargin * 2;
            var withinMarginHeight = height - doubleMargin;
            int buttonWidth = width - doubleMargin;
            int buttonHeight = withinMarginHeight / (max + 1);
            var buttonX = x + ButtonMargin;
            var buttonY = y + ButtonMargin + value * buttonHeight;

            // Check for focus
            if (state.RegionHitByMouse(x, y, width, height))
            {
                state.TrySetActiveItem(id, state.MouseDown);
            }

            if (state.FocusedItem == slider.Id)
            {
                buffer.DrawQuad(x - 6, y - 6, width + 12, height + 12, FocusColor);
            }

            // Render the scrollbar
            buffer.DrawQuad(x, y, width, height, ButtonBackgroundColor);

            // Render scroll button
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
            buffer.DrawQuad(buttonX, buttonY, buttonWidth, buttonHeight, color);

            // Update widget value
            bool returnVal = false;
            int v = value;
            if (state.ActiveItem == id)
            {
                int mousepos = state.MouseY - (y + ButtonMargin);
                if (mousepos < 0) { mousepos = 0; }
                if (mousepos > withinMarginHeight) { mousepos = withinMarginHeight; }

                v = mousepos / buttonHeight;
            }

            if (state.ProcessKeyboardFocus(id))
            {
                switch (state.KeyEntered)
                {
                    case Engine.Platform.KeyboardButtonCode.KC_DOWN:
                    case Engine.Platform.KeyboardButtonCode.KC_LEFT:
                        --v;
                        break;
                    case Engine.Platform.KeyboardButtonCode.KC_UP:
                    case Engine.Platform.KeyboardButtonCode.KC_RIGHT:
                        ++v;
                        break;
                }
            }

            if(v < 0)
            {
                v = 0;
            }
            else if (v > max)
            {
                v = max;
            }

            if (v != value)
            {
                value = v;
                returnVal = true;
            }

            return returnVal;
        }
    }
}

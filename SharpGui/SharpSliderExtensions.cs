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
            int left = slider.Rect.Left;
            int top = slider.Rect.Top;
            int right = slider.Rect.Right;
            int bottom = slider.Rect.Bottom;
            int max = slider.Max;

            state.GrabKeyboardFocus(id);

            var withinMarginHeight = slider.Rect.Height - ButtonMargin * 2;
            int buttonHeight = withinMarginHeight / max;
            var buttonLeft = left + ButtonMargin;
            var buttonTop = top + ButtonMargin + value * buttonHeight;
            int buttonRight = right - ButtonMargin;
            int buttonBottom = buttonTop + buttonHeight;

            // Check for focus
            if (state.RegionHitByMouse(left, top, right, bottom))
            {
                state.TrySetActiveItem(id, state.MouseDown);
            }

            if (state.FocusedItem == slider.Id)
            {
                buffer.DrawQuad(left - 6, top - 6, right + 6, bottom + 6, FocusColor);
            }

            // Render the scrollbar
            buffer.DrawQuad(left, top, right, bottom, ButtonBackgroundColor);

            // Render scroll button
            var color = Normal;
            if (state.HoverItem == id)
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
            buffer.DrawQuad(buttonLeft, buttonTop, buttonRight, buttonBottom, color);

            // Update widget value
            bool stealFocus = false;
            bool returnVal = false;
            int v = value;
            if (state.ActiveItem == id)
            {
                int mousepos = state.MouseY - (top + ButtonMargin);
                if (mousepos < 0) { mousepos = 0; }
                if (mousepos > withinMarginHeight) { mousepos = withinMarginHeight; }

                v = mousepos / buttonHeight;
                stealFocus = true;
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
            else if (v >= max)
            {
                v = max - 1;
            }

            if (v != value)
            {
                value = v;
                returnVal = true;
            }

            if(stealFocus)
            {
                state.StealKeyboardFocus(id);
            }

            return returnVal;
        }
    }
}

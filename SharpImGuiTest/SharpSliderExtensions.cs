using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{ 
    static class SharpSliderExtensions
    {
        private static int SliderBoxMargin = 8;
        private static Color SliderBoxBackgroundColor = Color.FromARGB(0xff777777);
        private static Color SliderFocusAndActive = Color.FromARGB(0xffff0000);
        private static Color SliderFocus = Color.FromARGB(0xffffffff);
        private static Color SliderNormal = Color.FromARGB(0xffaaaaaa);

        public static bool Slider(this SharpSlider slider, ref int value, SharpGuiState state, SharpGuiBuffer buffer)
        {
            Guid id = slider.Id;
            int x = slider.X;
            int y = slider.Y;
            int width = slider.Width;
            int height = slider.Height;
            int max = slider.Max;

            var doubleMargin = SliderBoxMargin * 2;
            var withinMarginHeight = height - doubleMargin;
            var buttonX = x + SliderBoxMargin;
            var buttonY = y + SliderBoxMargin;
            int buttonWidth = width - doubleMargin;
            int buttonHeight = withinMarginHeight / (max + 1);

            // Calculate mouse cursor's relative y offset
            int ypos = value * buttonHeight;
            buttonY += ypos;

            // Check for hotness
            if (state.RegionHit(x, y, width, height))
            {
                state.FocusItem = id;
                if (state.ActiveItem == Guid.Empty && state.MouseDown)
                {
                    state.ActiveItem = id;
                }
            }

            // Render the scrollbar
            buffer.DrawQuad(x, y, width, height, SliderBoxBackgroundColor);

            // Render scroll button
            var color = SliderNormal;
            if (state.FocusItem == id)
            {
                if (state.ActiveItem == id)
                {
                    color = SliderFocusAndActive;
                }
                else
                {
                    color = SliderFocus;
                }
            }
            buffer.DrawQuad(buttonX, buttonY, buttonWidth, buttonHeight, color);

            // Update widget value
            if (state.ActiveItem == id)
            {
                int mousepos = state.MouseY - (y + SliderBoxMargin);
                if (mousepos < 0) { mousepos = 0; }
                if (mousepos > withinMarginHeight) { mousepos = withinMarginHeight; }

                int v = mousepos / buttonHeight;
                if (v > max)
                {
                    v = max;
                }
                if (v != value)
                {
                    value = v;
                    return true;
                }
            }

            return false;
        }
    }
}

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

        public static bool Process(this SharpButton button, SharpGuiState state, SharpGuiBuffer buffer)
        {
            Guid id = button.Id;
            int x = button.X;
            int y = button.Y;
            int width = button.Width;
            int height = button.Height;

            state.GrabKeyboardFocus(id);

            // Check whether the button should be hot
            bool regionHit = state.RegionHit(x, y, width, height);
            if (regionHit)
            {
                state.MouseHoverItem = id;
                if (state.ActiveItem == Guid.Empty && state.MouseDown)
                {
                    state.ActiveItem = id;
                }
            }

            //Draw shadow
            buffer.DrawQuad(x + 8, y + 8, width, height, ShadowColor);

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

            //Determine clicked
            bool clicked = false;
            if (regionHit && !state.MouseDown && state.ActiveItem == id)
            {
                clicked = true;
            }
            return clicked;
        }
    }
}

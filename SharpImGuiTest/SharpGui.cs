﻿using DiligentEngine;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    public class SharpGui
    {
        private Guid EmptySpace = Guid.NewGuid(); //A guid for when the user clicks on empty space. This gets considered to be active
        private readonly SharpGuiBuffer buffer;
        private readonly SharpGuiRenderer renderer;
        private readonly SharpGuiState state = new SharpGuiState();

        public SharpGui(SharpGuiBuffer buffer, SharpGuiRenderer renderer)
        {
            this.buffer = buffer;
            this.renderer = renderer;
        }

        public void SetMouseState(int x, int y, bool mouseDown)
        {
            state.SetMouseState(x, y, mouseDown);
        }

        public void Begin()
        {
            state.FocusItem = Guid.Empty;
            buffer.Begin();
        }

        public void End()
        {
            if (state.MouseDown)
            {
                //This needs to say nested, above check is just mouse up / down
                //If ActiveItem is empty at the end of the frame, consider empty space to be clicked.
                if (state.ActiveItem == Guid.Empty)
                {
                    state.ActiveItem = EmptySpace;
                }
            }
            else
            {
                state.ActiveItem = Guid.Empty;
            }
        }

        /// <summary>
        /// Draw a button. Returns true if the button was clicked.
        /// </summary>
        /// <param name="button">The button to draw.</param>
        /// <returns>True if clicked.</returns>
        public bool Button(SharpButton button)
        {
            return button.Button(state, buffer);
        }

        private int SliderBoxMargin = 8;
        private Color SliderBoxBackgroundColor = Color.FromARGB(0xff777777);
        private Color SliderFocusAndActive = Color.FromARGB(0xffff0000);
        private Color SliderFocus = Color.FromARGB(0xffffffff);
        private Color SliderNormal = Color.FromARGB(0xffaaaaaa);

        public bool Slider(Guid id, int x, int y, int width, int height, int max, ref int value)
        {
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
                if(v > max)
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

        public void Render(IDeviceContext immediateContext)
        {
            renderer.Render(buffer, immediateContext);
        }
    }
}

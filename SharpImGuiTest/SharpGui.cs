using DiligentEngine;
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
            state.MouseHoverItem = Guid.Empty;
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
            return button.Process(state, buffer);
        }

        /// <summary>
        /// Draw a slider. Returns true if the passed in value changed.
        /// </summary>
        /// <param name="slider">The slider to draw.</param>
        /// <param name="value">The current value of the slider.</param>
        /// <returns>True if value changed.</returns>
        public bool Slider(SharpSlider slider, ref int value)
        {
            return slider.Process(ref value, state, buffer);
        }

        public void Render(IDeviceContext immediateContext)
        {
            renderer.Render(buffer, immediateContext);
        }
    }
}

using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    public class SharpGui : IDisposable
    {
        private readonly SharpGuiBuffer buffer;
        private readonly SharpGuiRenderer renderer;
        private readonly EventManager eventManager;
        private readonly SharpGuiState state = new SharpGuiState();
        private KeyboardButtonCode lastKeyPressed = KeyboardButtonCode.KC_UNASSIGNED;

        public SharpGui(SharpGuiBuffer buffer, SharpGuiRenderer renderer, EventManager eventManager)
        {
            this.buffer = buffer;
            this.renderer = renderer;
            this.eventManager = eventManager;
            eventManager.Keyboard.KeyReleased += Keyboard_KeyReleased;
        }

        public void Dispose()
        {
            eventManager.Keyboard.KeyReleased -= Keyboard_KeyReleased;
        }

        private void Keyboard_KeyReleased(KeyboardButtonCode keyCode)
        {
            lastKeyPressed = keyCode;
        }

        public void Begin()
        {
            var keyboard = eventManager.Keyboard;
            var mouse = eventManager.Mouse;
            state.Begin(
                mouse.AbsolutePosition.x, mouse.AbsolutePosition.y, 
                mouse.buttonDown(MouseButtonCode.MB_BUTTON0), 
                lastKeyPressed, 
                keyboard.isModifierDown(Modifier.Shift), 
                keyboard.isModifierDown(Modifier.Alt), 
                keyboard.isModifierDown(Modifier.Ctrl));

            lastKeyPressed = KeyboardButtonCode.KC_UNASSIGNED;
            buffer.Begin();
        }

        public void End()
        {
            state.End();
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

        public void Text(int x, int y, Color color, String text)
        {
            int xOffset = 0;
            var font = renderer.Font;
            foreach (var c in text)
            {
                uint charCode = c;
                if (font.CharMap.TryGetValue(c, out var charMap))
                {
                    charCode = charMap;
                }
                GlyphInfo glyphInfo;
                if (font.GlyphInfo.TryGetValue(charCode, out glyphInfo))
                {
                    buffer.DrawTextQuad(x + xOffset + (int)glyphInfo.bearingX, y + (int)glyphInfo.bearingY, (int)glyphInfo.width, (int)glyphInfo.height, ref color, ref glyphInfo.uvRect);
                    int fullAdvance = (int)glyphInfo.advance + (int)glyphInfo.bearingX;
                    xOffset += fullAdvance;
                }
            }
        }

        public void Render(IDeviceContext immediateContext)
        {
            renderer.Render(buffer, immediateContext);
        }
    }
}

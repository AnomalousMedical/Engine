using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGui
{
    class SharpGuiImpl : ISharpGui, IDisposable
    {
        private const int RepeatMs = 150;

        private readonly SharpGuiBuffer buffer;
        private readonly SharpGuiRenderer renderer;
        private readonly EventManager eventManager;
        private readonly SharpGuiState state = new SharpGuiState();
        private KeyboardButtonCode lastKeyPressed = KeyboardButtonCode.KC_UNASSIGNED;
        private KeyboardButtonCode lastKeyReleased = KeyboardButtonCode.KC_UNASSIGNED;
        private int nextRepeatCountdown = 0;
        private bool sentRepeat = false;
        private SharpStyle buttonStyle;
        private SharpStyle sliderStyle;

        public SharpGuiImpl(SharpGuiBuffer buffer, SharpGuiRenderer renderer, EventManager eventManager, IScaleHelper scaleHelper)
        {
            this.buffer = buffer;
            this.renderer = renderer;
            this.eventManager = eventManager;
            eventManager.Keyboard.KeyPressed += Keyboard_KeyPressed;
            eventManager.Keyboard.KeyReleased += Keyboard_KeyReleased;
            buttonStyle = SharpStyle.CreateComplete(scaleHelper);
            sliderStyle = SharpStyle.CreateComplete(scaleHelper);
            sliderStyle.Padding = new IntPad(8);
            sliderStyle.Active.Color = Color.FromARGB(0xff4376a9).ToSrgb();
            sliderStyle.HoverAndActive.Color = Color.FromARGB(0xff4376a9).ToSrgb();
            sliderStyle.HoverAndActiveAndFocus.Color = Color.FromARGB(0xff4376a9).ToSrgb();
        }

        public void Dispose()
        {
            eventManager.Keyboard.KeyPressed -= Keyboard_KeyPressed;
            eventManager.Keyboard.KeyReleased -= Keyboard_KeyReleased;
        }

        private void Keyboard_KeyPressed(KeyboardButtonCode keyCode, uint keyChar)
        {
            nextRepeatCountdown = RepeatMs;
            lastKeyPressed = keyCode;
            sentRepeat = false;
        }

        private void Keyboard_KeyReleased(KeyboardButtonCode keyCode)
        {
            lastKeyReleased = keyCode;
            if (lastKeyPressed == keyCode)
            {
                lastKeyPressed = KeyboardButtonCode.KC_UNASSIGNED;
            }
        }

        public void Begin(Clock clock)
        {
            var keyboard = eventManager.Keyboard;
            var mouse = eventManager.Mouse;

            var keyToSend = lastKeyReleased;
            if (sentRepeat)
            {
                keyToSend = KeyboardButtonCode.KC_UNASSIGNED;
            }
            if(lastKeyPressed != KeyboardButtonCode.KC_UNASSIGNED)
            {
                nextRepeatCountdown -= (int)(clock.DeltaTimeMicro * Clock.MicroToMilliseconds);
                if(nextRepeatCountdown < 0)
                {
                    nextRepeatCountdown = RepeatMs;
                    keyToSend = lastKeyPressed;
                    sentRepeat = true;
                }
            }

            state.Begin(
                mouse.AbsolutePosition.x, mouse.AbsolutePosition.y,
                mouse.buttonDown(MouseButtonCode.MB_BUTTON0),
                keyToSend,
                keyboard.isModifierDown(Modifier.Shift),
                keyboard.isModifierDown(Modifier.Alt),
                keyboard.isModifierDown(Modifier.Ctrl));

            lastKeyReleased = KeyboardButtonCode.KC_UNASSIGNED;
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
            return button.Process(state, buffer, renderer.Font, buttonStyle);
        }

        /// <summary>
        /// Draw a slider. Returns true if the passed in value changed.
        /// </summary>
        /// <param name="slider">The slider to draw.</param>
        /// <param name="value">The current value of the slider.</param>
        /// <returns>True if value changed.</returns>
        public bool Slider(SharpSlider slider, ref int value)
        {
            return slider.Process(ref value, state, buffer, sliderStyle);
        }

        public void Text(int x, int y, Color color, String text)
        {
            buffer.DrawText(x, y, color, text, renderer.Font);
        }

        public void Render(IDeviceContext immediateContext)
        {
            renderer.Render(buffer, immediateContext);
        }

        public IntSize2 MeasureButton(SharpButton sharpButton)
        {
            return sharpButton.GetDesiredSize(renderer.Font, state, buttonStyle);
        }
    }
}

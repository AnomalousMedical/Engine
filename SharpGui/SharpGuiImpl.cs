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
        private const int StartRepeatMs = 300;
        private const int RepeatMs = 200;

        private readonly SharpGuiBuffer buffer;
        private readonly SharpGuiRenderer renderer;
        private readonly EventManager eventManager;
        private readonly SharpGuiState state = new SharpGuiState();
        private KeyboardButtonCode lastKeyPressed = KeyboardButtonCode.KC_UNASSIGNED;
        private uint lastKeyCharPressed = uint.MaxValue;
        private uint immediateInjectChar = uint.MaxValue;
        private KeyboardButtonCode lastKeyReleased = KeyboardButtonCode.KC_UNASSIGNED;
        private GamepadButtonCode lastGamepadPressed = GamepadButtonCode.NUM_BUTTONS;
        private GamepadButtonCode lastGamepadReleased = GamepadButtonCode.NUM_BUTTONS;
        private int nextRepeatCountdown = 0;
        private bool sentRepeat = false;
        private SharpStyle buttonStyle;
        private SharpStyle sliderStyle;
        private SharpStyle inputStyle;
        private SharpStyle panelStyle;

        public SharpGuiImpl(SharpGuiBuffer buffer, SharpGuiRenderer renderer, EventManager eventManager, IScaleHelper scaleHelper)
        {
            this.buffer = buffer;
            this.renderer = renderer;
            this.eventManager = eventManager;
            eventManager.Keyboard.KeyPressed += Keyboard_KeyPressed;
            eventManager.Keyboard.KeyReleased += Keyboard_KeyReleased;

            eventManager.Pad1.ButtonDown += Pad1_ButtonDown;
            eventManager.Pad1.ButtonUp += Pad1_ButtonUp;

            buttonStyle = SharpStyle.CreateComplete(scaleHelper);
            
            sliderStyle = SharpStyle.CreateComplete(scaleHelper);
            sliderStyle.Padding = new IntPad(8);
            sliderStyle.Active.Color = Color.FromARGB(0xff4376a9).ToSrgb();
            sliderStyle.HoverAndActive.Color = Color.FromARGB(0xff4376a9).ToSrgb();
            sliderStyle.HoverAndActiveAndFocus.Color = Color.FromARGB(0xff4376a9).ToSrgb();

            inputStyle = SharpStyle.CreateComplete(scaleHelper);

            panelStyle = SharpStyle.CreateComplete(scaleHelper);
        }

        private void Pad1_ButtonDown(Engine.Platform.Input.Gamepad pad, GamepadButtonCode buttonCode)
        {
            nextRepeatCountdown = StartRepeatMs;
            lastKeyPressed = KeyboardButtonCode.KC_UNASSIGNED;
            immediateInjectChar = lastKeyCharPressed = uint.MaxValue;
            lastGamepadPressed = buttonCode;
            sentRepeat = false;
        }

        private void Pad1_ButtonUp(Engine.Platform.Input.Gamepad pad, GamepadButtonCode buttonCode)
        {
            lastGamepadReleased = buttonCode;
            if (lastGamepadPressed == buttonCode)
            {
                lastGamepadPressed = GamepadButtonCode.NUM_BUTTONS;
            }
        }

        public void Dispose()
        {
            eventManager.Pad1.ButtonDown -= Pad1_ButtonDown;
            eventManager.Pad1.ButtonUp -= Pad1_ButtonUp;
            eventManager.Keyboard.KeyPressed -= Keyboard_KeyPressed;
            eventManager.Keyboard.KeyReleased -= Keyboard_KeyReleased;
        }

        private void Keyboard_KeyPressed(KeyboardButtonCode keyCode, uint keyChar)
        {
            //Some keys are not valid, filter them out
            if(keyChar < 32)
            {
                keyChar = uint.MaxValue;
            }

            nextRepeatCountdown = StartRepeatMs;
            lastKeyPressed = keyCode;
            immediateInjectChar = lastKeyCharPressed = keyChar;
            lastGamepadPressed = GamepadButtonCode.NUM_BUTTONS;
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

            uint keyCharToSend = uint.MaxValue;
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
                    if (lastKeyCharPressed != uint.MaxValue)
                    {
                        keyCharToSend = lastKeyCharPressed;
                    }
                }
            }

            //This is the highest priority since it will have just been pressed
            //Do it last so it overrides anything else
            if(immediateInjectChar != uint.MaxValue)
            {
                keyCharToSend = immediateInjectChar;
            }

            var buttonToSend = lastGamepadReleased;
            if (sentRepeat)
            {
                buttonToSend = GamepadButtonCode.NUM_BUTTONS;
            }
            if(lastGamepadPressed != GamepadButtonCode.NUM_BUTTONS)
            {
                nextRepeatCountdown -= (int)(clock.DeltaTimeMicro * Clock.MicroToMilliseconds);
                if (nextRepeatCountdown < 0)
                {
                    nextRepeatCountdown = RepeatMs;
                    buttonToSend = lastGamepadPressed;
                    sentRepeat = true;
                }
            }

            state.Begin(
                mouse.AbsolutePosition.x, mouse.AbsolutePosition.y,
                mouse.buttonDown(MouseButtonCode.MB_BUTTON0),
                keyToSend,
                keyCharToSend,
                keyboard.isModifierDown(Modifier.Shift),
                keyboard.isModifierDown(Modifier.Alt),
                keyboard.isModifierDown(Modifier.Ctrl),
                buttonToSend);

            lastKeyReleased = KeyboardButtonCode.KC_UNASSIGNED;
            lastGamepadReleased = GamepadButtonCode.NUM_BUTTONS;
            immediateInjectChar = uint.MaxValue;
            buffer.Begin();
        }

        public void End()
        {
            state.End();
        }

        public bool Button(SharpButton button, Guid? navUp = null, Guid? navDown = null, Guid? navLeft = null, Guid? navRight = null)
        {
            return button.Process(state, buffer, renderer.Font, buttonStyle, navUp, navDown, navLeft, navRight);
        }

        public bool Input(SharpInput input, Guid? navUp = null, Guid? navDown = null, Guid? navLeft = null, Guid? navRight = null)
        {
            return input.Process(state, buffer, renderer.Font, inputStyle, navUp, navDown, navLeft, navRight);
        }

        public bool Slider(SharpSliderHorizontal slider, ref int value, Guid? navUp = null, Guid? navDown = null)
        {
            return slider.Process(ref value, state, buffer, sliderStyle, navUp, navDown);
        }

        public bool Slider(SharpSliderVertical slider, ref int value, Guid? navLeft = null, Guid? navRight = null)
        {
            return slider.Process(ref value, state, buffer, sliderStyle, navLeft, navRight);
        }

        public void Panel(SharpPanel panel)
        {
            panel.Process(state, buffer, panelStyle);
        }

        public void Text(int x, int y, Color color, String text)
        {
            buffer.DrawText(x, y, int.MaxValue, color, text, renderer.Font);
        }

        public void Text(int x, int y, Color color, String text, int maxWidth)
        {
            buffer.DrawText(x, y, x + maxWidth, color, text, renderer.Font);
        }

        public void Text(SharpText text)
        {
            buffer.DrawText(text.Rect.Left, text.Rect.Top, text.Rect.Right, text.color, text.Text, renderer.Font);
        }

        public void Render(IDeviceContext immediateContext)
        {
            renderer.Render(buffer, immediateContext);
        }

        public IntSize2 MeasureButton(SharpButton button)
        {
            return button.GetDesiredSize(renderer.Font, state, buttonStyle);
        }

        public IntSize2 MeasureInput(SharpInput input)
        {
            return input.GetDesiredSize(renderer.Font, state, inputStyle);
        }

        public IntSize2 MeasurePanel(SharpPanel panel)
        {
            return panel.GetDesiredSize(state, panelStyle);
        }

        public IntSize2 MeasureText(String text)
        {
            return renderer.Font.MeasureText(text);
        }

        public IntSize2 MeasureText(StringBuilder text)
        {
            return renderer.Font.MeasureText(text);
        }

        public void StealFocus(Guid id)
        {
            state.StealFocus(id);
        }

        public Guid ActiveItem => state.ActiveItem;

        public Guid HoverItem => state.HoverItem;

        public Guid FocusedItem => state.FocusedItem;

        public KeyboardButtonCode KeyEntered => state.KeyEntered;

        public GamepadButtonCode GamepadButtonEntered => state.GamepadButtonEntered;
    }
}

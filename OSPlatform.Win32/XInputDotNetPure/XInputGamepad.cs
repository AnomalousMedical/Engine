using Engine.Platform;
using Engine.Platform.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XInputDotNetPure;

namespace OSPlatform.Win32.XInputDotNetPure
{
    class XInputGamepad : GamepadHardware
    {
        private PlayerIndex playerIndex;
        private GamePadState lastState;

        public XInputGamepad(Gamepad pad) 
            : base(pad)
        {
            this.playerIndex = PlayerIndex.One;
            switch (pad.Id)
            {
                case GamepadId.Pad1:
                    this.playerIndex = PlayerIndex.One;
                    break;
                case GamepadId.Pad2:
                    this.playerIndex = PlayerIndex.Two;
                    break;
                case GamepadId.Pad3:
                    this.playerIndex = PlayerIndex.Three;
                    break;
                case GamepadId.Pad4:
                    this.playerIndex = PlayerIndex.Four;
                    break;
            }

            lastState = GamePad.GetState(playerIndex);
        }

        public override void Dispose()
        {

        }

        private void process(ButtonState last, ButtonState current, GamepadButtonCode code)
        {
            switch (last)
            {
                case ButtonState.Pressed:
                    if(current == ButtonState.Released)
                    {
                        this.fireButtonUp(code);
                    }
                    break;
                case ButtonState.Released:
                    if (current == ButtonState.Pressed)
                    {
                        this.fireButtonDown(code);
                    }
                    break;
            }
        }

        public override void Update()
        {
            //Have to poll xinput
            var state = GamePad.GetState(playerIndex);
            if(lastState.PacketNumber != state.PacketNumber)
            {
                process(lastState.Buttons.A, state.Buttons.A, GamepadButtonCode.XInput_A);
                process(lastState.Buttons.B, state.Buttons.B, GamepadButtonCode.XInput_B);
                process(lastState.Buttons.X, state.Buttons.X, GamepadButtonCode.XInput_X);
                process(lastState.Buttons.Y, state.Buttons.Y, GamepadButtonCode.XInput_Y);
                process(lastState.Buttons.Back, state.Buttons.Back, GamepadButtonCode.XInput_Select);
                process(lastState.Buttons.Start, state.Buttons.Start, GamepadButtonCode.XInput_Start);
                process(lastState.Buttons.LeftShoulder, state.Buttons.LeftShoulder, GamepadButtonCode.XInput_LeftShoulder);
                process(lastState.Buttons.RightShoulder, state.Buttons.RightShoulder, GamepadButtonCode.XInput_RightShoulder);
                process(lastState.Buttons.LeftStick, state.Buttons.LeftStick, GamepadButtonCode.XInput_LThumb);
                process(lastState.Buttons.RightStick, state.Buttons.RightStick, GamepadButtonCode.XInput_RThumb);
                process(lastState.Buttons.Guide, state.Buttons.Guide, GamepadButtonCode.XInput_Guide);

                process(lastState.Triggers.LeftState, state.Triggers.LeftState, GamepadButtonCode.XInput_LTrigger);
                process(lastState.Triggers.RightState, state.Triggers.RightState, GamepadButtonCode.XInput_RTrigger);

                process(lastState.DPad.Up, state.DPad.Up, GamepadButtonCode.XInput_DPadUp);
                process(lastState.DPad.Down, state.DPad.Down, GamepadButtonCode.XInput_DPadDown);
                process(lastState.DPad.Left, state.DPad.Left, GamepadButtonCode.XInput_DPadLeft);
                process(lastState.DPad.Right, state.DPad.Right, GamepadButtonCode.XInput_DPadRight);

                this.fireMovement(
                    new Engine.Vector2(lastState.ThumbSticks.Left.X, lastState.ThumbSticks.Left.Y),
                    new Engine.Vector2(lastState.ThumbSticks.Right.X, lastState.ThumbSticks.Right.Y),
                    lastState.Triggers.Left, 
                    lastState.Triggers.Right);

                this.lastState = state;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Platform.Input
{
    public abstract class GamepadHardware : IDisposable
    {
        private Gamepad pad;

        public GamepadHardware(Gamepad pad)
        {
            this.pad = pad;
        }

        public abstract void Dispose();

        protected void fireButtonDown(GamepadButtonCode button)
        {
            pad.fireButtonDown(button);
        }

        protected void fireButtonUp(GamepadButtonCode button)
        {
            pad.fireButtonUp(button);
        }

        protected void fireMovement(Vector2 lStick, Vector2 rStick, float lTrigger, float rTrigger)
        {
            pad.fireMovement(lStick, rStick, lTrigger, rTrigger);
        }

        public abstract void Update();
    }

    public class NullGamepadHardware : GamepadHardware
    {
        public NullGamepadHardware(Gamepad pad) : base(pad)
        {
        }

        public override void Dispose()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}

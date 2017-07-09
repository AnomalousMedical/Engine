using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Platform.Input
{
    public delegate void GamepadButtonCallback(Gamepad pad, GamepadButtonCode buttonCode);

    public class Gamepad
    {
        private bool[] buttonDownStatus = new bool[(int)GamepadButtonCode.NUM_BUTTONS];
        private bool[] pressedThisFrame = new bool[(int)GamepadButtonCode.NUM_BUTTONS];
        private bool[] downAndUpThisFrame = new bool[(int)GamepadButtonCode.NUM_BUTTONS];
        private EventManager eventManager;

        public Vector2 LStick { get; private set; }
        public Vector2 RStick { get; private set; }
        public float LTrigger { get; private set; }
        public float RTrigger { get; private set; }

        /// <summary>
        /// Called when a mouse button is pressed.
        /// </summary>
        public event GamepadButtonCallback ButtonDown;

        /// <summary>
        /// Called when a mouse button is released.
        /// </summary>
        public event GamepadButtonCallback ButtonUp;

        public Gamepad(EventManager eventManager, GamepadId padId)
        {
            this.eventManager = eventManager;
            this.Id = padId;
        }

        /// <summary>
        /// Determines if the specified button is pressed.
        /// </summary>
        /// <returns>True if the button is pressed.  False if it is not.</returns>
        public bool isButtonDown(GamepadButtonCode button)
        {
            return buttonDownStatus[(int)button];
        }

        /// <summary>
        /// Determins if the button was pressed on this frame, will be true the first frame the
        /// button is down and false every frame after that until it is released. This is intended
        /// to catch down / up events that happen before an update can be called.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool buttonDownAndUpThisFrame(GamepadButtonCode button)
        {
            return downAndUpThisFrame[(int)button];
        }

        internal void postUpdate()
        {
            pressedThisFrame[(int)GamepadButtonCode.Button0] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button1] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button2] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button3] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button4] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button5] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button6] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button7] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button8] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button9] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button10] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button11] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button12] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button13] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button14] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button15] = false;
            pressedThisFrame[(int)GamepadButtonCode.Button16] = false;

            downAndUpThisFrame[(int)GamepadButtonCode.Button0] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button1] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button2] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button3] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button4] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button5] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button6] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button7] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button8] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button9] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button10] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button11] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button12] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button13] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button14] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button15] = false;
            downAndUpThisFrame[(int)GamepadButtonCode.Button16] = false;
        }

        internal void fireButtonDown(GamepadButtonCode button)
        {
            int index = (int)button;

            buttonDownStatus[index] = true;
            pressedThisFrame[index] = true;

            if (ButtonDown != null)
            {
                ButtonDown.Invoke(this, button);
            }
        }

        internal void fireButtonUp(GamepadButtonCode button)
        {
            int index = (int)button;

            //Make sure the button is down
            if (buttonDownStatus[index])
            {
                buttonDownStatus[index] = false;
                if (pressedThisFrame[index])
                {
                    downAndUpThisFrame[index] = true;
                }

                if (ButtonUp != null)
                {
                    ButtonUp.Invoke(this, button);
                }
            }
        }

        internal void fireMovement(Vector2 lStick, Vector2 rStick, float lTrigger, float rTrigger)
        {
            this.LStick = lStick;
            this.RStick = rStick;
            this.LTrigger = lTrigger;
            this.RTrigger = rTrigger;
        }

        public GamepadId Id { get; private set; }
    }
}

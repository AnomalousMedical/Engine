using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    public delegate void MouseButtonCallback(Mouse mouse, MouseButtonCode buttonCode);
    public delegate void MouseCallback(Mouse mouse);

    public class Mouse
    {
        private IntVector3 absMouse = new IntVector3(0, 0, 0);
        private IntVector3 relMouse = new IntVector3(0, 0, 0);
        private IntVector3 lastMouse = new IntVector3(0, 0, 0);
        private bool[] buttonDownStatus = new bool[(int)MouseButtonCode.NUM_BUTTONS]; //What we are currently reporting as down status
        private bool[] buttonDownProcessedStatus = new bool[(int)MouseButtonCode.NUM_BUTTONS]; //If the down status has been processed at least one frame

        //The actual live status of the button depending on what has been reported, this reports up so it does not have to be negated in the checkButtonUp
        //function
        private bool[] buttonUpActualStatus = new bool[(int)MouseButtonCode.NUM_BUTTONS];

        public Mouse()
        {
            for(int i = 0; i < buttonUpActualStatus.Length; ++i)
            {
                buttonUpActualStatus[i] = true;
            }
        }

        /// <summary>
        /// Called when a mouse button is pressed.
        /// </summary>
        public event MouseButtonCallback ButtonDown;

        /// <summary>
        /// Called when a mouse button is released.
        /// </summary>
        public event MouseButtonCallback ButtonUp;

        /// <summary>
        /// Called when the mouse moves in x or y direction. Or when the wheel is scrolled in the z direction.
        /// </summary>
        public event MouseCallback Moved;

        /// <summary>
        /// Determines if the specified button is pressed.
        /// </summary>
        /// <returns>True if the button is pressed.  False if it is not.</returns>
        public bool buttonDown(MouseButtonCode button)
        {
            return buttonDownStatus[(int)button];
        }

        internal void capture()
        {
            relMouse.x = absMouse.x - lastMouse.x;
            relMouse.y = absMouse.y - lastMouse.y;
            relMouse.z = absMouse.z - lastMouse.z;

            lastMouse = absMouse;
        }

        internal void postUpdate()
        {
            checkButtonUp(MouseButtonCode.MB_BUTTON0);
            checkButtonUp(MouseButtonCode.MB_BUTTON1);
            checkButtonUp(MouseButtonCode.MB_BUTTON2);
            checkButtonUp(MouseButtonCode.MB_BUTTON3);
            checkButtonUp(MouseButtonCode.MB_BUTTON4);
            checkButtonUp(MouseButtonCode.MB_BUTTON5);
            checkButtonUp(MouseButtonCode.MB_BUTTON6);
            checkButtonUp(MouseButtonCode.MB_BUTTON7);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void checkButtonUp(MouseButtonCode code)
        {
            int index = (int)code;
            if(buttonDownStatus[index] && buttonUpActualStatus[index])
            {
                buttonDownStatus[index] = false;
            }
            buttonDownProcessedStatus[index] = true;
        }

        /// <summary>
        /// Returns the absolute mouse position on the screen bounded by the mouse area
        /// and 0, 0.
        /// </summary>
        /// <returns>The position of the mouse.</returns>
        public IntVector3 AbsolutePosition
        {
            get
            {
                return absMouse;
            }
        }

        /// <summary>
        /// Returns the relative mouse position from the last time capture was called.
        /// </summary>
        /// <returns>The relative mouse position.</returns>
        public IntVector3 RelativePosition
        {
            get
            {
                return relMouse;
            }
        }

        /// <summary>
        /// Get the width that the mouse produces input for.
        /// </summary>
        public int AreaWidth { get; internal set; }

        /// <summary>
        /// Get the height that the mouse produces input for.
        /// </summary>
        public int AreaHeight { get; internal set; }

        public bool AnyButtonsDown
        {
            get
            {
              return !(buttonUpActualStatus[(int)MouseButtonCode.MB_BUTTON0]
                    && buttonUpActualStatus[(int)MouseButtonCode.MB_BUTTON1]
                    && buttonUpActualStatus[(int)MouseButtonCode.MB_BUTTON2]
                    && buttonUpActualStatus[(int)MouseButtonCode.MB_BUTTON3]
                    && buttonUpActualStatus[(int)MouseButtonCode.MB_BUTTON4]
                    && buttonUpActualStatus[(int)MouseButtonCode.MB_BUTTON5]
                    && buttonUpActualStatus[(int)MouseButtonCode.MB_BUTTON6]
                    && buttonUpActualStatus[(int)MouseButtonCode.MB_BUTTON7]);
            }
        }

        internal void fireButtonDown(MouseButtonCode button)
        {
            int index = (int)button;

            buttonDownStatus[index] = true;
            buttonDownProcessedStatus[index] = false;
            buttonUpActualStatus[index] = false;

            if (ButtonDown != null)
            {
                ButtonDown.Invoke(this, button);
            }
        }

        internal void fireButtonUp(MouseButtonCode button)
        {
            int index = (int)button;
            buttonUpActualStatus[index] = true;

            //Make sure the button is down
            if (buttonDownStatus[index])
            {
                //Before setting the button back to up make sure it has been processed at least once
                if (buttonDownProcessedStatus[index])
                {
                    buttonDownStatus[index] = false;
                }

                //Always fire the raw input
                if (ButtonUp != null)
                {
                    ButtonUp.Invoke(this, button);
                }
            }
        }

        internal void fireMoved(int x, int y)
        {
            absMouse.x = x;
            absMouse.y = y;

            if (Moved != null)
            {
                Moved.Invoke(this);
            }
        }

        internal void fireWheel(int z)
        {
            absMouse.z += z;

            if (Moved != null)
            {
                Moved.Invoke(this);
            }
        }

        internal static string PrettyButtonName(MouseButtonCode button)
        {
            switch (button)
            {
                case MouseButtonCode.MB_BUTTON0:
                    return "Left Click";
                case MouseButtonCode.MB_BUTTON1:
                    return "Right Click";
                case MouseButtonCode.MB_BUTTON2:
                    return "Mouse Button 2";
                case MouseButtonCode.MB_BUTTON3:
                    return "Mouse Button 3";
                case MouseButtonCode.MB_BUTTON4:
                    return "Mouse Button 4";
                case MouseButtonCode.MB_BUTTON5:
                    return "Mouse Button 5";
                case MouseButtonCode.MB_BUTTON6:
                    return "Mouse Button 6";
                case MouseButtonCode.MB_BUTTON7:
                    return "Mouse Button 7";
                default:
                    return "Unknown Mouse Button";
            }
        }
    }
}

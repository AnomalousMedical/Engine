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
        private bool[] buttonDownStatus = new bool[(int)MouseButtonCode.NUM_BUTTONS];
        private bool[] pressedThisFrame = new bool[(int)MouseButtonCode.NUM_BUTTONS];
        private bool[] downAndUpThisFrame = new bool[(int)MouseButtonCode.NUM_BUTTONS];

        public Mouse()
        {
            
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
        /// Note that this event is fired as the os updates and not as capture is called, it is not safe to use
        /// RelativePosition in handlers for this event.
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
        
        /// <summary>
        /// Determins if the button was pressed on this frame, will be true the first frame the
        /// button is down and false every frame after that until it is released. This is intended
        /// to catch down / up events that happen before an update can be called.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool buttonDownAndUpThisFrame(MouseButtonCode button)
        {
            return downAndUpThisFrame[(int)button];
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
            pressedThisFrame[(int)MouseButtonCode.MB_BUTTON0] = false;
            pressedThisFrame[(int)MouseButtonCode.MB_BUTTON1] = false;
            pressedThisFrame[(int)MouseButtonCode.MB_BUTTON2] = false;
            pressedThisFrame[(int)MouseButtonCode.MB_BUTTON3] = false;
            pressedThisFrame[(int)MouseButtonCode.MB_BUTTON4] = false;
            pressedThisFrame[(int)MouseButtonCode.MB_BUTTON5] = false;
            pressedThisFrame[(int)MouseButtonCode.MB_BUTTON6] = false;
            pressedThisFrame[(int)MouseButtonCode.MB_BUTTON7] = false;

            downAndUpThisFrame[(int)MouseButtonCode.MB_BUTTON0] = false;
            downAndUpThisFrame[(int)MouseButtonCode.MB_BUTTON1] = false;
            downAndUpThisFrame[(int)MouseButtonCode.MB_BUTTON2] = false;
            downAndUpThisFrame[(int)MouseButtonCode.MB_BUTTON3] = false;
            downAndUpThisFrame[(int)MouseButtonCode.MB_BUTTON4] = false;
            downAndUpThisFrame[(int)MouseButtonCode.MB_BUTTON5] = false;
            downAndUpThisFrame[(int)MouseButtonCode.MB_BUTTON6] = false;
            downAndUpThisFrame[(int)MouseButtonCode.MB_BUTTON7] = false;
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
                return   buttonDownStatus[(int)MouseButtonCode.MB_BUTTON0]
                      || buttonDownStatus[(int)MouseButtonCode.MB_BUTTON1]
                      || buttonDownStatus[(int)MouseButtonCode.MB_BUTTON2]
                      || buttonDownStatus[(int)MouseButtonCode.MB_BUTTON3]
                      || buttonDownStatus[(int)MouseButtonCode.MB_BUTTON4]
                      || buttonDownStatus[(int)MouseButtonCode.MB_BUTTON5]
                      || buttonDownStatus[(int)MouseButtonCode.MB_BUTTON6]
                      || buttonDownStatus[(int)MouseButtonCode.MB_BUTTON7];
            }
        }

        internal void fireButtonDown(MouseButtonCode button)
        {
            int index = (int)button;

            buttonDownStatus[index] = true;
            pressedThisFrame[index] = true;

            if (ButtonDown != null)
            {
                ButtonDown.Invoke(this, button);
            }
        }

        internal void fireButtonUp(MouseButtonCode button)
        {
            int index = (int)button;

            //Make sure the button is down
            if (buttonDownStatus[index])
            {
                buttonDownStatus[index] = false;
                if(pressedThisFrame[index])
                {
                    downAndUpThisFrame[index] = true;
                }

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

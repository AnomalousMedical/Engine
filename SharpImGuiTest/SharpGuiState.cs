using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    class SharpGuiState
    {
        private Guid EmptySpace = Guid.NewGuid(); //A guid for when the user clicks on empty space. This gets considered to be active

        public Guid MouseHoverItem { get; internal set; }
        public Guid ActiveItem { get; internal set; }
        public int MouseX { get; private set; }
        public int MouseY { get; private set; }
        public bool MouseDown { get; private set; }

        public void Begin(int mouseX, int mouseY, bool mouseDown, KeyboardButtonCode lastKeyPressed, bool isShift, bool isAlt, bool isCtrl)
        {
            KeyEntered = lastKeyPressed;
            MouseX = mouseX;
            MouseY = mouseY;
            MouseDown = mouseDown;
            MouseHoverItem = Guid.Empty;
            this.IsShift = isShift;
            this.IsAlt = isAlt;
            this.IsCtrl = isCtrl;
        }

        public void End()
        {
            if (MouseDown)
            {
                //This needs to say nested, above check is just mouse up / down
                //If ActiveItem is empty at the end of the frame, consider empty space to be clicked.
                if (ActiveItem == Guid.Empty)
                {
                    ActiveItem = EmptySpace;
                }
            }
            else
            {
                ActiveItem = Guid.Empty;
            }

            KeyEntered = KeyboardButtonCode.KC_UNASSIGNED;
        }

        public bool RegionHitByMouse(int x, int y, int w, int h)
        {
            return !(MouseX < x ||
                   MouseY < y ||
                   MouseX >= x + w ||
                   MouseY >= y + h);
        }

        /// <summary>
        /// Call this in every widget to try to get keyboard focus. Only changes if nothing has focus.
        /// </summary>
        /// <param name="id"></param>
        public void GrabKeyboardFocus(Guid id)
        {
            if (FocusedItem == Guid.Empty)
            {
                FocusedItem = id;
            }
        }

        /// <summary>
        /// Handle common keyboard focus code. This will return true if the calling code should further process
        /// input and false if it should do nothing more.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ProcessKeyboardFocus(Guid id)
        {
            bool callerHandlesInput = false;
            if (FocusedItem == id)
            {
                callerHandlesInput = true;
                //If tab is pressed, drop to allow next widget to pick it up.
                switch (KeyEntered)
                {
                    case KeyboardButtonCode.KC_TAB:
                        if (IsShift)
                        {
                            FocusedItem = LastWidget;
                        }
                        else
                        {
                            FocusedItem = Guid.Empty;
                        }
                        callerHandlesInput = false;
                        break;
                }

                //If input was handled by anything here, clear the current key so nothing else processes it.
                if (!callerHandlesInput)
                {
                    KeyEntered = KeyboardButtonCode.KC_UNASSIGNED;
                }
            }
            LastWidget = id;
            return callerHandlesInput;
        }

        public Guid FocusedItem { get; internal set; } //Focused

        public KeyboardButtonCode KeyEntered { get; private set; }

        public bool IsShift { get; set; }

        public bool IsAlt { get; set; }

        public bool IsCtrl { get; set; }

        public Guid LastWidget { get; set; }
    }
}

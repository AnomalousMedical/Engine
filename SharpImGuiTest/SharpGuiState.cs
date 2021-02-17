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

            //If nothing is focused at the end tab must have been hit on the last widget, loop back around to the first one in that case.
            if (KbdItem == Guid.Empty)
            {
                KbdItem = firstWidget;
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
            if (KbdItem == Guid.Empty)
            {
                KbdItem = id;
            }
        }

        /// <summary>
        /// Handle common keyboard focus code. This will return true if nothing was done and false if
        /// the input was handled. If this returns true the calling code should do its own input processing.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ProcessKeyboardFocus(Guid id)
        {
            bool processedNoInput = true;
            if (KbdItem == id)
            {
                //If tab is pressed, drop to allow next widget to pick it up.
                switch (KeyEntered)
                {
                    case KeyboardButtonCode.KC_TAB:
                        if (IsShift)
                        {
                            KbdItem = LastWidget;
                        }
                        else
                        {
                            KbdItem = Guid.Empty;
                        }
                        processedNoInput = false;
                        break;
                }
            }
            LastWidget = id;
            if (!processedNoInput) //If input was handled by anything clear the current key
            {
                KeyEntered = KeyboardButtonCode.KC_UNASSIGNED;
            }
            return processedNoInput;
        }

        public Guid KbdItem { get; internal set; } //Focused

        public KeyboardButtonCode KeyEntered { get; private set; }

        public bool IsShift { get; set; }

        public bool IsAlt { get; set; }

        public bool IsCtrl { get; set; }

        private Guid firstWidget = Guid.Empty;
        private Guid lastWidget = Guid.Empty;
        public Guid LastWidget
        {
            get
            {
                return lastWidget;
            }
            private set
            {
                lastWidget = value;
                if (firstWidget == Guid.Empty)
                {

                }
            }
        }
    }
}

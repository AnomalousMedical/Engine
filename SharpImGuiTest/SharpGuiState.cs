using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    class SharpGuiState
    {
        public Guid MouseHoverItem { get; internal set; }
        public Guid ActiveItem { get; internal set; }
        public int MouseX { get; private set; }
        public int MouseY { get; private set; }
        public bool MouseDown { get; private set; }

        public void SetMouseState(int x, int y, bool mouseDown)
        {
            MouseX = x;
            MouseY = y;
            MouseDown = mouseDown;
        }

        public bool RegionHit(int x, int y, int w, int h)
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
            if(KbdItem == Guid.Empty)
            {
                KbdItem = id;
            }
        }

        public Guid KbdItem { get; internal set; } //Focused
        public int KeyEntered { get; private set; }
        public int KeyMod { get; private set; }
        public Guid LastWidget { get; internal set; }
    }
}

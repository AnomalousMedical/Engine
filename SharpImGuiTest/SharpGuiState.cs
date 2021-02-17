using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    class SharpGuiState
    {
        public Guid FocusItem { get; internal set; }
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

        //Guid KbdItem; //Focused
        //int KeyEntered;
        //int KeyMod;

        //Guid LastWidget;
    }
}

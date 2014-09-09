using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    /// <summary>
    /// This class allows access to the state of a mouse.
    /// </summary>
    public abstract class MouseHardware
    {
        private Mouse mouse;

        public MouseHardware(Mouse mouse)
        {
            this.mouse = mouse;
        }

        protected void fireButtonDown(MouseButtonCode button)
        {
            mouse.fireButtonDown(button);
        }

        protected void fireButtonUp(MouseButtonCode button)
        {
            mouse.fireButtonUp(button);
        }

        protected void fireMoved(int x, int y)
        {
            mouse.fireMoved(x, y);
        }

        protected void fireWheel(int z)
        {
            mouse.fireWheel(z);
        }

        protected void fireSizeChanged(int width, int height)
        {
            mouse.AreaWidth = width;
            mouse.AreaHeight = height;
        }
    }
}

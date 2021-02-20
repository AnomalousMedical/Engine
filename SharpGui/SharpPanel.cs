using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGui
{
    public class SharpPanel
    {
        public SharpPanel()
        {
            
        }

        public void SetRect(int left, int top, int width, int height)
        {
            this.Rect.Left = left;
            this.Rect.Top = top;
            this.Rect.Width = width;
            this.Rect.Height = height;
        }

        public IntRect Rect;

        /// <summary>
        /// The desired size calculated during the last measurement of the panel. <seealso cref="PanelLayout"/>
        /// </summary>
        internal IntSize2 CalcDesiredSize;

        /// <summary>
        /// The int pad calculated during the last measurement of this panel. <seealso cref="PanelLayout"/>
        /// </summary>
        internal IntPad CalcIntPad;
    }
}

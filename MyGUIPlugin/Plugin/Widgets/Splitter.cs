using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    /// <summary>
    /// A helper class to split two widgets with a third splitter widget allowing resize.
    /// </summary>
    public class Splitter
    {
        private Widget splitterParent;
        private Widget splitterWidget;
        private Widget widget1;
        private Widget widget2;

        private IntVector2 dragLastPosition;
        private IntSize2 dragTotalSize;

        private int widget1Min = 0;
        private int widget2Min = 0;

        private bool horizontal = true;
        private float splitterPositionPercent = 0.0f;

        /// <summary>
        /// Called when the splitter moves from the user moving it. Does not get called
        /// when resized is called.
        /// </summary>
        public event Action<Splitter> Moved;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="splitterWidget">The widget to use as a splitter.</param>
        /// <param name="widget1">The first widget.</param>
        /// <param name="widget2">The second widget.</param>
        public Splitter(Widget splitterWidget, Widget widget1, Widget widget2)
        {
            this.splitterWidget = splitterWidget;
            splitterParent = splitterWidget.Parent;
            splitterPositionPercent = (float)splitterWidget.Left / splitterParent.Width;
            splitterWidget.MouseDrag += splitterWidget_MouseDrag;
            splitterWidget.MouseButtonPressed += splitterWidget_MouseButtonPressed;
            splitterWidget.Pointer = PointerManager.SIZE_HORZ;
            this.widget1 = widget1;
            this.widget2 = widget2;
        }

        public void resized()
        {
            if(horizontal)
            {
                splitterWidget.Left = (int)(splitterParent.Width * splitterPositionPercent);
                splitterMovedHorizontal();
            }
            else
            {

            }
        }

        private void splitterWidget_MouseDrag(Widget source, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            IntVector2 offset = me.Position - dragLastPosition;

            if (horizontal) //Horizontal
            {
                splitterWidget.Left += offset.x;
                splitterMovedHorizontal();
            }
            else
            {
                    
            }

            dragLastPosition = me.Position;
            splitterPositionPercent = (float)splitterWidget.Left / splitterParent.Width;

            if (Moved != null)
            {
                Moved.Invoke(this);
            }
        }

        private void splitterMovedHorizontal()
        {
            int widget1Width = splitterWidget.Left;
            if (widget1Width <= widget1Min)
            {
                widget1Width = widget1Min;
                splitterWidget.Left = widget1Min;
            }

            int widget2Width = splitterParent.Width - splitterWidget.Right;
            if (widget2Width < widget2Min)
            {
                widget2Width = widget2Min;
                splitterWidget.Right = splitterParent.Width - widget2Width;
                //Also reset the size for widget1 since we had to move the splitter
                widget1Width = splitterWidget.Left;
            }

            widget1.Width = widget1Width;
            widget2.Left = splitterWidget.Right;
            widget2.Width = widget2Width;
        }

        void splitterWidget_MouseButtonPressed(Widget source, EventArgs e)
        {
            dragLastPosition = ((MouseEventArgs)e).Position;
            dragTotalSize = widget1.Size + widget2.Size;
        }
    }
}

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

        private int widget1Min = 50;
        private int widget2Min = 50;
        private int lastSplitterPosition = int.MinValue;

        private bool horizontal = true;
        private float splitterPositionPercent = 0.0f;

        /// <summary>
        /// Called when the splitter moves. This could be because the user dragged the splitter,
        /// the <see cref="parentResized"/> function was called or the position percent was set programatically
        /// via <see cref="SplitterPosition"/>.
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
            splitterWidget.Pointer = horizontal ? PointerManager.SIZE_HORZ : PointerManager.SIZE_VERT;
            this.widget1 = widget1;
            this.widget2 = widget2;
        }

        /// <summary>
        /// Call this when the parent widget for this splitter resizes, this will move the splitter and
        /// its widgets to the correct new position and will fire <see cref="Moved"/>.
        /// </summary>
        public void parentResized()
        {
            if(horizontal)
            {
                splitterWidget.Left = (int)(splitterParent.Width * splitterPositionPercent);
                splitterMovedHorizontal();
            }
            else
            {

            }

            fireMoved();
        }

        /// <summary>
        /// Set the splitter position as a percentage of its parent widget. This will
        /// also update the splitter and call the <see cref="Moved"/> event.
        /// </summary>
        public float SplitterPosition
        {
            get
            {
                return splitterPositionPercent;
            }
            set
            {
                splitterPositionPercent = value;
                if (horizontal)
                {
                    splitterWidget.Left = (int)(splitterParent.Width * splitterPositionPercent);
                    splitterMovedHorizontal();
                }
                else
                {

                }
            }
        }

        private void splitterWidget_MouseDrag(Widget source, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            IntVector2 offset = me.Position - dragLastPosition;
            dragLastPosition = me.Position;

            if (horizontal) //Horizontal
            {
                splitterWidget.Left += offset.x;
                splitterMovedHorizontal();
            }
            else
            {
                    
            }

            if (splitterParent.Width != 0)
            {
                splitterPositionPercent = (float)splitterWidget.Left / splitterParent.Width;
            }
            else
            {
                splitterPositionPercent = .5f;
            }

            fireMoved();
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
                splitterWidget.Left = splitterParent.Width - widget2Width - splitterWidget.Width;
                //Also reset the size for widget1 since we had to move the splitter
                widget1Width = splitterWidget.Left;
                //Critical condition where both are too small, in this case split up the visible area
                if(widget1Width < widget1Min)
                {
                    widget1Width = widget2Width = splitterParent.Width / 2 - splitterWidget.Width;
                    if(widget1Width < 0)
                    {
                        widget1Width = 0;
                        widget2Width = 0;
                    }
                    splitterWidget.Left = widget1Width;
                }
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

        private void fireMoved()
        {
            int currentPosition = horizontal ? splitterWidget.Left : splitterWidget.Top;
            if (currentPosition != lastSplitterPosition)
            {
                lastSplitterPosition = currentPosition;
                if (Moved != null)
                {
                    Moved.Invoke(this);
                }
            }
        }
    }
}

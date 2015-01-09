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

        private int widget1Min = ScaleHelper.Scaled(50);
        private int widget2Min = ScaleHelper.Scaled(50);
        private IntSize2 lastWidget1Size = IntSize2.MinValue;
        private IntSize2 lastWidget2Size = IntSize2.MinValue;

        private bool horizontal = true;
        private float splitterPositionPercent = 0.0f;

        /// <summary>
        /// Called when widget 1 is resized. This could be because the user dragged the splitter,
        /// the <see cref="layout"/> function was called or the position percent was set programatically
        /// via <see cref="SplitterPosition"/>. Note that this size change covers both width and height
        /// no matter what the type of splitter this is.
        /// </summary>
        public event Action<Splitter> Widget1Resized;

        /// <summary>
        /// Called when widget 2 is resized. This could be because the user dragged the splitter,
        /// the <see cref="layout"/> function was called or the position percent was set programatically
        /// via <see cref="SplitterPosition"/>. Note that this size change covers both width and height
        /// no matter what the type of splitter this is.
        /// </summary>
        public event Action<Splitter> Widget2Resized;

        /// <summary>
        /// Constructor that will lookup the properties of the splitter from User Strings on the passed Widget.
        /// Whether this is horizontal or vertical will depend on the dimensions of the passed widget. Width > Height
        /// means vertical and Height > Width means horizontal.
        /// <para>You must set several user strings on the passed widget.</para>
        /// <para>Widget1Name - The name of the first widget. This is the top or left widget.</para>
        /// <para>Widget2Name - The name of the second widget. This is the right or bottom widget.</para>
        /// </summary>
        /// <param name="splitterWidget">The splitter widget to use.</param>
        public Splitter(Widget splitterWidget)
        {
            Widget splitterParent = splitterWidget.Parent;
            String widget1Name = splitterWidget.getUserString("Widget1Name", () => { throw new MyGUIException("You must define a Widget1Name user string."); });
            String widget2Name = splitterWidget.getUserString("Widget2Name", () => { throw new MyGUIException("You must define a Widget2Name user string."); });
            setup(splitterWidget, splitterParent, splitterParent.findWidget(widget1Name), splitterParent.findWidget(widget2Name), splitterWidget.Height > splitterWidget.Width);
        }

        /// <summary>
        /// Constructor allows you to pass all widgets manually.
        /// </summary>
        /// <param name="splitterWidget">The widget to use as a splitter.</param>
        /// <param name="widget1">The first widget.</param>
        /// <param name="widget2">The second widget.</param>
        /// <param name="horizontal">True to be a horizontal splitter and false to be vertical.</param>
        public Splitter(Widget splitterWidget, Widget widget1, Widget widget2, bool horizontal)
        {
            setup(splitterWidget, splitterWidget.Parent, widget1, widget2, horizontal);
        }

        private void setup(Widget splitterWidget, Widget splitterParent, Widget widget1, Widget widget2, bool horizontal)
        {
            this.splitterWidget = splitterWidget;
            this.splitterParent = splitterParent;
            this.horizontal = horizontal;
            splitterPositionPercent = horizontal ? (float)splitterWidget.Left / splitterParent.Width : (float)splitterWidget.Top / splitterParent.Height;
            splitterWidget.MouseDrag += splitterWidget_MouseDrag;
            splitterWidget.MouseButtonPressed += splitterWidget_MouseButtonPressed;
            splitterWidget.Pointer = horizontal ? PointerManager.SIZE_HORZ : PointerManager.SIZE_VERT;
            this.widget1 = widget1;
            this.widget2 = widget2;
        }

        /// <summary>
        /// Call this to layout the splitter and widgets. It will fire <see cref="Widget1Resized"/> and <see cref="Widget1Resized"/> as appropriate.
        /// </summary>
        public void layout()
        {
            if(horizontal)
            {
                splitterWidget.Left = (int)(splitterParent.Width * splitterPositionPercent);
                splitterMovedHorizontal();
            }
            else
            {
                splitterWidget.Top = (int)(splitterParent.Height * splitterPositionPercent);
                splitterMovedVertical();
            }

            fireMoved();
        }

        /// <summary>
        /// Set the splitter position as a percentage of its parent widget, this will move the splitter and
        /// its widgets to the correct new position and will fire <see cref="Widget1Resized"/> and <see cref="Widget1Resized"/> as appropriate.
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
                    splitterWidget.Top = (int)(splitterParent.Height * splitterPositionPercent);
                    splitterMovedVertical();
                }

                fireMoved();
            }
        }

        /// <summary>
        /// The minimum size for Widget 1. Setting this will not adjust the widgets, you must call <see cref="layout"/>
        /// to update the widgets.
        /// </summary>
        public int Widget1Min
        {
            get
            {
                return widget1Min;
            }
            set
            {
                widget1Min = value;
            }
        }

        /// <summary>
        /// The minimum size for Widget 2. Setting this will not adjust the widgets, you must call <see cref="layout"/>
        /// to update the widgets.
        /// </summary>
        public int Widget2Min
        {
            get
            {
                return widget2Min;
            }
            set
            {
                widget2Min = value;
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

                if (splitterParent.Width != 0)
                {
                    splitterPositionPercent = (float)splitterWidget.Left / splitterParent.Width;
                }
                else
                {
                    splitterPositionPercent = .5f;
                }
            }
            else
            {
                splitterWidget.Top += offset.y;
                splitterMovedVertical();

                if (splitterParent.Height != 0)
                {
                    splitterPositionPercent = (float)splitterWidget.Top / splitterParent.Height;
                }
                else
                {
                    splitterPositionPercent = .5f;
                }
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

        private void splitterMovedVertical()
        {
            int widget1Height = splitterWidget.Top;
            if (widget1Height <= widget1Min)
            {
                widget1Height = widget1Min;
                splitterWidget.Top = widget1Min;
            }

            int widget2Height = splitterParent.Height - splitterWidget.Bottom;
            if (widget2Height < widget2Min)
            {
                widget2Height = widget2Min;
                splitterWidget.Top = splitterParent.Height - widget2Height - splitterWidget.Height;
                //Also reset the size for widget1 since we had to move the splitter
                widget1Height = splitterWidget.Top;
                //Critical condition where both are too small, in this case split up the visible area
                if (widget1Height < widget1Min)
                {
                    widget1Height = widget2Height = splitterParent.Height / 2 - splitterWidget.Height;
                    if (widget1Height < 0)
                    {
                        widget1Height = 0;
                        widget2Height = 0;
                    }
                    splitterWidget.Top = widget1Height;
                }
            }

            widget1.Height = widget1Height;
            widget2.Top = splitterWidget.Bottom;
            widget2.Height = widget2Height;
        }

        void splitterWidget_MouseButtonPressed(Widget source, EventArgs e)
        {
            dragLastPosition = ((MouseEventArgs)e).Position;
            dragTotalSize = widget1.Size + widget2.Size;
        }

        private void fireMoved()
        {
            IntSize2 currentSize = horizontal ? widget1.Size : widget1.Size;
            if (currentSize != lastWidget1Size)
            {
                lastWidget1Size = currentSize;
                if (Widget1Resized != null)
                {
                    Widget1Resized.Invoke(this);
                }
            }

            currentSize = horizontal ? widget2.Size : widget2.Size;
            if (currentSize != lastWidget2Size)
            {
                lastWidget2Size = currentSize;
                if (Widget2Resized != null)
                {
                    Widget2Resized.Invoke(this);
                }
            }
        }
    }
}

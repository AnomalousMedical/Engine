using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    class TimelineSelectionBox
    {
        private Widget timelineSelectionWidget;
        private IntVector2 mouseDownPosition;
        private ScrollView scrollView;

        public event EventDelegate<TimelineSelectionBox> SelectionAreaDefined;

        public TimelineSelectionBox(ScrollView scrollView)
        {
            this.scrollView = scrollView;

            timelineSelectionWidget = scrollView.createWidgetT("Widget", "TimelineSelectionRegion", 0, 0, 1, 1, Align.Left | Align.Top, "");
            timelineSelectionWidget.Visible = false;

            scrollView.ClientWidget.MouseButtonPressed += new MyGUIEvent(scrollView_MouseButtonPressed);
            scrollView.ClientWidget.MouseButtonReleased += new MyGUIEvent(scrollView_MouseButtonReleased);
            scrollView.ClientWidget.MouseDrag += new MyGUIEvent(scrollView_MouseDrag);
        }

        public bool intersects(TimelineViewButton timelineButton)
        {
            int thisLeft = timelineSelectionWidget.Left;
            int thisRight = thisLeft + timelineSelectionWidget.Width;
            int thisTop = timelineSelectionWidget.Top;
            int thisBottom = thisTop + timelineSelectionWidget.Height;

            int otherLeft = timelineButton.Left;
            int otherRight = otherLeft + timelineButton.Width;
            int otherTop = timelineButton.Top;
            int otherBottom = otherRight + timelineButton.Height;

            if (thisBottom < otherTop)
            {
                return false;
            }
            if (thisTop > otherBottom)
            {
                return false;
            }

            if (thisRight < otherLeft)
            {
                return false;
            }
            if (thisLeft > otherRight)
            {
                return false;
            }

            return true;
        }

        void scrollView_MouseDrag(Widget source, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            IntVector2 currentMousePosition = me.Position;
            currentMousePosition.x -= scrollView.ClientWidget.AbsoluteLeft;
            currentMousePosition.y -= scrollView.ClientWidget.AbsoluteTop;

            int left, top, width, height;
            if (currentMousePosition.x > mouseDownPosition.x)
            {
                left = mouseDownPosition.x;
                width = currentMousePosition.x - mouseDownPosition.x;
            }
            else
            {
                left = currentMousePosition.x;
                width = mouseDownPosition.x - currentMousePosition.x;
            }
            if (currentMousePosition.y > mouseDownPosition.y)
            {
                top = mouseDownPosition.y;
                height = currentMousePosition.y - mouseDownPosition.y;
            }
            else
            {
                top = currentMousePosition.y;
                height = mouseDownPosition.y - currentMousePosition.y;
            }
            timelineSelectionWidget.setPosition(left, top);
            timelineSelectionWidget.setSize(width, height);
        }

        void scrollView_MouseButtonReleased(Widget source, EventArgs e)
        {
            if (SelectionAreaDefined != null)
            {
                SelectionAreaDefined.Invoke(this);
            }
            timelineSelectionWidget.Visible = false;
        }

        void scrollView_MouseButtonPressed(Widget source, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            mouseDownPosition = me.Position;
            mouseDownPosition.x -= scrollView.ClientWidget.AbsoluteLeft;
            mouseDownPosition.y -= scrollView.ClientWidget.AbsoluteTop;
            timelineSelectionWidget.Visible = true;
            timelineSelectionWidget.setPosition(mouseDownPosition.x, mouseDownPosition.y);
            timelineSelectionWidget.setSize(0, 0);
        }
    }
}

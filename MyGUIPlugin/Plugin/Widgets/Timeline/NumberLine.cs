using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using Engine;

namespace MyGUIPlugin
{
    public class NumberLine
    {
        private ScrollView numberlineScroller;
        private int pixelsPerSecond;
        private float numberSeparationDuration = 1.0f;
        private TimelineView timelineView;

        private List<NumberLineNumber> activeNumbers = new List<NumberLineNumber>();
        private List<NumberLineNumber> inactiveNumbers = new List<NumberLineNumber>();

        public NumberLine(ScrollView numberlineScroller, TimelineView timelineView)
        {
            this.numberlineScroller = numberlineScroller;
            this.timelineView = timelineView;
            pixelsPerSecond = timelineView.PixelsPerSecond;
            timelineView.CanvasPositionChanged += new CanvasPositionChanged(actionView_CanvasPositionChanged);
            timelineView.CanvasWidthChanged += new CanvasSizeChanged(actionView_CanvasWidthChanged);
            timelineView.PixelsPerSecondChanged += new EventHandler(actionView_PixelsPerSecondChanged);
            numberlineScroller.ClientWidget.MouseButtonReleased += new MyGUIEvent(ClientWidget_MouseButtonReleased);
            canvasModified();
        }

        public int PixelsPerSecond
        {
            get
            {
                return pixelsPerSecond;
            }
            set
            {
                pixelsPerSecond = value;
                if (pixelsPerSecond == 1)
                {
                    numberSeparationDuration = 40.0f;
                }
                else if (pixelsPerSecond <= 3)
                {
                    numberSeparationDuration = 20.0f;
                }
                else if (pixelsPerSecond <= 7)
                {
                    numberSeparationDuration = 10.0f;
                }
                else if (pixelsPerSecond <= 10)
                {
                    numberSeparationDuration = 5.0f;
                }
                else if (pixelsPerSecond <= 20)
                {
                    numberSeparationDuration = 3.0f;
                }
                else if (pixelsPerSecond <= 40)
                {
                    numberSeparationDuration = 2.0f;
                }
                else
                {
                    numberSeparationDuration = 1.0f;
                }
                foreach(NumberLineNumber number in activeNumbers)
                {
                    returnNumberToPool(number);
                }
                activeNumbers.Clear();
                canvasModified();
            }
        }

        internal void moveMarker(float time)
        {
            timelineView.MarkerTime = time;
        }

        void ClientWidget_MouseButtonReleased(Widget source, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            moveMarker((me.Position.x - numberlineScroller.AbsoluteLeft + numberlineScroller.CanvasPosition.x) / PixelsPerSecond);
        }

        void actionView_CanvasPositionChanged(CanvasEventArgs info)
        {
            numberlineScroller.CanvasPosition = new Vector2(-info.Left, 0.0f);
            canvasModified();
        }

        void actionView_PixelsPerSecondChanged(object sender, EventArgs e)
        {
            TimelineView actionView = (TimelineView)sender;
            this.PixelsPerSecond = actionView.PixelsPerSecond;
        }

        void actionView_CanvasWidthChanged(float newSize)
        {
            numberlineScroller.CanvasSize = new Size2(newSize > numberlineScroller.Width ? newSize : numberlineScroller.Width, numberlineScroller.Height);
            canvasModified();
        }

        void canvasModified()
        {
            float leftSide = numberlineScroller.CanvasPosition.x - pixelsPerSecond * numberSeparationDuration;
            float rightSide = leftSide + numberlineScroller.ClientCoord.width + pixelsPerSecond * numberSeparationDuration;

            //Remove inactive numbers
            for (int i = 0; i < activeNumbers.Count; ++i)
            {
                NumberLineNumber number = activeNumbers[i];
                if (number.Right < leftSide)
                {
                    activeNumbers.RemoveAt(i--);
                    returnNumberToPool(number);
                }
                if (number.Left > rightSide)
                {
                    activeNumbers.RemoveAt(i--);
                    returnNumberToPool(number);
                }
            }

            //If there are any active numbers
            if (activeNumbers.Count > 0)
            {
                NumberLineNumber number = null;

                //See if any numbers need to be added to the front of the list
                float startingPoint = activeNumbers[0].Time - numberSeparationDuration;
                for (float i = startingPoint; i * pixelsPerSecond > leftSide; i -= numberSeparationDuration)
                {
                    number = getPooledNumber();
                    number.Time = i;
                    activeNumbers.Insert(0, number);
                }

                //Add numbers to the back of the list
                number = null;
                startingPoint = activeNumbers[activeNumbers.Count - 1].Time + numberSeparationDuration;
                for (float i = startingPoint; i <= timelineView.Duration && i * pixelsPerSecond < rightSide; i += numberSeparationDuration)
                {
                    number = getPooledNumber();
                    number.Time = i;
                    activeNumbers.Add(number);
                }
                if (number != null)
                {
                    Size2 canvasSize = numberlineScroller.CanvasSize;
                    if (number.Right > canvasSize.Width)
                    {
                        canvasSize.Width = number.Right;
                        numberlineScroller.CanvasSize = canvasSize;
                    }
                }
            }
            //If there are currently no active numbers
            else
            {
                //Compute the starting point on the next even start point.
                float startingPoint = leftSide / pixelsPerSecond;
                int numTicks = (int)(startingPoint / numberSeparationDuration);
                startingPoint = numTicks * numberSeparationDuration;
                NumberLineNumber number = null;
                for (float i = startingPoint; i <= timelineView.Duration && i * pixelsPerSecond < rightSide; i += numberSeparationDuration)
                {
                    number = getPooledNumber();
                    number.Time = i;
                    activeNumbers.Add(number);
                }
                Vector2 scrollPos = numberlineScroller.CanvasPosition;
                numberlineScroller.CanvasSize = new Size2(number.Right, numberlineScroller.Height);
                numberlineScroller.CanvasPosition = scrollPos; //Have to set the position back cause sometimes it changes.
            }
        }

        private NumberLineNumber getPooledNumber()
        {
            NumberLineNumber number;
            if (inactiveNumbers.Count > 0)
            {
                number = inactiveNumbers[0];
                inactiveNumbers.RemoveAt(0);
                number.Visible = true;
            }
            else
            {
                number = new NumberLineNumber(numberlineScroller.createWidgetT("StaticText", "StaticText", 0, 0, 10, 15, Align.Left | Align.Top, "") as StaticText,
                    numberlineScroller.createWidgetT("Widget", "Separator1", 0, 16, 1, numberlineScroller.Height - 16, Align.Left | Align.Top, ""), this);
            }
            return number;
        }

        private void returnNumberToPool(NumberLineNumber number)
        {
            number.Visible = false;
            inactiveNumbers.Add(number);
        }
    }
}

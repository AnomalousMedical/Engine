using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;

namespace MyGUIPlugin
{
    class TimelineMarker : IDisposable
    {
        private Widget widget;
        private int pixelsPerSecond;
        private float dragStartPos;
        private float dragStartTime;
        private float time = 0.0f;
        private TimelineView timelineView;

        public event EventHandler CoordChanged;

        public TimelineMarker(TimelineView timelineView, ScrollView scrollView)
        {
            this.widget = scrollView.createWidgetT("Widget", "TimelineSeparator", 1, 0, 2, (int)scrollView.CanvasSize.Height, Align.Left | Align.Top, "");
            widget.Pointer = "size_horz";
            widget.MouseDrag += new MyGUIEvent(widget_MouseDrag);
            widget.MouseButtonPressed += new MyGUIEvent(widget_MouseButtonPressed);

            this.timelineView = timelineView;
            pixelsPerSecond = timelineView.PixelsPerSecond;
            timelineView.CanvasHeightChanged += new CanvasSizeChanged(timelineView_CanvasHeightChanged);
            timelineView.PixelsPerSecondChanged += new EventHandler(timelineView_PixelsPerSecondChanged);
        }

        public void Dispose()
        {
            Gui.Instance.destroyWidget(widget);
        }

        public float Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                if (time < 0)
                {
                    time = 0;
                }
                else if (time > timelineView.Duration)
                {
                    time = timelineView.Duration;
                }
                int physicalPosition = (int)(time * pixelsPerSecond);
                widget.setPosition(physicalPosition, widget.Top);
                if (CoordChanged != null)
                {
                    CoordChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public int Left
        {
            get
            {
                return widget.Left;
            }
        }

        public int Right
        {
            get
            {
                return widget.Right;
            }
        }

        public int Width
        {
            get
            {
                return widget.Width;
            }
        }

        void widget_MouseButtonPressed(Widget source, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            if (me.Button == Engine.Platform.MouseButtonCode.MB_BUTTON0)
            {
                dragStartPos = me.Position.x;
                dragStartTime = Time;
            }
        }

        void widget_MouseDrag(Widget source, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            float newTime = dragStartTime + (me.Position.x - dragStartPos) / pixelsPerSecond;
            if (newTime < 0.0f)
            {
                newTime = 0.0f;
            }
            Time = newTime;
        }

        void timelineView_CanvasHeightChanged(float newSize)
        {
            widget.setSize(widget.Width, (int)newSize);
        }

        void timelineView_PixelsPerSecondChanged(object sender, EventArgs e)
        {
            this.pixelsPerSecond = ((TimelineView)sender).PixelsPerSecond;
            Time = time;
        }
    }
}

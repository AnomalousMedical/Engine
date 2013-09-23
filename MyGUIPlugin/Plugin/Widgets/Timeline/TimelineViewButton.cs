using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using System.ComponentModel;
using Engine;

namespace MyGUIPlugin
{
    class TimelineViewButtonEventArgs : EventArgs
    {
        public int OldLeft { get; private set; }
        public int OldRight { get; private set; }
        public int OldTop { get; private set; }
        public int OldBottom { get; private set; }

        internal void _setValues(Button button)
        {
            OldLeft = button.Left;
            OldTop = button.Top;
            OldRight = button.Right;
            OldBottom = button.Bottom;
        }
    }

    class TimelineViewButton
    {
        private static readonly int MinButtonSize = ScaleHelper.Scaled(17);
        private static readonly int DurationButtonWidth = ScaleHelper.Scaled(3);

        private Widget colorArea;
        private Button button;
        private Button durationButton;
        private Button startTimeButton;
        private int pixelsPerSecond;
        private float dragStartPos;
        private float dragStartTime;
        private float durationStartTime;
        private float timelineDuration;
        private TimelineData timelineData;

        public event EventDelegate<TimelineViewButton, MouseEventArgs> Clicked;
        public event EventDelegate<TimelineViewButton, TimelineViewButtonEventArgs> CoordChanged;
        public event EventDelegate<TimelineViewButton, float> ButtonDragged;

        private static TimelineViewButtonEventArgs sharedEventArgs = new TimelineViewButtonEventArgs();

        public TimelineViewButton(int pixelsPerSecond, float timelineDuration, Button button, TimelineData timelineData)
        {
            colorArea = button.createWidgetT("Button", "TimelineButtonCenterSkin", 1, 1, button.Width - 2, button.Height - 2, Align.Stretch, "ColorArea");

            this.pixelsPerSecond = pixelsPerSecond;
            this.button = button;

            if (button.Width < MinButtonSize)
            {
                button.setSize(MinButtonSize, button.Height);
            }

            durationButton = button.createWidgetT("Button", "TimelineButton", button.Width - DurationButtonWidth, button.Top, DurationButtonWidth, button.Height, Align.Top | Align.Right, "") as Button;
            durationButton.MouseDrag += new MyGUIEvent(durationButton_MouseDrag);
            durationButton.MouseButtonPressed += new MyGUIEvent(durationButton_MouseButtonPressed);
            durationButton.Pointer = "size_horz";

            startTimeButton = button.createWidgetT("Button", "TimelineButton", 0, button.Top, DurationButtonWidth, button.Height, Align.Top | Align.Left, "") as Button;
            startTimeButton.MouseDrag += new MyGUIEvent(startTimeButton_MouseDrag);
            startTimeButton.MouseButtonPressed += new MyGUIEvent(startTimeButton_MouseButtonPressed);
            startTimeButton.Pointer = "size_horz";
            
            this.timelineData = timelineData;
            this.timelineDuration = timelineDuration;
            timelineData._CurrentButton = this;
            button.MouseDrag += new MyGUIEvent(button_MouseDrag);
            button.MouseButtonPressed += new MyGUIEvent(button_MouseButtonPressed);
        }

        /// <summary>
        /// Move the top of the button, should only be called by ActionViewRow.
        /// This will not fire the coordChanged event.
        /// </summary>
        /// <param name="top"></param>
        internal void _moveTop(int top)
        {
            button.setPosition(button.Left, top);
        }

        void button_MouseButtonPressed(Widget source, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            if (me.Button == Engine.Platform.MouseButtonCode.MB_BUTTON0)
            {
                if (Clicked != null)
                {
                    Clicked.Invoke(this, me);
                }

                dragStartPos = me.Position.x;
                dragStartTime = StartTime;
            }
        }

        void button_MouseDrag(Widget source, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            float newStartTime = dragStartTime + (me.Position.x - dragStartPos) / pixelsPerSecond;
            if (newStartTime + Duration > timelineDuration)
            {
                newStartTime = timelineDuration - Duration;
            }
            if (newStartTime < 0.0f)
            {
                newStartTime = 0.0f;
            }
            float oldStartTime = StartTime;
            StartTime = newStartTime;
            if (ButtonDragged != null)
            {
                ButtonDragged.Invoke(this, oldStartTime);
            }
        }

        void durationButton_MouseButtonPressed(Widget source, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            if (me.Button == Engine.Platform.MouseButtonCode.MB_BUTTON0)
            {
                dragStartPos = me.Position.x;
                durationStartTime = Duration;
            }
        }

        void durationButton_MouseDrag(Widget source, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            float newDuration = durationStartTime + (me.Position.x - dragStartPos) / pixelsPerSecond;
            if (newDuration < 0.0f)
            {
                newDuration = 0.0f;
            }
            if (newDuration + StartTime > timelineDuration)
            {
                newDuration = timelineDuration - Duration;
            }
            Duration = newDuration;
        }

        void startTimeButton_MouseButtonPressed(Widget source, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            if (me.Button == Engine.Platform.MouseButtonCode.MB_BUTTON0)
            {
                dragStartPos = me.Position.x;
                dragStartTime = StartTime;
                durationStartTime = Duration;
            }
        }

        void startTimeButton_MouseDrag(Widget source, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            float delta = (me.Position.x - dragStartPos) / pixelsPerSecond;
            float newStartTime = dragStartTime + delta;
            float newDuration = durationStartTime - delta;
            if (newStartTime + newDuration > timelineDuration)
            {
                newDuration = timelineDuration - newStartTime;
            }
            if (newStartTime < 0.0f)
            {
                newStartTime = 0.0f;
            }
            StartTime = newStartTime;
            Duration = newDuration;
        }

        public void Dispose()
        {
            Gui.Instance.destroyWidget(button);
        }

        public bool StateCheck
        {
            get
            {
                return button.Selected;
            }
            set
            {
                button.Selected = value;
            }
        }

        public float StartTime
        {
            get
            {
                return timelineData.StartTime;
            }
            set
            {
                timelineData.StartTime = value;
            }
        }

        public float Duration
        {
            get
            {
                return timelineData.Duration;
            }
            set
            {
                timelineData.Duration = value;
            }
        }

        public int Top
        {
            get
            {
                return button.Top;
            }
            set
            {
                button.setPosition(button.Left, value);
            }
        }

        public int Left
        {
            get
            {
                return button.Left;
            }
        }

        public int Right
        {
            get
            {
                return button.Right;
            }
        }

        public int Width
        {
            get
            {
                return button.Width;
            }
        }

        public int Height
        {
            get
            {
                return button.Height;
            }
        }

        public int Bottom
        {
            get
            {
                return button.Bottom;
            }
        }

        public TimelineData Data
        {
            get
            {
                return timelineData;
            }
        }

        public void setColor(Color color)
        {
            colorArea.setColour(color);
        }

        internal void changePixelsPerSecond(int pixelsPerSecond)
        {
            this.pixelsPerSecond = pixelsPerSecond;
            updatePosition();
            updateDurationWidth();
        }

        internal void changeDuration(float duration)
        {
            this.timelineDuration = duration;
            if (StartTime + Duration > timelineDuration)
            {
                //Figure out where the button should move to if it is longer than the duration
                float durationTime = timelineDuration - Duration;
                if (durationTime < 0)
                {
                    StartTime = 0;
                    Duration = timelineDuration;
                }
                else
                {
                    StartTime = timelineDuration - Duration;
                }
            }
        }

        internal void updatePosition()
        {
            sharedEventArgs._setValues(button);
            button.setPosition((int)(timelineData.StartTime * pixelsPerSecond), button.Top);
            if (CoordChanged != null)
            {
                CoordChanged.Invoke(this, sharedEventArgs);
            }
        }

        internal void updateDurationWidth()
        {
            sharedEventArgs._setValues(button);
            int buttonWidth = (int)(timelineData.Duration * pixelsPerSecond);
            if (buttonWidth < MinButtonSize)
            {
                buttonWidth = MinButtonSize;
            }
            button.setSize(buttonWidth, button.Height);
            if (CoordChanged != null)
            {
                CoordChanged.Invoke(this, sharedEventArgs);
            }
        }
    }
}

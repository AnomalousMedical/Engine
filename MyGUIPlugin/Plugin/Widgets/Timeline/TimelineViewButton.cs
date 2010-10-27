﻿using System;
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
        private Button button;
        private int pixelsPerSecond;
        private float dragStartPos;
        private float dragStartTime;
        private float timelineDuration;
        private TimelineData timelineData;

        public event EventHandler Clicked;
        public event EventHandler CoordChanged;

        private static TimelineViewButtonEventArgs sharedEventArgs = new TimelineViewButtonEventArgs();

        public TimelineViewButton(int pixelsPerSecond, float timelineDuration, Button button, TimelineData timelineData)
        {
            this.pixelsPerSecond = pixelsPerSecond;
            this.button = button;
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
                    Clicked.Invoke(this, e);
                }

                dragStartPos = me.Position.x;
                dragStartTime = StartTime;
            }
        }

        void button_MouseDrag(Widget source, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            float newStartTime = dragStartTime + (me.Position.x - dragStartPos) / pixelsPerSecond;
            if(newStartTime < 0.0f)
            {
                newStartTime = 0.0f;
            }
            if (newStartTime + Duration > timelineDuration)
            {
                newStartTime = timelineDuration - Duration;
            }
            StartTime = newStartTime;
        }

        public void Dispose()
        {
            Gui.Instance.destroyWidget(button);
        }

        public bool StateCheck
        {
            get
            {
                return button.StateCheck;
            }
            set
            {
                button.StateCheck = value;
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
            button.setColour(color);
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
            if (buttonWidth < 10)
            {
                buttonWidth = 10;
            }
            button.setSize(buttonWidth, button.Height);
            if (CoordChanged != null)
            {
                CoordChanged.Invoke(this, sharedEventArgs);
            }
        }
    }
}

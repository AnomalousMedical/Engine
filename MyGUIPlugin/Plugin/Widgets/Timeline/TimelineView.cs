using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using Engine;

namespace MyGUIPlugin
{
    public delegate void TimelineTrackEvent(TimelineViewTrack track);

    public class TimelineView : IDisposable
    {
        private const int PREVIEW_PADDING = 10;
        private const int TRACK_START_Y = 3;

        private TimelineScrollView timelineScrollView;
        private int pixelsPerSecond = 100;
        private Dictionary<String, int> rowIndexes = new Dictionary<string, int>();
        private List<TimelineViewTrack> tracks = new List<TimelineViewTrack>();
        private TimelineViewButton currentButton;
        private TimelineMarker timelineMarker;
        private int trackY = TRACK_START_Y;

        public event EventHandler ActiveActionChanged;
        public event TimelineTrackEvent TrackPositionChanged;
        public event TimelineTrackEvent TrackAdded;
        public event EventHandler PixelsPerSecondChanged;
        public event CanvasSizeChanged CanvasWidthChanged
        {
            add
            {
                timelineScrollView.CanvasWidthChanged += value;
            }
            remove
            {
                timelineScrollView.CanvasWidthChanged -= value;
            }
        }

        public event CanvasSizeChanged CanvasHeightChanged
        {
            add
            {
                timelineScrollView.CanvasHeightChanged += value;
            }
            remove
            {
                timelineScrollView.CanvasHeightChanged -= value;
            }
        }

        public event CanvasPositionChanged CanvasPositionChanged
        {
            add
            {
                timelineScrollView.CanvasPositionChanged += value;
            }
            remove
            {
                timelineScrollView.CanvasPositionChanged -= value;
            }
        }

        public TimelineView(ScrollView scrollView)
        {
            timelineScrollView = new TimelineScrollView(scrollView);
            scrollView.MouseLostFocus += new MyGUIEvent(scrollView_MouseLostFocus);
            scrollView.MouseWheel += new MyGUIEvent(scrollView_MouseWheel);
            scrollView.KeyButtonPressed += new MyGUIEvent(scrollView_KeyButtonPressed);
            scrollView.KeyButtonReleased += new MyGUIEvent(scrollView_KeyButtonReleased);
            timelineMarker = new TimelineMarker(this, scrollView);
            timelineMarker.CoordChanged += new EventHandler(timelineMarker_CoordChanged);
        }

        public void Dispose()
        {
            foreach (TimelineViewTrack row in tracks)
            {
                row.Dispose();
            }
            timelineMarker.Dispose();
        }

        public void addTrack(String name, Color color)
        {
            TimelineViewTrack track = new TimelineViewTrack(name, trackY, pixelsPerSecond, color);
            track.BottomChanged += new EventHandler(actionViewRow_BottomChanged);
            tracks.Add(track);
            rowIndexes.Add(name, tracks.Count - 1);
            trackY = track.Bottom;
            timelineScrollView.CanvasHeight = trackY;
            if (TrackAdded != null)
            {
                TrackAdded.Invoke(track);
            }
        }

        public void addData(TimelineData data)
        {
            Button button = timelineScrollView.createButton(pixelsPerSecond * data.StartTime, pixelsPerSecond * data.Duration);
            TimelineViewButton actionButton = tracks[rowIndexes[data.Track]].addButton(button, data);
            actionButton.Clicked += new EventHandler(actionButton_Clicked);
        }

        public void removeData(TimelineData data)
        {
            TimelineViewButton button = tracks[rowIndexes[data.Track]].removeButton(data);
            if (button == CurrentButton)
            {
                //Null the internal property first as you do not want to toggle the state of the button that has already been disposed.
                currentButton = null;
                CurrentButton = null;
            }
        }

        public void removeAllData()
        {
            foreach (TimelineViewTrack row in tracks)
            {
                row.removeAllActions();
            }
            currentButton = null;
            CurrentButton = null;
            timelineScrollView.CanvasWidth = 2.0f;
            timelineScrollView.CanvasHeight = tracks.Count != 0 ? tracks[tracks.Count - 1].Bottom : 0.0f;
        }

        public void trimVisibleArea()
        {
            TimelineViewButton rightmostButton = null;
            foreach (TimelineViewTrack row in tracks)
            {
                row.findRightmostButton(ref rightmostButton);
            }
            if (rightmostButton != null)
            {
                timelineScrollView.CanvasWidth = rightmostButton.Right;
            }
            else
            {
                timelineScrollView.CanvasWidth = 2.0f;
                timelineScrollView.CanvasHeight = tracks.Count != 0 ? tracks[tracks.Count - 1].Bottom : 0.0f;
            }
        }

        public int PixelsPerSecond
        {
            get
            {
                return pixelsPerSecond;
            }
            set
            {
                if (pixelsPerSecond != value)
                {
                    pixelsPerSecond = value;
                    if (pixelsPerSecond < 1)
                    {
                        pixelsPerSecond = 1;
                    }
                    else if (pixelsPerSecond > 150)
                    {
                        pixelsPerSecond = 150;
                    }
                    foreach (TimelineViewTrack row in tracks)
                    {
                        row.changePixelsPerSecond(pixelsPerSecond);
                    }
                    trimVisibleArea();
                    if (PixelsPerSecondChanged != null)
                    {
                        PixelsPerSecondChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public IEnumerable<TimelineViewTrack> Tracks
        {
            get
            {
                return tracks;
            }
        }

        public float MarkerTime
        {
            get
            {
                return timelineMarker.Time;
            }
            set
            {
                timelineMarker.Time = value;
            }
        }

        public TimelineData CurrentData
        {
            get
            {
                return currentButton != null ? currentButton.Data : null;
            }
            set
            {
                TimelineViewButton button = tracks[rowIndexes[value.Track]].findButton(value);
                CurrentButton = button;
            }
        }

        void timelineMarker_CoordChanged(object sender, EventArgs e)
        {
            respondToCoordChange(timelineMarker.Left, timelineMarker.Right, timelineMarker.Width);
        }

        void currentButton_CoordChanged(object sender, EventArgs e)
        {
            respondToCoordChange(currentButton.Left, currentButton.Right, currentButton.Width);
        }

        private void respondToCoordChange(int left, int right, int width)
        {
            float canvasWidth = timelineScrollView.CanvasWidth;
            //Ensure the canvas is large enough.
            if (right > canvasWidth)
            {
                timelineScrollView.CanvasWidth = right;
            }

            //Ensure the button is still visible.
            Vector2 canvasPosition = timelineScrollView.CanvasPosition;
            int clientWidth = timelineScrollView.ClientCoord.width;

            float visibleSize = canvasPosition.x + clientWidth;
            if (visibleSize + PREVIEW_PADDING < canvasWidth)
            {
                visibleSize -= PREVIEW_PADDING;
            }
            int rightSide = right;
            //If the button is longer than the display area tweak the right side value.
            if (width > clientWidth)
            {
                rightSide = left + clientWidth - PREVIEW_PADDING * 2;
            }
            //Ensure the right side is visible
            if (rightSide > visibleSize)
            {
                canvasPosition.x += rightSide - visibleSize;
                timelineScrollView.CanvasPosition = canvasPosition;
            }
            //Ensure the left side is visible as well
            else if (left < canvasPosition.x + PREVIEW_PADDING)
            {
                canvasPosition.x = left - PREVIEW_PADDING;
                if (canvasPosition.x < 0.0f)
                {
                    canvasPosition.x = 0.0f;
                }
                timelineScrollView.CanvasPosition = canvasPosition;
            }
        }

        void actionButton_Clicked(object sender, EventArgs e)
        {
            CurrentButton = sender as TimelineViewButton;
        }

        void scrollView_KeyButtonReleased(Widget source, EventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            if (ke.Key == Engine.Platform.KeyboardButtonCode.KC_LCONTROL)
            {
                timelineScrollView.AllowMouseScroll = true;
            }
        }

        void scrollView_KeyButtonPressed(Widget source, EventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            if (ke.Key == Engine.Platform.KeyboardButtonCode.KC_LCONTROL)
            {
                timelineScrollView.AllowMouseScroll = false;
            }
        }

        void scrollView_MouseWheel(Widget source, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            if (PixelsPerSecond > 10)
            {
                PixelsPerSecond += (int)(10 * (me.RelativeWheelPosition / 120.0f));
            }
            else if (PixelsPerSecond < 10)
            {
                PixelsPerSecond += (int)((me.RelativeWheelPosition / 120.0f));
            }
            else if (PixelsPerSecond == 10)
            {
                if (me.RelativeWheelPosition > 0)
                {
                    PixelsPerSecond += (int)(10 * (me.RelativeWheelPosition / 120.0f));
                }
                else
                {
                    PixelsPerSecond += (int)((me.RelativeWheelPosition / 120.0f));
                }
            }
        }

        void scrollView_MouseLostFocus(Widget source, EventArgs e)
        {
            FocusEventArgs fe = e as FocusEventArgs;
            Widget newFocus = fe.OtherWidget;
            if (newFocus == null)
            {
                timelineScrollView.AllowMouseScroll = true;
            }
            else
            {
                int absRight = timelineScrollView.AbsoluteLeft + timelineScrollView.Width;
                int absBottom = timelineScrollView.AbsoluteTop + timelineScrollView.Height;
                if (newFocus.AbsoluteLeft < timelineScrollView.AbsoluteLeft || newFocus.AbsoluteLeft > absRight
                                            || newFocus.AbsoluteTop < timelineScrollView.AbsoluteTop || newFocus.AbsoluteTop > absBottom)
                {
                    timelineScrollView.AllowMouseScroll = true;
                }
            }
        }

        void fireRowPositionChanged(TimelineViewTrack row)
        {
            if (TrackPositionChanged != null)
            {
                TrackPositionChanged.Invoke(row);
            }
        }

        void actionViewRow_BottomChanged(object sender, EventArgs e)
        {
            bool shiftRow = false;
            int lastBottom = 0;
            foreach (TimelineViewTrack row in tracks)
            {
                if (shiftRow)
                {
                    row.moveEntireRow(lastBottom);
                    fireRowPositionChanged(row);
                }
                else
                {
                    shiftRow = row == sender;
                }
                lastBottom = row.Bottom;
            }
            timelineScrollView.CanvasHeight = lastBottom;
        }

        private TimelineViewButton CurrentButton
        {
            get
            {
                return currentButton;
            }
            set
            {
                if (currentButton != null)
                {
                    currentButton.StateCheck = false;
                    currentButton.CoordChanged -= currentButton_CoordChanged;
                }
                currentButton = value;
                if (currentButton != null)
                {
                    currentButton.StateCheck = true;
                    currentButton.CoordChanged += currentButton_CoordChanged;
                }
                if (ActiveActionChanged != null)
                {
                    ActiveActionChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}

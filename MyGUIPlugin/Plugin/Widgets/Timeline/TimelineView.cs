using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using Engine;
using Engine.Platform;

namespace MyGUIPlugin
{
    public delegate void TimelineTrackEvent(TimelineViewTrack track);

    enum SelectionMode
    {
        AddSelection,
        RemoveSelection,
        SetSelection
    }

    public class TimelineView : IDisposable
    {
        private static readonly Color[] DefaultTrackColors = { Color.FromRGB(0xb366ff), Color.FromRGB(0x66d598), Color.FromRGB(0xf5b362), Color.FromRGB(0x60b5f0), Color.FromRGB(0xff6666) };
        private static readonly Color[] DefaultTrackSelectedColors = { Color.FromRGB(0x8000ff), Color.FromRGB(0x00b050), Color.FromRGB(0xf58700), Color.FromRGB(0x008ef0), Color.FromRGB(0xff0000) };

        private static MessageEvent addSelection;
        private static MessageEvent removeSelection;

        static TimelineView()
        {
            addSelection = new MessageEvent(SelectionMode.AddSelection, MyGUIInterface.EventLayerKey);
            addSelection.addButton(KeyboardButtonCode.KC_LCONTROL);
            DefaultEvents.registerDefaultEvent(addSelection);

            removeSelection = new MessageEvent(SelectionMode.RemoveSelection, MyGUIInterface.EventLayerKey);
            removeSelection.addButton(KeyboardButtonCode.KC_LMENU);
            DefaultEvents.registerDefaultEvent(removeSelection);
        }

        private static readonly int PREVIEW_PADDING = ScaleHelper.Scaled(10);
        private static readonly int TRACK_START_Y = ScaleHelper.Scaled(3);

        private static readonly int MaxPixelsPerSecond = ScaleHelper.Scaled(150);
        private static readonly int MinPixelsPerSecond = ScaleHelper.Scaled(1);
        private static readonly int StartingPixelsPerSecond = ScaleHelper.Scaled(100);

        private TimelineScrollView timelineScrollView;
        private int pixelsPerSecond = StartingPixelsPerSecond;
        private Dictionary<String, TimelineViewTrack> namedTracks = new Dictionary<string, TimelineViewTrack>();
        private List<TimelineViewTrack> tracks = new List<TimelineViewTrack>();
        private TimelineMarker timelineMarker;
        private int trackY = TRACK_START_Y;
        private float duration = float.MaxValue;
        private bool enabled = true;
        private TimelineSelectionCollection selectionCollection;
        private TimelineSelectionBox timelineSelectionBox;

        public event EventHandler<CancelEventArgs> ActiveDataChanging;
        public event EventHandler ActiveDataChanged;
        public event TimelineTrackEvent TrackPositionChanged;
        public event TimelineTrackEvent TrackAdded;
        public event TimelineTrackEvent TrackRemoved;
        public event EventHandler PixelsPerSecondChanged;
        public event EventHandler DurationChanged;
        public event EventHandler<KeyEventArgs> KeyPressed;
        public event EventHandler<KeyEventArgs> KeyReleased;
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

        public event EventDelegate<TimelineView, float> MarkerMoved;

        public TimelineView(ScrollView scrollView)
        {
            timelineSelectionBox = new TimelineSelectionBox(scrollView);
            timelineSelectionBox.SelectionAreaDefined += new EventDelegate<TimelineSelectionBox>(timelineSelectionBox_SelectionAreaDefined);
            selectionCollection = new TimelineSelectionCollection(this);
            timelineScrollView = new TimelineScrollView(scrollView);
            scrollView.ClientWidget.MouseLostFocus += new MyGUIEvent(scrollView_MouseLostFocus);
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

        public void addTrack(String name, Object userObject = null)
        {
            int colorIndex = tracks.Count % DefaultTrackColors.Length;
            TimelineViewTrack track = new TimelineViewTrack(name, trackY, pixelsPerSecond, duration, DefaultTrackColors[colorIndex], DefaultTrackSelectedColors[colorIndex]);
            track.BottomChanged += new EventHandler(actionViewRow_BottomChanged);
            track.UserObject = userObject;
            tracks.Add(track);
            namedTracks.Add(name, track);
            trackY = track.Bottom;
            timelineScrollView.CanvasHeight = trackY;
            if (TrackAdded != null)
            {
                TrackAdded.Invoke(track);
            }
        }

        public void removeTrack(String name)
        {
            selectionCollection.clearSelection();
            TimelineViewTrack track = namedTracks[name];
            track.removeAllActions();
            namedTracks.Remove(name);
            tracks.Remove(track);
            //Look for new bottom.
            int lowestTrack = TRACK_START_Y;
            foreach (TimelineViewTrack leftoverTrack in tracks)
            {
                int bottom = leftoverTrack.Bottom;
                if (bottom > lowestTrack)
                {
                    lowestTrack = bottom;
                }
            }
            trackY = lowestTrack;
            if (TrackRemoved != null)
            {
                TrackRemoved.Invoke(track);
            }
            track.Dispose();
        }

        public void clearTracks()
        {
            selectionCollection.clearSelection();
            foreach (TimelineViewTrack track in tracks)
            {
                if (TrackRemoved != null)
                {
                    TrackRemoved.Invoke(track);
                }
                track.Dispose();
            }
            trackY = TRACK_START_Y;
            namedTracks.Clear();
            tracks.Clear();
        }

        public void addData(TimelineData data, bool clearSelection = true)
        {
            if (clearSelection)
            {
                selectionCollection.clearSelection();
            }
            Button button = timelineScrollView.createButton(pixelsPerSecond * data.StartTime, pixelsPerSecond * data.Duration);
            TimelineViewButton actionButton = namedTracks[data.Track].addButton(button, data);
            actionButton.Clicked += actionButton_Clicked;
        }

        public void removeData(TimelineData data)
        {
            selectionCollection.removeButton(data._CurrentButton);
            TimelineViewButton button = namedTracks[data.Track].removeButton(data);
            if (button == selectionCollection.CurrentButton)
            {
                //Null the internal property first as you do not want to toggle the state of the button that has already been disposed.
                selectionCollection.nullCurrentButton();
                selectionCollection.setCurrentButton(null);
            }
        }

        public void removeAllData()
        {
            selectionCollection.nullCurrentButton();
            selectionCollection.clearSelection();
            foreach (TimelineViewTrack row in tracks)
            {
                row.removeAllActions();
            }
            timelineScrollView.CanvasWidth = 2;
            timelineScrollView.CanvasHeight = tracks.Count != 0 ? tracks[tracks.Count - 1].Bottom : 0;
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
                timelineScrollView.CanvasWidth = 2;
                timelineScrollView.CanvasHeight = tracks.Count != 0 ? tracks[tracks.Count - 1].Bottom : 0;
            }
        }

        public void selectAll()
        {
            selectionCollection.clearSelection();
            foreach (TimelineViewTrack row in tracks)
            {
                row.selectAll(selectionCollection);
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
                    if (pixelsPerSecond < MinPixelsPerSecond)
                    {
                        pixelsPerSecond = MinPixelsPerSecond;
                    }
                    else if (pixelsPerSecond > MaxPixelsPerSecond)
                    {
                        pixelsPerSecond = MaxPixelsPerSecond;
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
                return selectionCollection.CurrentButton != null ? selectionCollection.CurrentButton.Data : null;
            }
            set
            {
                if (value != null)
                {
                    TimelineViewButton button = namedTracks[value.Track].findButton(value);
                    selectionCollection.setCurrentButton(button);
                }
                else
                {
                    selectionCollection.clearSelection();
                }
            }
        }

        public IEnumerable<TimelineData> SelectedData
        {
            get
            {
                List<TimelineData> timelineDataList = new List<TimelineData>();
                foreach (TimelineViewButton button in selectionCollection.SelectedButtons)
                {
                    timelineDataList.Add(button.Data);
                }
                return timelineDataList;
            }
        }

        public float Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
                if (duration < 0.0f)
                {
                    duration = 0.0f;
                }
                foreach (TimelineViewTrack row in tracks)
                {
                    row.changeDuration(duration);
                }
                trimVisibleArea();
                if (DurationChanged != null)
                {
                    DurationChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    timelineMarker.Enabled = enabled;
                }
            }
        }

        void timelineMarker_CoordChanged(object sender, EventArgs e)
        {
            respondToCoordChange(timelineMarker.Left, timelineMarker.Right, timelineMarker.Width);
            if (MarkerMoved != null)
            {
                MarkerMoved.Invoke(this, timelineMarker.Time);
            }
        }

        internal void respondToCoordChange(int left, int right, int width)
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

        internal void fireActiveDataChanging(CancelEventArgs cancelArgs)
        {
            if (ActiveDataChanging != null)
            {
                ActiveDataChanging.Invoke(this, cancelArgs);
            }
        }

        internal void fireActiveDataChanged()
        {
            if (ActiveDataChanged != null)
            {
                ActiveDataChanged.Invoke(this, EventArgs.Empty);
            }
        }

        void actionButton_Clicked(TimelineViewButton sender, MouseEventArgs e)
        {
            if (addSelection.HeldDown)
            {
                selectionCollection.addButton(sender);
            }
            else if (removeSelection.HeldDown)
            {
                selectionCollection.removeButton(sender);
            }
            else
            {
                if (!sender.StateCheck)
                {
                    selectionCollection.clearSelection(false);
                }
                selectionCollection.setCurrentButton(sender);
            }
        }

        void scrollView_KeyButtonReleased(Widget source, EventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            if (ke.Key == Engine.Platform.KeyboardButtonCode.KC_LCONTROL)
            {
                timelineScrollView.AllowMouseScroll = true;
            }
            if (KeyReleased != null)
            {
                KeyReleased.Invoke(this, ke);
            }
        }

        void scrollView_KeyButtonPressed(Widget source, EventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            if (ke.Key == Engine.Platform.KeyboardButtonCode.KC_LCONTROL)
            {
                timelineScrollView.AllowMouseScroll = false;
            }
            if (KeyPressed != null)
            {
                KeyPressed.Invoke(this, ke);
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

        void timelineSelectionBox_SelectionAreaDefined(TimelineSelectionBox source)
        {
            if (addSelection.HeldDown)
            {
                foreach (TimelineViewTrack track in tracks)
                {
                    track.addSelection(selectionCollection, source);
                }
            }
            else if (removeSelection.HeldDown)
            {
                foreach (TimelineViewTrack track in tracks)
                {
                    track.removeSelection(selectionCollection, source);
                }
            }
            else
            {
                selectionCollection.clearSelection();
                foreach (TimelineViewTrack track in tracks)
                {
                    track.addSelection(selectionCollection, source);
                }
            }
        }
    }
}

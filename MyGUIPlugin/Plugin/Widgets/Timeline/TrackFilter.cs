using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using Engine;

namespace MyGUIPlugin
{
    public delegate void AddTrackItemCallback(String name, Object trackUserObject);

    public class TrackFilter
    {
        public event AddTrackItemCallback AddTrackItem;

        private ScrollView scrollView;
        private Dictionary<TimelineViewTrack, TrackFilterButton> filterButtons = new Dictionary<TimelineViewTrack, TrackFilterButton>();

        private static readonly int ButtonWidth = ScaleHelper.Scaled(15);
        private static readonly int ButtonHeight = ScaleHelper.Scaled(15);
        private int textWidth;

        private bool enabled = true;

        public TrackFilter(ScrollView scrollView, TimelineView actionView)
        {
            this.scrollView = scrollView;
            textWidth = (int)scrollView.CanvasSize.Width - ButtonWidth - 1;

            actionView.TrackPositionChanged += new TimelineTrackEvent(actionView_TrackPositionChanged);
            actionView.CanvasHeightChanged += new CanvasSizeChanged(actionView_CanvasHeightChanged);
            actionView.CanvasPositionChanged += new CanvasPositionChanged(actionView_CanvasPositionChanged);
            actionView.TrackAdded += new TimelineTrackEvent(actionView_RowAdded);
            actionView.TrackRemoved += new TimelineTrackEvent(actionView_TrackRemoved);

            foreach (TimelineViewTrack row in actionView.Tracks)
            {
                actionView_RowAdded(row);
            }
        }

        void actionView_RowAdded(TimelineViewTrack track)
        {            
            Button button = (Button)scrollView.createWidgetT("Button", "ButtonExpandSkin", 0, track.Top, ButtonWidth, ButtonHeight, Align.Default, "");
            button.Selected = true;
            
            TextBox staticText = (TextBox)scrollView.createWidgetT("TextBox", "TextBox", ButtonWidth + 1, track.Top, textWidth, ButtonHeight, Align.Default, "");
            staticText.TextAlign = Align.Left | Align.VCenter;
            staticText.TextColor = track.SelectedColor;
            
            TrackFilterButton filterButton = new TrackFilterButton(button, staticText, track);
            filterButtons.Add(track, filterButton);
            filterButton.CreateButtonClicked += filterButton_CreateButtonClicked;
            filterButton.Enabled = enabled;

            //Resize canvas
            IntSize2 canvasSize = scrollView.CanvasSize;
            canvasSize.Height = button.Bottom;
            scrollView.CanvasSize = canvasSize;
        }

        void actionView_TrackRemoved(TimelineViewTrack track)
        {
            TrackFilterButton filterButton = filterButtons[track];
            filterButtons.Remove(track);
            filterButton.Dispose();
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
                    foreach (TrackFilterButton button in filterButtons.Values)
                    {
                        button.Enabled = enabled;
                    }
                }
            }
        }

        void actionView_TrackPositionChanged(TimelineViewTrack row)
        {
            TrackFilterButton button = filterButtons[row];
            button.moveButtonTop(row.Top);
        }

        void actionView_CanvasHeightChanged(int newSize)
        {
            IntSize2 canvasSize = scrollView.CanvasSize;
            canvasSize.Height = newSize;
            scrollView.CanvasSize = canvasSize;
        }

        void actionView_CanvasPositionChanged(CanvasEventArgs info)
        {
            IntVector2 currentPos = scrollView.CanvasPosition;
            currentPos.y = -info.Top;
            scrollView.CanvasPosition = currentPos;
        }

        void filterButton_CreateButtonClicked(TrackFilterButton sender)
        {
            if (AddTrackItem != null)
            {
                AddTrackItem.Invoke(sender.Caption, sender.Track.UserObject);
            }
        }
    }
}

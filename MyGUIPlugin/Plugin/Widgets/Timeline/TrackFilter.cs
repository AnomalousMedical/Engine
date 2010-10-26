using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using Engine;

namespace MyGUIPlugin
{
    public class TrackFilter : IDisposable
    {
        private ScrollView scrollView;
        private Dictionary<TimelineViewTrack, TrackFilterButton> filterButtons = new Dictionary<TimelineViewTrack, TrackFilterButton>();

        private int buttonWidth;
        private int buttonHeight = 19;

        public TrackFilter(ScrollView scrollView, TimelineView actionView)
        {
            this.scrollView = scrollView;
            buttonWidth = (int)scrollView.CanvasSize.Width;

            actionView.TrackPositionChanged += new TimelineTrackEvent(actionView_TrackPositionChanged);
            actionView.CanvasHeightChanged += new CanvasSizeChanged(actionView_CanvasHeightChanged);
            actionView.CanvasPositionChanged += new CanvasPositionChanged(actionView_CanvasPositionChanged);
            actionView.TrackAdded += new TimelineTrackEvent(actionView_RowAdded);

            Button button = null;
            foreach (TimelineViewTrack row in actionView.Tracks)
            {
                String actionName = row.Name;
                button = scrollView.createWidgetT("Button", "CheckBox", 0, row.Top, buttonWidth, buttonHeight, Align.Default, "") as Button;
                TrackFilterButton filterButton = new TrackFilterButton(button, actionName);
                filterButtons.Add(row, filterButton);
                button.TextColor = row.Color;
            }

            if (button != null)
            {
                Size2 canvasSize = scrollView.CanvasSize;
                canvasSize.Height = button.Bottom;
                scrollView.CanvasSize = canvasSize;
            }
        }

        void actionView_RowAdded(TimelineViewTrack row)
        {
            String actionName = row.Name;
            Button button = scrollView.createWidgetT("Button", "CheckBox", 0, row.Top, buttonWidth, buttonHeight, Align.Default, "") as Button;
            TrackFilterButton filterButton = new TrackFilterButton(button, actionName);
            filterButtons.Add(row, filterButton);
            button.TextColor = row.Color;
            Size2 canvasSize = scrollView.CanvasSize;
            canvasSize.Height = button.Bottom;
            scrollView.CanvasSize = canvasSize;
        }

        public void Dispose()
        {

        }

        void actionView_TrackPositionChanged(TimelineViewTrack row)
        {
            TrackFilterButton button = filterButtons[row];
            button.moveButtonTop(row.Top);
        }

        void actionView_CanvasHeightChanged(float newSize)
        {
            Size2 canvasSize = scrollView.CanvasSize;
            canvasSize.Height = newSize;
            scrollView.CanvasSize = canvasSize;
        }

        void actionView_CanvasPositionChanged(CanvasEventArgs info)
        {
            Vector2 currentPos = scrollView.CanvasPosition;
            currentPos.y = -info.Top;
            scrollView.CanvasPosition = currentPos;
        }
    }
}

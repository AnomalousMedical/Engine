using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using Engine;

namespace MyGUIPlugin
{
    public delegate void AddTrackItemCallback(String name);

    public class TrackFilter : IDisposable
    {
        public event AddTrackItemCallback AddTrackItem;

        private ScrollView scrollView;
        private Dictionary<TimelineViewTrack, TrackFilterButton> filterButtons = new Dictionary<TimelineViewTrack, TrackFilterButton>();

        private int buttonWidth = 19;
        private int textWidth;
        private int buttonHeight = 19;

        public TrackFilter(ScrollView scrollView, TimelineView actionView)
        {
            this.scrollView = scrollView;
            textWidth = (int)scrollView.CanvasSize.Width - buttonWidth - 1;

            actionView.TrackPositionChanged += new TimelineTrackEvent(actionView_TrackPositionChanged);
            actionView.CanvasHeightChanged += new CanvasSizeChanged(actionView_CanvasHeightChanged);
            actionView.CanvasPositionChanged += new CanvasPositionChanged(actionView_CanvasPositionChanged);
            actionView.TrackAdded += new TimelineTrackEvent(actionView_RowAdded);

            foreach (TimelineViewTrack row in actionView.Tracks)
            {
                actionView_RowAdded(row);
            }
        }

        void actionView_RowAdded(TimelineViewTrack row)
        {
            String actionName = row.Name;
            
            Button button = scrollView.createWidgetT("Button", "ButtonMinusPlus", 0, row.Top, buttonWidth, buttonHeight, Align.Default, "") as Button;
            button.StateCheck = true;
            
            StaticText staticText = scrollView.createWidgetT("StaticText", "StaticText", buttonWidth + 1, row.Top, textWidth, buttonHeight, Align.Default, "") as StaticText;
            staticText.TextAlign = Align.Left | Align.VCenter;
            staticText.TextColor = row.Color;
            
            TrackFilterButton filterButton = new TrackFilterButton(button, staticText, actionName);
            filterButtons.Add(row, filterButton);
            filterButton.CreateButtonClicked += new EventHandler(filterButton_CreateButtonClicked);

            //Resize canvas
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

        void filterButton_CreateButtonClicked(object sender, EventArgs e)
        {
            if (AddTrackItem != null)
            {
                AddTrackItem.Invoke(((TrackFilterButton)sender).Caption);
            }
        }
    }
}

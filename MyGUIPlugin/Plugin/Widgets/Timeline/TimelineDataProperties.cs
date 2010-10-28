using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class TimelineDataProperties
    {
        private ScrollView propScroll;
        private TimelineData data;
        private TimelineView timelineView;

        private NumericEdit startTime;
        private NumericEdit duration;

        private Dictionary<String, TimelineDataPanel> additionalProperties = new Dictionary<String, TimelineDataPanel>();
        private Vector2 additionalPropertiesPosition;
        private TimelineDataPanel currentPanel;

        public TimelineDataProperties(ScrollView propScroll, TimelineView timelineView)
        {
            this.propScroll = propScroll;
            this.timelineView = timelineView;
            timelineView.ActiveDataChanged += new EventHandler(timelineView_ActiveDataChanged);

            startTime = new NumericEdit(propScroll.findWidget("StartTime") as Edit);
            startTime.ValueChanged += new MyGUIEvent(startTime_ValueChanged);
            startTime.MaxValue = float.MaxValue;
            duration = new NumericEdit(propScroll.findWidget("Duration") as Edit);
            duration.ValueChanged += new MyGUIEvent(duration_ValueChanged);
            duration.MaxValue = float.MaxValue;

            additionalPropertiesPosition = new Vector2(1, duration.Edit.Bottom + 2);
        }

        public void addPanel(String track, TimelineDataPanel panel)
        {
            panel.setPosition((int)additionalPropertiesPosition.x, (int)additionalPropertiesPosition.y);
            additionalProperties.Add(track, panel);
        }

        public TimelineData CurrentData
        {
            get
            {
                return data;
            }
            set
            {
                Size2 canvasSize = propScroll.CanvasSize;
                canvasSize.Height = additionalPropertiesPosition.y;
                if (currentPanel != null)
                {
                    currentPanel.Visible = false;
                }
                data = value;
                if (data != null)
                {
                    startTime.FloatValue = data.StartTime;
                    duration.FloatValue = data.Duration;
                    data.editingStarted();
                    additionalProperties.TryGetValue(data.Track, out currentPanel);
                    if (currentPanel != null)
                    {
                        currentPanel.Visible = true;
                        currentPanel.setCurrentData(value);
                        canvasSize.Height = currentPanel.Bottom;
                    }
                }
                else
                {
                    startTime.FloatValue = 0.0f;
                    duration.FloatValue = 0.0f;
                    currentPanel = null;
                }
                propScroll.CanvasSize = canvasSize;
            }
        }

        public bool Visible
        {
            get
            {
                return propScroll.Visible;
            }
            set
            {
                propScroll.Visible = value;
            }
        }

        void duration_ValueChanged(Widget source, EventArgs e)
        {
            if (data != null)
            {
                data.Duration = duration.FloatValue;
            }
        }

        void startTime_ValueChanged(Widget source, EventArgs e)
        {
            if (data != null)
            {
                data.StartTime = startTime.FloatValue;
            }
        }

        void timelineView_ActiveDataChanged(object sender, EventArgs e)
        {
            if (timelineView.CurrentData != null)
            {
                CurrentData = timelineView.CurrentData;
                Visible = true;
            }
            else
            {
                CurrentData = null;
                Visible = false;
            }
        }
    }
}

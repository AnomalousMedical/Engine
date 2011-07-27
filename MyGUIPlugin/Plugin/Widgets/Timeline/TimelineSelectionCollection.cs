using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    class TimelineSelectionCollection
    {
        private TimelineView timelineView;

        private List<TimelineViewButton> selectedButtons = new List<TimelineViewButton>();
        private TimelineViewButton currentButton;

        private CancelEventArgs cancelEventArgs = new CancelEventArgs();

        public TimelineSelectionCollection(TimelineView timelineView)
        {
            this.timelineView = timelineView;
        }

        public void nullCurrentButton()
        {
            currentButton = null;
        }

        public void addButton(TimelineViewButton button)
        {
            if (button != null)
            {
                button.StateCheck = true;
                button.CoordChanged += button_CoordChanged;
                selectedButtons.Add(button);
            }
        }

        public void removeButton(TimelineViewButton button)
        {
            if (button != null)
            {
                button.StateCheck = false;
                button.CoordChanged -= button_CoordChanged;
                selectedButtons.Add(button);
            }
        }

        public TimelineViewButton CurrentButton
        {
            get
            {
                return currentButton;
            }
            set
            {
                cancelEventArgs.reset();
                timelineView.fireActiveDataChanging(cancelEventArgs);
                if (!cancelEventArgs.Cancel)
                {
                    removeButton(currentButton);
                    currentButton = value;
                    addButton(currentButton);
                    timelineView.fireActiveDataChanged();
                }
            }
        }

        void button_CoordChanged(TimelineViewButton sender, TimelineViewButtonEventArgs e)
        {
            timelineView.respondToCoordChange(currentButton.Left, currentButton.Right, currentButton.Width);
        }
    }
}

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
            if (button != null && !selectedButtons.Contains(button))
            {
                setButtonSelected(button);
                selectedButtons.Add(button);
            }
        }

        public void removeButton(TimelineViewButton button)
        {
            if (button != null)
            {
                setButtonUnselected(button);
                selectedButtons.Remove(button);
                if (currentButton == button)
                {
                    setCurrentButton(null);
                }
            }
        }

        public void clearSelection()
        {
            if (setCurrentButton(null))
            {
                foreach (TimelineViewButton button in selectedButtons)
                {
                    setButtonUnselected(button);
                }
                selectedButtons.Clear();
            }
        }

        public bool setCurrentButton(TimelineViewButton button)
        {
            cancelEventArgs.reset();
            timelineView.fireActiveDataChanging(cancelEventArgs);
            if (!cancelEventArgs.Cancel)
            {
                addButton(button);
                currentButton = button;
                timelineView.fireActiveDataChanged();
                return true;
            }
            return false;
        }

        public TimelineViewButton CurrentButton
        {
            get
            {
                return currentButton;
            }
        }

        public IEnumerable<TimelineViewButton> SelectedButtons
        {
            get
            {
                return selectedButtons;
            }
        }

        public bool HasMultipleSelection
        {
            get
            {
                return selectedButtons.Count > 1;
            }
        }

        void button_ButtonDragged(TimelineViewButton source, float arg)
        {
            float startDelta = source.StartTime - arg;
            foreach (TimelineViewButton button in selectedButtons)
            {
                if (button != source)
                {
                    button.StartTime += startDelta;
                }
            }
        }

        void button_CoordChanged(TimelineViewButton sender, TimelineViewButtonEventArgs e)
        {
            timelineView.respondToCoordChange(sender.Left, sender.Right, sender.Width);
        }

        private void setButtonSelected(TimelineViewButton button)
        {
            button.StateCheck = true;
            button.CoordChanged += button_CoordChanged;
            button.ButtonDragged += button_ButtonDragged;
        }

        private void setButtonUnselected(TimelineViewButton button)
        {
            button.StateCheck = false;
            button.CoordChanged -= button_CoordChanged;
            button.ButtonDragged -= button_ButtonDragged;
        }
    }
}

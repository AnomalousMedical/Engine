using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using Engine;

namespace MyGUIPlugin
{
    public class TimelineViewTrack : IDisposable
    {
        public event EventHandler BottomChanged;

        private List<TimelineViewButton> buttons = new List<TimelineViewButton>();
        private int yPosition;
        private int pixelsPerSecond;
        private float timelineDuration;
        private Color color;
        private int ROW_HEIGHT = 19;
        private int STACKED_BUTTON_SPACE = 3;
        private int bottom;
        private bool processButtonChanges = true;

        public TimelineViewTrack(String name, int yPosition, int pixelsPerSecond, float timelineDuration, Color color)
        {
            this.Name = name;
            this.yPosition = yPosition;
            this.color = color;
            this.pixelsPerSecond = pixelsPerSecond;
            this.timelineDuration = timelineDuration;
            bottom = yPosition + ROW_HEIGHT;
        }

        public void Dispose()
        {
            foreach (TimelineViewButton button in buttons)
            {
                button.Dispose();
            }
        }

        public String Name { get; private set; }

        public Color Color
        {
            get
            {
                return color;
            }
        }

        internal TimelineViewButton addButton(Button button, TimelineData data)
        {
            TimelineViewButton viewButton = new TimelineViewButton(pixelsPerSecond, timelineDuration, button, data);
            buttons.Add(viewButton);
            button.setPosition(button.Left, yPosition);
            button.setColour(color);
            viewButton.CoordChanged += viewButton_CoordChanged;
            computeButtonPosition(viewButton);
            findLowestButton();
            return viewButton;
        }

        internal TimelineViewButton removeButton(TimelineData data)
        {
            TimelineViewButton removeMe = findButton(data);
            if (removeMe != null)
            {
                buttons.Remove(removeMe);
                removeMe.CoordChanged -= viewButton_CoordChanged;
                closeGaps(removeMe, removeMe.Left, removeMe.Right);
                removeMe.Dispose();
            }
            findLowestButton();
            return removeMe;
        }

        internal TimelineViewButton findButton(TimelineData data)
        {
            foreach (TimelineViewButton button in buttons)
            {
                if (button.Data == data)
                {
                    return button;
                }
            }
            return null;
        }

        internal void removeAllActions()
        {
            foreach (TimelineViewButton button in buttons)
            {
                button.Dispose();
            }
            buttons.Clear();
        }

        internal void changePixelsPerSecond(int pixelsPerSecond)
        {
            this.pixelsPerSecond = pixelsPerSecond;
            foreach (TimelineViewButton button in buttons)
            {
                button.changePixelsPerSecond(pixelsPerSecond);
            }
        }

        internal void changeDuration(float duration)
        {
            this.timelineDuration = duration;
            foreach (TimelineViewButton button in buttons)
            {
                button.changeDuration(duration);
            }
        }

        internal void findRightmostButton(ref TimelineViewButton rightmostButton)
        {
            if (rightmostButton == null && buttons.Count > 0)
            {
                rightmostButton = buttons[0];
            }
            foreach (TimelineViewButton button in buttons)
            {
                if (button.Right > rightmostButton.Right)
                {
                    rightmostButton = button;
                }
            }
        }

        internal void moveEntireRow(int newYPosition)
        {
            processButtonChanges = false;
            bottom = newYPosition + ROW_HEIGHT;
            int delta = newYPosition - yPosition;
            foreach (TimelineViewButton button in buttons)
            {
                button._moveTop(button.Top + delta);
                if (button.Bottom > bottom)
                {
                    bottom = button.Bottom;
                }
            }
            yPosition = newYPosition;
            processButtonChanges = true;
        }

        internal int Bottom
        {
            get
            {
                return bottom;
            }
        }

        internal int Top
        {
            get
            {
                return yPosition;
            }
        }

        internal void addSelection(TimelineSelectionCollection selectionCollection, TimelineSelectionBox timelineSelectionBox)
        {
            foreach (TimelineViewButton button in buttons)
            {
                if (timelineSelectionBox.intersects(button))
                {
                    selectionCollection.addButton(button);
                }
            }
        }

        internal void removeSelection(TimelineSelectionCollection selectionCollection, TimelineSelectionBox timelineSelectionBox)
        {
            foreach (TimelineViewButton button in buttons)
            {
                if (timelineSelectionBox.intersects(button))
                {
                    selectionCollection.removeButton(button);
                }
            }
        }

        private void viewButton_CoordChanged(TimelineViewButton sender, TimelineViewButtonEventArgs e)
        {
            if (processButtonChanges)
            {
                computeButtonPosition(sender);
                closeGaps(sender, e.OldLeft, e.OldRight);
                findLowestButton();
            }
        }

        private void computeButtonPosition(TimelineViewButton movedButton)
        {
            //Find the buttons that currently intersect the moved button
            List<TimelineViewButton> currentStackedButtons = new List<TimelineViewButton>();
            findIntersectingButtons(currentStackedButtons, movedButton.Left, movedButton.Right);
            currentStackedButtons.Remove(movedButton);

            //If there are no buttons currently intersecting the new position, put the button at the top.
            if (currentStackedButtons.Count == 0)
            {
                movedButton._moveTop(yPosition);
            }
            //Put the button at the first blank space that can be found
            else
            {
                insertButtonIntoStack(currentStackedButtons, movedButton);
            }
        }

        private void closeGaps(TimelineViewButton movedButton, int oldLeft, int oldRight)
        {
            //Move any buttons that can be moved up.
            //Find the buttons that intersect the old position
            List<TimelineViewButton> formerStackedButtons = new List<TimelineViewButton>();
            if (oldLeft == movedButton.Left && oldRight == movedButton.Right) //Did not move, could be removed.
            {
                findIntersectingButtons(formerStackedButtons, oldLeft, oldRight);
            }
            else if (oldLeft > movedButton.Left)//Moved left
            {
                findIntersectingButtons(formerStackedButtons, movedButton.Right, oldRight);
            }
            else //Moved right
            {
                findIntersectingButtons(formerStackedButtons, oldLeft, movedButton.Left);
            }
            formerStackedButtons.Remove(movedButton);

            //Sort the old stack by top.
            formerStackedButtons.Sort(topSortButtons);

            //Go through the list and find the index of the first gap.
            int gapIndex = findGapIndex(formerStackedButtons);
            if (gapIndex != -1)
            {
                for (int i = gapIndex; i < formerStackedButtons.Count; ++i)
                {
                    moveUp(formerStackedButtons[i]);
                }
            }
        }

        private void moveUp(TimelineViewButton button)
        {
            //Find the buttons that currently intersect the button
            List<TimelineViewButton> stackedButtons = new List<TimelineViewButton>();
            findIntersectingButtons(stackedButtons, button.Left, button.Right);
            stackedButtons.Remove(button);
            insertButtonIntoStack(stackedButtons, button);
        }

        private int findGapIndex(List<TimelineViewButton> sortedStackedButtons)
        {
            for(int i = 0; i < sortedStackedButtons.Count - 1; ++i)
            {
                if (sortedStackedButtons[i].Bottom + STACKED_BUTTON_SPACE != sortedStackedButtons[i + 1].Bottom || //Index is not next expected y position
                    sortedStackedButtons[i].Bottom != sortedStackedButtons[i + 1].Bottom) //Y positions are not the same
                {
                    return i;
                }
            }
            return -1;
        }

        private void findIntersectingButtons(List<TimelineViewButton> results, int left, int right)
        {
            foreach (TimelineViewButton compare in buttons)
            {
                if ((left >= compare.Left && left <= compare.Right) ||
                    (right >= compare.Left && right <= compare.Right) ||
                    (compare.Left >= left && compare.Left <= right) ||
                    (compare.Right >= left && compare.Right <= right))
                {
                    results.Add(compare);
                }
            }
        }

        private void insertButtonIntoStack(List<TimelineViewButton> buttonStack, TimelineViewButton insert)
        {
            //Sort the stack in order from top to bottom.
            buttonStack.Sort(topSortButtons);
            //Search the stack looking for a space in the buttons.
            int insertYPos = yPosition;
            int lastButtonYPos = -1;
            foreach (TimelineViewButton button in buttonStack)
            {
                if (lastButtonYPos != button.Top) //Make sure the next button is actually lower, its possible that it is not
                {
                    lastButtonYPos = button.Top;
                    if (button.Top == insertYPos)
                    {
                        insertYPos = button.Bottom + STACKED_BUTTON_SPACE;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            insert._moveTop(insertYPos);
        }

        private int topSortButtons(TimelineViewButton a1, TimelineViewButton a2)
        {
            return a1.Top - a2.Top;
        }

        private void findLowestButton()
        {
            int lowest = yPosition + ROW_HEIGHT;
            foreach (TimelineViewButton button in buttons)
            {
                if (button.Bottom > lowest)
                {
                    lowest = button.Bottom;
                }
            }
            if (lowest != bottom)
            {
                bottom = lowest + STACKED_BUTTON_SPACE;
                if (BottomChanged != null)
                {
                    BottomChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}

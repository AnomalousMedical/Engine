using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class SingleSelectionStrategy : ButtonGridSelectionStrategy
    {
        public event EventHandler SelectedValueChanged;

        private ButtonGridItem selectedItem;

        public SingleSelectionStrategy()
        {
            HighlightSelectedButton = true;
        }

        public void itemChosen(ButtonGridItem item)
        {
            SelectedItem = item;
        }

        public void itemRemoved(ButtonGridItem item)
        {
            if (item == SelectedItem)
            {
                SelectedItem = null;
            }
        }

        public void itemsCleared()
        {
            SelectedItem = null;
        }

        public bool HighlightSelectedButton { get; set; }

        public ButtonGridItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (selectedItem != value)
                {
                    if (selectedItem != null)
                    {
                        selectedItem.StateCheck = false;
                    }
                    selectedItem = value;
                    if (selectedItem != null)
                    {
                        selectedItem.StateCheck = HighlightSelectedButton;
                    }
                    if (SelectedValueChanged != null)
                    {
                        SelectedValueChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}

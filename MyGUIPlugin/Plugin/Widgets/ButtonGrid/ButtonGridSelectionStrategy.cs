using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class ButtonGridSelectionStrategy
    {
        public event EventHandler SelectedValueChanged;

        private ButtonGridItem selectedItem;

        public ButtonGridSelectionStrategy()
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

        public void cleared()
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

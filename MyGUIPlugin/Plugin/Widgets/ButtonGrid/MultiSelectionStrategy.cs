using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class MultiSelectionStrategy : ButtonGridSelectionStrategy
    {
        public event EventHandler SelectedValueChanged;

        private ButtonGridItem selectedItem;
        private List<ButtonGridItem> selectedItems = new List<ButtonGridItem>();

        public MultiSelectionStrategy()
        {
            
        }

        public void itemChosen(ButtonGridItem item)
        {
            if (InputManager.Instance.isControlPressed())
            {
                int index = selectedItems.IndexOf(item);
                if (index == -1)
                {
                    selectedItems.Add(item);
                    item.StateCheck = true;
                    setSelected(item);
                }
                else if (selectedItems.Count > 1)
                {
                    selectedItems.RemoveAt(index);
                    item.StateCheck = false;
                    if (item == SelectedItem)
                    {
                        if (index < selectedItems.Count)
                        {
                            setSelected(selectedItems[index]);
                        }
                        else if (index - 1 >= 0)
                        {
                            setSelected(selectedItems[index - 1]);
                        }
                        else
                        {
                            setSelected(null);
                        }
                    }
                }
            }
            else
            {
                uncheckSelectedAndClear();
                item.StateCheck = true;
                selectedItems.Add(item);
                setSelected(item);
            }
        }

        public void itemRemoved(ButtonGridItem item)
        {
            if (item == SelectedItem)
            {
                setSelected(null);
            }
            selectedItems.Remove(item);
        }

        public void itemsCleared()
        {
            setSelected(null);
            selectedItems.Clear();
        }

        public void setSelection(ButtonGridItem primary, IEnumerable<ButtonGridItem> secondary)
        {
            uncheckSelectedAndClear();
            selectedItems.Add(primary);
            selectedItems.AddRange(secondary);
            foreach (ButtonGridItem item in selectedItems)
            {
                item.StateCheck = true;
            }
            setSelected(primary);
        }

        public ButtonGridItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                uncheckSelectedAndClear();
                selectedItems.Add(value);
                setSelected(value);
            }
        }

        public IEnumerable<ButtonGridItem> SelectedItems
        {
            get
            {
                return selectedItems;
            }
        }

        private void setSelected(ButtonGridItem value)
        {
            if (selectedItem != value)
            {
                selectedItem = value;
                if (selectedItem != null)
                {
                    selectedItem.StateCheck = true;
                }
                if (SelectedValueChanged != null)
                {
                    SelectedValueChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void uncheckSelectedAndClear()
        {
            foreach (ButtonGridItem uncheck in selectedItems)
            {
                uncheck.StateCheck = false;
            }
            selectedItems.Clear();
        }
    }
}

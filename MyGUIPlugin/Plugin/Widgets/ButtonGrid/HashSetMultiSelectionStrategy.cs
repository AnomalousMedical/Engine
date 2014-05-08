using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class HashSetMultiSelectionStrategy : ButtonGridSelectionStrategy
    {
        public event EventHandler SelectedValueChanged;

        private ButtonGridItem selectedItem;
        private HashSet<ButtonGridItem> selectedItems = new HashSet<ButtonGridItem>();

        public HashSetMultiSelectionStrategy()
        {
            
        }

        public void itemChosen(ButtonGridItem item)
        {
            if (InputManager.Instance.isControlPressed())
            {
                if (selectedItems.Contains(item))
                {
                    removeSelected(item);
                }
                else
                {
                    addSelected(item);
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

        public void setSelection(IEnumerable<ButtonGridItem> items)
        {
            uncheckSelectedAndClear();
            foreach (ButtonGridItem item in items)
            {
                selectedItems.Add(item);
                item.StateCheck = true;
            }
            setSelected(selectedItems.FirstOrDefault());
        }

        public void setSelection(ButtonGridItem primary, IEnumerable<ButtonGridItem> secondary)
        {
            uncheckSelectedAndClear();
            if (primary != null)
            {
                selectedItems.Add(primary);
            }
            foreach (ButtonGridItem item in secondary)
            {
                selectedItems.Add(item);
                item.StateCheck = true;
            }
            setSelected(primary);
        }

        public void addSelected(ButtonGridItem item)
        {
            if (item != null && !selectedItems.Contains(item))
            {
                selectedItems.Add(item);
                item.StateCheck = true;
                setSelected(item);
            }
        }

        public void removeSelected(ButtonGridItem item)
        {
            if (item != null)
            {
                selectedItems.Remove(item);
                item.StateCheck = false;
                if (selectedItem == item)
                {
                    setSelected(selectedItems.FirstOrDefault());
                }
            }
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

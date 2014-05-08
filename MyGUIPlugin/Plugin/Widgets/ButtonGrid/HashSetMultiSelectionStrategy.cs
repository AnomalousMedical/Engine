using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class HashSetMultiSelectionStrategy : ButtonGridSelectionStrategy
    {
        public event EventHandler SelectedValueChanged;
        public event Action<ButtonGridItem> ItemChosen;

        private HashSet<ButtonGridItem> selectedItems = new HashSet<ButtonGridItem>();

        public HashSetMultiSelectionStrategy()
        {
            
        }

        public void itemChosen(ButtonGridItem item)
        {
            if(ItemChosen != null)
            {
                ItemChosen.Invoke(item);
            }
        }

        public void itemRemoved(ButtonGridItem item)
        {
            selectedItems.Remove(item);
        }

        public void itemsCleared()
        {
            selectedItems.Clear();
            setSelected();
        }

        public void setSelection(IEnumerable<ButtonGridItem> items)
        {
            uncheckSelectedAndClear();
            foreach (ButtonGridItem item in items)
            {
                selectedItems.Add(item);
                item.StateCheck = true;
            }
            setSelected();
        }

        public void addSelected(ButtonGridItem item)
        {
            if (item != null && !selectedItems.Contains(item))
            {
                selectedItems.Add(item);
                item.StateCheck = true;
                setSelected();
            }
        }

        public void removeSelected(ButtonGridItem item)
        {
            if (item != null)
            {
                selectedItems.Remove(item);
                item.StateCheck = false;
                setSelected();
            }
        }

        public IEnumerable<ButtonGridItem> SelectedItems
        {
            get
            {
                return selectedItems;
            }
        }

        private void setSelected()
        {
            if (SelectedValueChanged != null)
            {
                SelectedValueChanged.Invoke(this, EventArgs.Empty);
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

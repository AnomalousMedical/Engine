using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class provides a nice interface to a button grid with a single item able to be seleted at a time.
    /// </summary>
    public class HashSetMultiSelectButtonGrid : ButtonGrid
    {
        private HashSetMultiSelectionStrategy multiSelection = new HashSetMultiSelectionStrategy();

        public event EventHandler SelectedValueChanged
        {
            add
            {
                multiSelection.SelectedValueChanged += value;
            }
            remove
            {
                multiSelection.SelectedValueChanged -= value;
            }
        }

        public event Action<ButtonGridItem> ItemChosen
        {
            add
            {
                multiSelection.ItemChosen += value;
            }
            remove
            {
                multiSelection.ItemChosen -= value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrollView"></param>
        public HashSetMultiSelectButtonGrid(ScrollView scrollView)
            : this(scrollView, new ButtonGridGridLayout(), null, null)
        {

        }

        public HashSetMultiSelectButtonGrid(ScrollView scrollView, IComparer<ButtonGridItem> itemComparer)
            : this(scrollView, new ButtonGridGridLayout(), itemComparer, null)
        {

        }



        public HashSetMultiSelectButtonGrid(ScrollView scrollView, ButtonGridLayout layoutEngine)
            : this(scrollView, layoutEngine, null, null)
        {

        }

        public HashSetMultiSelectButtonGrid(ScrollView scrollView, ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer)
            : this(scrollView, layoutEngine, itemComparer, null)
        {

        }

        public HashSetMultiSelectButtonGrid(ScrollView scrollView, ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer, CompareButtonGroupUserObjects groupComparer)
            : base(scrollView, null, layoutEngine, itemComparer, groupComparer)
        {
            _workaroundSetSelectionStrategy(multiSelection);
        }

        public void setSelection(IEnumerable<ButtonGridItem> items)
        {
            multiSelection.setSelection(items);
        }

        public void addSelected(ButtonGridItem item)
        {
            multiSelection.addSelected(item);
        }

        public void removeSelected(ButtonGridItem item)
        {
            multiSelection.removeSelected(item);
        }

        public IEnumerable<ButtonGridItem> SelectedItems
        {
            get
            {
                return multiSelection.SelectedItems;
            }
        }
    }
}

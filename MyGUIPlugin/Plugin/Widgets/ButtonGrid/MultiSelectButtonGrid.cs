using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class provides a nice interface to a button grid with a single item able to be seleted at a time.
    /// </summary>
    public class MultiSelectButtonGrid : ButtonGrid
    {
        private MultiSelectionStrategy multiSelection = new MultiSelectionStrategy();

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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrollView"></param>
        public MultiSelectButtonGrid(ScrollView scrollView)
            : this(scrollView, new ButtonGridGridLayout(), null, null)
        {

        }

        public MultiSelectButtonGrid(ScrollView scrollView, IComparer<ButtonGridItem> itemComparer)
            : this(scrollView, new ButtonGridGridLayout(), itemComparer, null)
        {

        }



        public MultiSelectButtonGrid(ScrollView scrollView, ButtonGridLayout layoutEngine)
            : this(scrollView, layoutEngine, null, null)
        {

        }

        public MultiSelectButtonGrid(ScrollView scrollView, ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer)
            : this(scrollView, layoutEngine, itemComparer, null)
        {

        }

        public MultiSelectButtonGrid(ScrollView scrollView, ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer, CompareButtonGroupUserObjects groupComparer)
            : base(scrollView, null, layoutEngine, itemComparer, groupComparer)
        {
            _workaroundSetSelectionStrategy(multiSelection);
        }

        /// <summary>
        /// The currently selected item.
        /// </summary>
        public ButtonGridItem SelectedItem
        {
            get
            {
                return multiSelection.SelectedItem;
            }
            set
            {
                multiSelection.SelectedItem = value;
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class provides a nice interface to a button grid with a single item able to be seleted at a time.
    /// </summary>
    public class SingleSelectButtonGrid : ButtonGrid
    {
        private SingleSelectionStrategy singleSelection = new SingleSelectionStrategy();

        public event EventHandler SelectedValueChanged
        {
            add
            {
                singleSelection.SelectedValueChanged += value;
            }
            remove
            {
                singleSelection.SelectedValueChanged -= value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrollView"></param>
        public SingleSelectButtonGrid(ScrollView scrollView)
            :this(scrollView, new ButtonGridGridLayout(), null, null)
        {
            
        }

        public SingleSelectButtonGrid(ScrollView scrollView, IComparer<ButtonGridItem> itemComparer)
            :this(scrollView, new ButtonGridGridLayout(), itemComparer, null)
        {

        }



        public SingleSelectButtonGrid(ScrollView scrollView, ButtonGridLayout layoutEngine)
            : this(scrollView, layoutEngine, null, null)
        {

        }

        public SingleSelectButtonGrid(ScrollView scrollView, ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer)
            : this(scrollView, layoutEngine, itemComparer, null)
        {

        }

        public SingleSelectButtonGrid(ScrollView scrollView, ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer, CompareButtonGroupUserObjects groupComparer)
            : base(scrollView, null, layoutEngine, itemComparer, groupComparer)
        {
            _workaroundSetSelectionStrategy(singleSelection);
        }

        /// <summary>
        /// The currently selected item.
        /// </summary>
        public ButtonGridItem SelectedItem
        {
            get
            {
                return singleSelection.SelectedItem;
            }
            set
            {
                singleSelection.SelectedItem = value;
            }
        }
        
        public bool HighlightSelectedButton
        {
            get
            {
                return singleSelection.HighlightSelectedButton;
            }
            set
            {
                singleSelection.HighlightSelectedButton = value;
            }
        }
    }
}

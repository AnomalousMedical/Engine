using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    /// <summary>
    /// A button grid where items cannot be selected.
    /// </summary>
    public class NoSelectButtonGrid : ButtonGrid
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrollView"></param>
        public NoSelectButtonGrid(ScrollView scrollView)
            :this(scrollView, new ButtonGridGridLayout(), null)
        {
            
        }

        public NoSelectButtonGrid(ScrollView scrollView, IComparer<ButtonGridItem> itemComparer)
            :this(scrollView, new ButtonGridGridLayout(), itemComparer)
        {

        }



        public NoSelectButtonGrid(ScrollView scrollView, ButtonGridLayout layoutEngine)
            : this(scrollView, layoutEngine, null)
        {

        }

        public NoSelectButtonGrid(ScrollView scrollView, ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer)
            : base(scrollView, new NoSelectionStrategy(), layoutEngine, itemComparer)
        {

        }
    }
}

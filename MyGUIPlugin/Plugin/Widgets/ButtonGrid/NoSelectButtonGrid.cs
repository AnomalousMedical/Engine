using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class NoSelectButtonGrid : ButtonGrid
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrollView"></param>
        public NoSelectButtonGrid(ScrollView scrollView)
            :this(scrollView, new ButtonGridGridLayout(), null, null)
        {
            
        }

        public NoSelectButtonGrid(ScrollView scrollView, IComparer<ButtonGridItem> itemComparer)
            :this(scrollView, new ButtonGridGridLayout(), itemComparer, null)
        {

        }



        public NoSelectButtonGrid(ScrollView scrollView, ButtonGridLayout layoutEngine)
            : this(scrollView, layoutEngine, null, null)
        {

        }

        public NoSelectButtonGrid(ScrollView scrollView, ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer)
            : this(scrollView, layoutEngine, itemComparer, null)
        {

        }

        public NoSelectButtonGrid(ScrollView scrollView, ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer, CompareButtonGroupUserObjects groupComparer)
            : base(scrollView, new NoSelectionStrategy(), layoutEngine, itemComparer, groupComparer)
        {
            
        }
    }
}

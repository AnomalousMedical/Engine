using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class ButtonGridItemNaturalSort : IComparer<ButtonGridItem>
    {
        public int Compare(ButtonGridItem x, ButtonGridItem y)
        {
            return NaturalSortAlgorithm.CompareFunc(x.Caption, y.Caption);
        }
    }
}

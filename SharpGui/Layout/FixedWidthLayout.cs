using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpGui
{
    public class FixedWidthLayout : ILayoutItem
    {
        public ILayoutItem Child;
        public int Width;

        public FixedWidthLayout(int minWidth, ILayoutItem child)
        {
            this.Child = child;
            this.Width = minWidth;
        }

        public IntSize2 GetDesiredSize(ISharpGui sharpGui)
        {
            var size = Child.GetDesiredSize(sharpGui);
            size.Width = Width;
            return size;
        }

        public void SetRect(IntRect rect)
        {
            Child.SetRect(rect);
        }
    }
}

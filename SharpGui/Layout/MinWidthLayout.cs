using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpGui
{
    public class MinWidthLayout : ILayoutItem
    {
        public ILayoutItem Child;
        public int MinWidth;

        public MinWidthLayout(int minWidth, ILayoutItem child)
        {
            this.Child = child;
            this.MinWidth = minWidth;
        }

        public IntSize2 GetDesiredSize(ISharpGui sharpGui)
        {
            var size = Child.GetDesiredSize(sharpGui);
            size.Width = Math.Max(size.Width, MinWidth);
            return size;
        }

        public void SetRect(in IntRect rect)
        {
            Child.SetRect(rect);
        }
    }
}

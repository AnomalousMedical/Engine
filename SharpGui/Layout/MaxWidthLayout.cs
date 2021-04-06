using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpGui
{
    public class MaxWidthLayout : ILayoutItem
    {
        public ILayoutItem Child;
        public int MaxWidth;

        public MaxWidthLayout(int maxWidth, ILayoutItem child)
        {
            this.Child = child;
            this.MaxWidth = maxWidth;
        }

        public IntSize2 GetDesiredSize(ISharpGui sharpGui)
        {
            var size = Child.GetDesiredSize(sharpGui);
            size.Width = Math.Min(size.Width, MaxWidth);
            return size;
        }

        public void SetRect(in IntRect rect)
        {
            Child.SetRect(rect);
        }
    }
}

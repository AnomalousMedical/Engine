using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpGui
{
    public struct MarginLayout : ILayoutItem
    {
        public ILayoutItem Child;
        public IntPad Margin;

        public MarginLayout(IntPad margin, ILayoutItem child)
        {
            this.Child = child;
            this.Margin = margin;
        }

        public IntSize2 GetDesiredSize(ISharpGui sharpGui)
        {
            var size = Child.GetDesiredSize(sharpGui) + Margin.ToSize();
            return size;
        }

        public void SetRect(IntRect rect)
        {
            Child.SetRect(new IntRect(
                rect.Left + Margin.Left,
                rect.Top + Margin.Top,
                rect.Width - Margin.Left - Margin.Right,
                rect.Height - Margin.Top - Margin.Bottom
                ));
            ;
        }
    }
}

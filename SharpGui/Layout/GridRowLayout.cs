using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGui
{
    public class GridRowLayout : ILayoutItem
    {
        public IntPad Margin;

        private List<ILayoutItem> items;

        public IntSize2 GetDesiredSize(ISharpGui sharpGui)
        {
            int width = 0;
            int height = 0;

            foreach (var item in items)
            {
                var itemSize = item.GetDesiredSize(sharpGui);
                width = Math.Max(width, itemSize.Width);
                height = Math.Max(height, itemSize.Height);
            }

            width += Margin.Left + Margin.Right;
            height += Margin.Top + Margin.Bottom;

            width *= items.Count;

            return new IntSize2(width, height);
        }

        public void SetRect(in IntRect rect)
        {
            if(items.Count == 0)
            {
                return;
            }

            int left = rect.Left;
            int top = rect.Top;
            int height = rect.Height;
            int width = rect.Width / items.Count;

            foreach (var item in items)
            {
                item.SetRect(new IntRect(left + Margin.Left, top + Margin.Top, width - Margin.Left - Margin.Right, height - Margin.Top - Margin.Bottom));
                left += width;
            }
        }

        public GridRowLayout(int capacity = 5)
        {
            this.items = new List<ILayoutItem>(capacity);
        }

        public GridRowLayout(params ILayoutItem[] items)
        {
            this.items = new List<ILayoutItem>(items);
        }

        public void Clear()
        {
            items.Clear();
        }

        public void Add(ILayoutItem item)
        {
            items.Add(item);
        }

        public void Add(params ILayoutItem[] items)
        {
            this.items.AddRange(items);
        }

        public void Remove(ILayoutItem item)
        {
            items.Remove(item);
        }
    }
}

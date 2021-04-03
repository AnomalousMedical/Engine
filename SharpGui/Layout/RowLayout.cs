using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGui
{
    public class RowLayout : ILayoutItem
    {
        class Entry
        {
            public Entry(ILayoutItem layoutItem)
            {
                LayoutItem = layoutItem;
            }

            public ILayoutItem LayoutItem { get; set; }

            public int DesiredWidth { get; set; }
        }

        public IntPad Margin;

        private List<Entry> items;

        public IntSize2 GetDesiredSize(ISharpGui sharpGui)
        {
            int width = 0;
            int height = 0;

            foreach (var item in items)
            {
                var itemSize = item.LayoutItem.GetDesiredSize(sharpGui);
                height = Math.Max(height, itemSize.Height);
                item.DesiredWidth = itemSize.Width + Margin.Left + Margin.Right;
                width += item.DesiredWidth;
            }

            return new IntSize2(width, height + Margin.Top + Margin.Bottom);
        }

        public void SetRect(IntRect rect)
        {
            int left = rect.Left;
            int top = rect.Top;
            int height = rect.Height;

            foreach (var item in items)
            {
                int width = item.DesiredWidth;
                item.LayoutItem.SetRect(new IntRect(left + Margin.Left, top + Margin.Top, width - Margin.Left - Margin.Right, height - Margin.Top - Margin.Bottom));
                left += width;
            }
        }

        public RowLayout(int capacity = 10)
        {
            this.items = new List<Entry>(capacity);
        }

        public RowLayout(params ILayoutItem[] items)
        {
            this.items = new List<Entry>(items.Select(i => new Entry(i)));
        }

        public void Clear()
        {
            items.Clear();
        }

        public void Add(ILayoutItem item)
        {
            items.Add(new Entry(item));
        }

        public void Add(params ILayoutItem[] items)
        {
            this.items.AddRange(items.Select(i => new Entry(i)));
        }

        public void Remove(ILayoutItem item)
        {
            var itemCount = items.Count;
            for (var i = 0; i < itemCount; ++i)
            {
                var current = this.items[i];
                if (current.LayoutItem == item)
                {
                    items.RemoveAt(i);
                    return; //Done
                }
            }
        }
    }
}

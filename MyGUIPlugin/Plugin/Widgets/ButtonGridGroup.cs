using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    internal class ButtonGridGroup
    {
        private ButtonGrid grid;
        private List<ButtonGridItem> items = new List<ButtonGridItem>();
        private StaticText captionText;
        private Widget separator;

        public ButtonGridGroup(String name, ButtonGrid grid)
        {
            this.grid = grid;
            this.Name = name;

            if (grid.ShowGroupCaptions)
            {
                captionText = grid.ScrollView.createWidgetT("StaticText", grid.GroupCaptionSkin, 0, 0, 10, 10, Align.Left | Align.Top, "") as StaticText;
                captionText.Font = grid.GroupCaptionFont;
                captionText.Caption = name;
                captionText.setSize((int)captionText.getTextSize().Width + 5, (int)captionText.FontHeight);

                separator = grid.ScrollView.createWidgetT("Widget", grid.GroupSeparatorSkin, 0, 0, 10, 1, Align.Left | Align.Top, "");
                separator.setSize((int)(grid.ScrollView.CanvasSize.Width - captionText.Width) - 10, 1);
            }
        }

        public ButtonGridItem addItem(String caption)
        {
            ButtonGridItem item = new ButtonGridItem(this, grid);
            item.Caption = caption;
            items.Add(item);
            return item;
        }

        public void removeItem(ButtonGridItem item)
        {
            items.Remove(item);
            item.Dispose();
        }

        public void clear()
        {
            foreach (ButtonGridItem item in items)
            {
                item.Dispose();
            }
            items.Clear();
        }

        /// <summary>
        /// Layout the items in the group. Returns the position the next group should start in.
        /// </summary>
        /// <param name="startPosition">The start position</param>
        /// <returns>The position for the next group to start at.</returns>
        public Vector2 layout(Vector2 startPosition, IComparer<ButtonGridItem> itemComparer)
        {
            Vector2 currentPosition = startPosition;

            if (grid.ShowGroupCaptions)
            {
                currentPosition.y += 5;
                captionText.setPosition((int)currentPosition.x, (int)currentPosition.y);
                separator.setPosition((int)(currentPosition.x + captionText.Width), (int)(currentPosition.y + captionText.Height / 2));
                separator.setSize((int)(grid.ScrollView.CanvasSize.Width - captionText.Width) - 10, 1);
                currentPosition.y += captionText.Height + 5;
            }

            List<ButtonGridItem> sortedItems = items;
            if (itemComparer != null)
            {
                sortedItems = new List<ButtonGridItem>(items);
                sortedItems.Sort(itemComparer);
            }

            foreach (ButtonGridItem item in sortedItems)
            {
                if (currentPosition.x + item.Width > grid.ScrollView.CanvasSize.Width)
                {
                    currentPosition.x = 0;
                    currentPosition.y += grid.ItemHeight + grid.ItemPaddingY;
                }
                item.setPosition(currentPosition);
                currentPosition.x += item.Width + grid.ItemPaddingX;
            }

            currentPosition.x = 0;
            currentPosition.y += grid.ItemHeight + grid.ItemPaddingY;

            return currentPosition;
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public String Name { get; private set; }

        internal ButtonGrid Grid
        {
            get
            {
                return grid;
            }
        }

        internal ButtonGridItem getItem(int index)
        {
            return items[index];
        }

        internal ButtonGridItem findItemByCaption(string caption)
        {
            foreach (ButtonGridItem item in items)
            {
                if (item.Caption == caption)
                {
                    return item;
                }
            }
            return null;
        }
    }
}

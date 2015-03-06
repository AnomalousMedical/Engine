using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    internal class ButtonGridGroup : IDisposable
    {
        private ButtonGrid grid;
        private List<ButtonGridItem> items = new List<ButtonGridItem>();
        private ButtonGridCaption caption;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="grid">The grid the group belongs to.</param>
        public ButtonGridGroup(String name, ButtonGrid grid)
        {
            this.grid = grid;
            this.Name = name;

            if (grid.ShowGroupCaptions)
            {
                caption = grid.CaptionFactory.createCaption(name);

                toggleCaptionVisibility();
            }
        }

        /// <summary>
        /// Dispose all items.
        /// </summary>
        public void Dispose()
        {
            clear();
            if (caption != null)
            {
                grid.CaptionFactory.destroyCaption(caption);
                caption = null;
            }
        }

        /// <summary>
        /// Add an item to the group.
        /// </summary>
        /// <param name="caption">The caption to add.</param>
        /// <returns>The newly created ButtonGridItem.</returns>
        public ButtonGridItem addItem(String caption)
        {
            ButtonGridItem item = new ButtonGridItem(this, grid);
            item.Caption = caption;
            items.Add(item);
            toggleCaptionVisibility();
            if (items.Count == 1)
            {
                grid.NonEmptyGroupCount++;
            }
            return item;
        }

        /// <summary>
        /// Insert an item into the group.
        /// </summary>
        /// <param name="index">The index to insert at.</param>
        /// <param name="caption">The caption to use.</param>
        /// <returns>The newly created ButtonGridItem.</returns>
        public ButtonGridItem insertItem(int index, String caption)
        {
            ButtonGridItem item = new ButtonGridItem(this, grid);
            item.Caption = caption;
            items.Insert(index, item);
            toggleCaptionVisibility();
            if (items.Count == 1)
            {
                grid.NonEmptyGroupCount++;
            }
            return item;
        }

        /// <summary>
        /// Remove an item from the group.
        /// </summary>
        /// <param name="item"></param>
        public void removeItem(ButtonGridItem item)
        {
            items.Remove(item);
            item.Dispose();
            toggleCaptionVisibility();
            if (items.Count == 0)
            {
                grid.NonEmptyGroupCount--;
            }
        }

        /// <summary>
        /// Clear all items in the group.
        /// </summary>
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
        /// <param name="layoutEngine">The layout engine to use.</param>
        /// <param name="itemComparer">The item comparer to use, can be null.</param>
        public void layout(ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer)
        {
            if (items.Count > 0)
            {
                if (grid.ShowGroupCaptions)
                {
                    layoutEngine.alignCaption(caption);
                }

                IEnumerable<ButtonGridItem> sortedItems = items;
                if (itemComparer != null)
                {
                    sortedItems = items.OrderBy(i => i, itemComparer);
                }

                foreach (ButtonGridItem item in sortedItems)
                {
                    layoutEngine.alignItem(item);
                }

                layoutEngine.finishGroupLayout();
            }
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

        internal ButtonGridItem findItemByUserObject(Object userObject)
        {
            foreach (ButtonGridItem item in items)
            {
                if (item.UserObject == userObject)
                {
                    return item;
                }
            }
            return null;
        }

        internal int CaptionHeight
        {
            get
            {
                return caption != null ? caption.Height : 0;
            }
        }

        internal IEnumerable<ButtonGridItem> Items
        {
            get
            {
                return items;
            }
        }

        private void toggleCaptionVisibility()
        {
            if (caption != null)
            {
                caption.Visible = items.Count > 0;
            }
        }
    }
}

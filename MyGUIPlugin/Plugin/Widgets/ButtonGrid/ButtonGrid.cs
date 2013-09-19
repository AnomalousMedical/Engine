using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Logging;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class will create a series of buttons on a ScrollView making it
    /// work like a grid of buttons.
    /// </summary>
    /// <remarks>
    /// The view can be customized by some user options on the ScrollView.
    /// 
    /// ItemHeight - The height of each item.
    /// ItemWidth - The width of each item.
    /// ItemPaddingX - The padding between items in the x direction.
    /// ItemPaddingY - The padding between items in the y direction.
    /// ButtonSkin - The skin to use for buttons.
    /// GroupCaptionFont - The font to use for group captions.
    /// ShowGroupCaptions - True to show captions False to hide them.
    /// GroupCaptionSkin - The skin to use for group captions.
    /// GroupSeparatorSkin - The skin to use for group caption separators.
    /// </remarks>
    public class ButtonGrid : IDisposable
    {
        private ScrollView scrollView;
        private List<ButtonGridGroup> groups = new List<ButtonGridGroup>();
        private int itemCount = 0;
        private IComparer<ButtonGridItem> itemComparer;
        private ButtonGridGroupComparer groupComparer;
        private ButtonGridLayout layoutEngine;
        private ButtonGridSelectionStrategy selectionStrategy;

        public event EventHandler ItemActivated;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrollView"></param>
        public ButtonGrid(ScrollView scrollView, ButtonGridSelectionStrategy selectionStrategy)
            :this(scrollView, selectionStrategy, new ButtonGridGridLayout(), null, null)
        {
            
        }

        public ButtonGrid(ScrollView scrollView, ButtonGridSelectionStrategy selectionStrategy, IComparer<ButtonGridItem> itemComparer)
            : this(scrollView, selectionStrategy, new ButtonGridGridLayout(), itemComparer, null)
        {

        }



        public ButtonGrid(ScrollView scrollView, ButtonGridSelectionStrategy selectionStrategy, ButtonGridLayout layoutEngine)
            : this(scrollView, selectionStrategy, layoutEngine, null, null)
        {

        }

        public ButtonGrid(ScrollView scrollView, ButtonGridSelectionStrategy selectionStrategy, ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer)
            : this(scrollView, selectionStrategy, layoutEngine, itemComparer, null)
        {

        }

        public ButtonGrid(ScrollView scrollView, ButtonGridSelectionStrategy selectionStrategy, ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer, CompareButtonGroupUserObjects groupComparer)
        {
            this.selectionStrategy = selectionStrategy;
            NonEmptyGroupCount = 0;

            String read;
            bool boolValue;
            int intValue;
            SuppressLayout = false;

            this.scrollView = scrollView;
            this.itemComparer = itemComparer;
            this.layoutEngine = layoutEngine;
            if (groupComparer != null)
            {
                this.groupComparer = new ButtonGridGroupComparer(groupComparer);
            }

            //Try to get properties from the widget itself.
            read = scrollView.getUserString("ItemHeight");
            if (read != null && NumberParser.TryParse(read, out intValue))
            {
                ItemHeight = ScaleHelper.Scaled(intValue);
            }
            else
            {
                ItemHeight = ScaleHelper.Scaled(122);
            }

            read = scrollView.getUserString("ItemWidth");
            if (read != null && NumberParser.TryParse(read, out intValue))
            {
                ItemWidth = ScaleHelper.Scaled(intValue);
            }
            else
            {
                ItemWidth = ScaleHelper.Scaled(122);
            }

            read = scrollView.getUserString("ItemPaddingX");
            if (read != null && NumberParser.TryParse(read, out intValue))
            {
                ItemPaddingX = ScaleHelper.Scaled(intValue);
            }
            else
            {
                ItemPaddingX = ScaleHelper.Scaled(10);
            }

            read = scrollView.getUserString("ItemPaddingY");
            if (read != null && NumberParser.TryParse(read, out intValue))
            {
                ItemPaddingY = ScaleHelper.Scaled(intValue);
            }
            else
            {
                ItemPaddingY = ScaleHelper.Scaled(2);
            }

            read = scrollView.getUserString("GroupPaddingY");
            if (read != null && NumberParser.TryParse(read, out intValue))
            {
                GroupPaddingY = ScaleHelper.Scaled(intValue);
            }
            else
            {
                GroupPaddingY = ScaleHelper.Scaled(ItemPaddingY);
            }

            ButtonSkin = scrollView.getUserString("ButtonSkin");
            if (ButtonSkin == null || ButtonSkin == String.Empty)
            {
                ButtonSkin = "ButtonGridButton";
            }

            GroupCaptionFont = scrollView.getUserString("GroupCaptionFont");
            if (GroupCaptionFont == null || GroupCaptionFont == String.Empty)
            {
                GroupCaptionFont = "Default";
            }

            read = scrollView.getUserString("ShowGroupCaptions");
            if (read != null && bool.TryParse(read, out boolValue))
            {
                ShowGroupCaptions = boolValue;
            }
            else
            {
                ShowGroupCaptions = true;
            }

            GroupCaptionSkin = scrollView.getUserString("GroupCaptionSkin");
            if (GroupCaptionSkin == null || GroupCaptionSkin == String.Empty)
            {
                GroupCaptionSkin = "TextBox";
            }

            GroupSeparatorSkin = scrollView.getUserString("GroupSeparatorSkin");
            if (GroupSeparatorSkin == null || GroupSeparatorSkin == String.Empty)
            {
                GroupSeparatorSkin = "SeparatorSkin";
            }
        }

        /// <summary>
        /// Dispose function
        /// </summary>
        public void Dispose()
        {
            clear();
        }

        public void defineGroup(String group, Object userObject = null)
        {
            ButtonGridGroup addGroup = findGroup(group);
            addGroup.UserObject = userObject;
        }

        public void setGroupUserObject(String group, Object userObject)
        {
            foreach (ButtonGridGroup groupIter in groups)
            {
                if (groupIter.Name == group)
                {
                    groupIter.UserObject = userObject;
                    break;
                }
            }
        }

        /// <summary>
        /// Add an item.
        /// </summary>
        /// <param name="group">The group to add the item to.</param>
        /// <param name="caption">The caption for the item.</param>
        /// <param name="imageResource">The image resource for the item</param>
        /// <returns></returns>
        public ButtonGridItem addItem(String group, String caption, String imageResource = null)
        {
            ButtonGridGroup addGroup = findGroup(group);
            ButtonGridItem item = addGroup.addItem(caption);
            item.setImage(imageResource);
            itemCount++;
            layout();
            return item;
        }

        /// <summary>
        /// Insert an item into a ButtonGridGroup.
        /// </summary>
        /// <param name="index">The index in the group to put the item</param>
        /// <param name="group">The group to add the item to</param>
        /// <param name="caption">The caption for the item</param>
        /// <param name="imageResource">The image resource for the item</param>
        /// <returns></returns>
        public ButtonGridItem insertItem(int index, String group, String caption, String imageResource = null)
        {
            ButtonGridGroup addGroup = findGroup(group);
            ButtonGridItem item = addGroup.insertItem(index, caption);
            item.setImage(imageResource);
            itemCount++;
            layout();
            return item;
        }

        /// <summary>
        /// Remove an item.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public void removeItem(ButtonGridItem item)
        {
            selectionStrategy.itemRemoved(item);
            itemCount--;
            item.Group.removeItem(item);
            layout();
        }

        /// <summary>
        /// Clear all items.
        /// </summary>
        public void clear()
        {
            foreach (ButtonGridGroup group in groups)
            {
                group.Dispose();
            }
            selectionStrategy.itemsCleared();
            groups.Clear();
            itemCount = 0;
            layout();
        }

        public void layout()
        {
            if (!SuppressLayout)
            {
                List<ButtonGridGroup> sortedGroups = groups;
                if (groupComparer != null)
                {
                    sortedGroups = new List<ButtonGridGroup>(groups);
                    sortedGroups.Sort(groupComparer);
                }

                layoutEngine.startLayout(this);
                foreach (ButtonGridGroup group in sortedGroups)
                {
                    group.layout(layoutEngine, itemComparer);
                }
                scrollView.CanvasSize = layoutEngine.FinalCanvasSize;
            }
        }

        public void layoutAndResize(int rowCount)
        {
            IntSize2 finalSize = new IntSize2((ItemWidth + ItemPaddingX) * rowCount, 300);
            scrollView.CanvasSize = finalSize;
            layoutEngine.startLayout(this);
            foreach (ButtonGridGroup group in groups)
            {
                group.layout(layoutEngine, itemComparer);
            }
            finalSize.Height = layoutEngine.FinalCanvasSize.Height;
            scrollView.CanvasSize = finalSize;
            //Set the final size with or without padding depending if the scroll bars are visible in that direction or not.
            scrollView.setSize(scrollView.VisibleHScroll ? (int)finalSize.Width + 23 : (int)finalSize.Width,
                scrollView.VisibleVScroll ? (int)finalSize.Height + 23 : (int)finalSize.Height + 5);
        }

        public void resizeAndLayout(int newWidth)
        {
            IntSize2 canvasSize = scrollView.CanvasSize;
            canvasSize.Width = newWidth;
            scrollView.CanvasSize = canvasSize;
            layout();
        }

        public ButtonGridItem getItem(int index)
        {
            if (index >= itemCount)
            {
                throw new Exception("Index exceeded the number of items.");
            }
            else
            {
                foreach (ButtonGridGroup group in groups)
                {
                    if (index < group.Count)
                    {
                        return group.getItem(index);
                    }
                    index -= group.Count;
                }
            }
            throw new Exception("Should not get this exception.");
        }

        public ButtonGridItem findItemByCaption(String caption)
        {
            foreach (ButtonGridGroup group in groups)
            {
                ButtonGridItem item = group.findItemByCaption(caption);
                if (item != null)
                {
                    return item;
                }
            }
            return null;
        }

        public ButtonGridItem findItemByUserObject(Object userObject)
        {
            foreach (ButtonGridGroup group in groups)
            {
                ButtonGridItem item = group.findItemByUserObject(userObject);
                if (item != null)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// The skin to use for buttons in the grid.
        /// </summary>
        public String ButtonSkin { get; set; }

        /// <summary>
        /// The width of each button in the grid.
        /// </summary>
        public int ItemWidth
        {
            get
            {
                return layoutEngine.ItemWidth;
            }
            set
            {
                layoutEngine.ItemWidth = value;
            }
        }

        /// <summary>
        /// The height of each button in the grid.
        /// </summary>
        public int ItemHeight
        {
            get
            {
                return layoutEngine.ItemHeight;
            }
            set
            {
                layoutEngine.ItemHeight = value;
            }
        }

        /// <summary>
        /// The padding in the x dimesion between items.
        /// </summary>
        public int ItemPaddingX
        {
            get
            {
                return layoutEngine.ItemPaddingX;
            }
            set
            {
                layoutEngine.ItemPaddingX = value;
            }
        }

        /// <summary>
        /// The padding in the y dimension between items.
        /// </summary>
        public int ItemPaddingY
        {
            get
            {
                return layoutEngine.ItemPaddingY;
            }
            set
            {
                layoutEngine.ItemPaddingY = value;
            }
        }

        public int GroupPaddingY
        {
            get
            {
                return layoutEngine.GroupPaddingY;
            }
            set
            {
                layoutEngine.GroupPaddingY = value;
            }
        }

        /// <summary>
        /// The font for group captions.
        /// </summary>
        public String GroupCaptionFont { get; set; }

        /// <summary>
        /// True to show groups with captions, false to hide the captions.
        /// </summary>
        public bool ShowGroupCaptions { get; set; }

        /// <summary>
        /// The skin for the group caption.
        /// </summary>
        public String GroupCaptionSkin { get; set; }

        /// <summary>
        /// The skin for the group separators.
        /// </summary>
        public String GroupSeparatorSkin { get; set; }

        /// <summary>
        /// Set this to true to prevent the control from updating its layout as
        /// items are added/removed. You can then call layout to lay out the
        /// items (make sure you set SuppressLayout back to true).
        /// </summary>
        public bool SuppressLayout { get; set; }

        /// <summary>
        /// The number of items.
        /// </summary>
        public int Count
        {
            get
            {
                return itemCount;
            }
        }

        /// <summary>
        /// The number of groups
        /// </summary>
        public int GroupCount
        {
            get
            {
                return groups.Count;
            }
        }

        /// <summary>
        /// The number of groups that have something in them. Do not set outside of ButtonGridGroup.
        /// </summary>
        public int NonEmptyGroupCount { get; internal set; }

        /// <summary>
        /// A UserObject. Not used directly by this class.
        /// </summary>
        public Object UserObject { get; set; }

        /// <summary>
        /// The total height of all the items in the ButtonGrid.
        /// </summary>
        public int TotalHeight
        {
            get
            {
                return (int)scrollView.CanvasSize.Height;
            }
        }

        public int Width
        {
            get
            {
                return scrollView.Width;
            }
        }

        public int Height
        {
            get
            {
                return scrollView.Height;
            }
        }

        public IEnumerable<ButtonGridItem> Items
        {
            get
            {
                foreach (ButtonGridGroup group in groups)
                {
                    foreach (ButtonGridItem item in group.Items)
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// The ScrollView for the grid.
        /// </summary>
        internal ScrollView ScrollView
        {
            get
            {
                return scrollView;
            }
        }

        internal int CaptionHeight
        {
            get
            {
                return groups.Count > 0 ? groups[0].CaptionHeight : 0;
            }
        }

        internal void itemActivated(ButtonGridItem item)
        {
            if (ItemActivated != null)
            {
                ItemActivated.Invoke(item, EventArgs.Empty);
            }
        }

        internal void itemChosen(ButtonGridItem item)
        {
            selectionStrategy.itemChosen(item);
        }

        private ButtonGridGroup findGroup(String group)
        {
            ButtonGridGroup addGroup = null;
            foreach (ButtonGridGroup groupIter in groups)
            {
                if (groupIter.Name == group)
                {
                    addGroup = groupIter;
                    break;
                }
            }
            if (addGroup == null)
            {
                addGroup = new ButtonGridGroup(group, this);
                groups.Add(addGroup);
            }
            return addGroup;
        }

        /// <summary>
        /// This function only exists to allow the subclasses of ButtonGrid for common strategies to actually be able to set
        /// their strategy. This should not (and can't) be called from other classes. If you do need to use ButtonGrid raw 
        /// externally to this library just favor object composition to get your grid setup correctly. If you do make a new subclass
        /// of ButtonGrid, pass null for the strategy in the base call and then call this method as soon as possible.
        /// </summary>
        /// <param name="selectionStrategy"></param>
        protected internal void _workaroundSetSelectionStrategy(ButtonGridSelectionStrategy selectionStrategy)
        {
            this.selectionStrategy = selectionStrategy;
        }
    }
}

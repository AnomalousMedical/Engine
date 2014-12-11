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
    /// ButtonSkin - The skin to use for buttons.
    /// ShowGroupCaptions - True to show captions False to hide them.
    /// CaptionType - Either Horizontal, TopSeparator, BottomSeparator or TextOnly
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
        private ButtonGridCaptionFactory captionFactory;
        private bool allowClickEvents = true;

        /// <summary>
        /// Called when an item is activated. With the mouse this means double clicked.
        /// </summary>
        public event Action<ButtonGridItem> ItemActivated;

        /// <summary>
        /// Called when an item is added.
        /// </summary>
        public event Action<ButtonGrid, ButtonGridItem> ItemAdded;

        /// <summary>
        /// Called when an item is removed.
        /// </summary>
        public event Action<ButtonGrid, ButtonGridItem> ItemRemoved;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrollView">The scroll view host.</param>
        /// <param name="selectionStrategy">The selection strategy to use.</param>
        public ButtonGrid(ScrollView scrollView, ButtonGridSelectionStrategy selectionStrategy)
            : this(scrollView, selectionStrategy, new ButtonGridGridLayout(), null, null, CreateDefaultCaptionFactory(scrollView))
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrollView">The scroll view host.</param>
        /// <param name="selectionStrategy">The selection strategy to use.</param>
        /// <param name="itemComparer">A comparison instance.</param>
        public ButtonGrid(ScrollView scrollView, ButtonGridSelectionStrategy selectionStrategy, IComparer<ButtonGridItem> itemComparer)
            : this(scrollView, selectionStrategy, new ButtonGridGridLayout(), itemComparer, null, CreateDefaultCaptionFactory(scrollView))
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrollView">The scroll view host.</param>
        /// <param name="selectionStrategy">The selection strategy to use.</param>
        /// <param name="layoutEngine">The Layout to use.</param>
        public ButtonGrid(ScrollView scrollView, ButtonGridSelectionStrategy selectionStrategy, ButtonGridLayout layoutEngine)
            : this(scrollView, selectionStrategy, layoutEngine, null, null, CreateDefaultCaptionFactory(scrollView))
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrollView">The scroll view host.</param>
        /// <param name="selectionStrategy">The selection strategy to use.</param>
        /// <param name="layoutEngine">The Layout to use.</param>
        /// <param name="itemComparer">A comparison instance for items.</param>
        public ButtonGrid(ScrollView scrollView, ButtonGridSelectionStrategy selectionStrategy, ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer)
            : this(scrollView, selectionStrategy, layoutEngine, itemComparer, null, CreateDefaultCaptionFactory(scrollView))
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrollView">The scroll view host.</param>
        /// <param name="selectionStrategy">The selection strategy to use.</param>
        /// <param name="layoutEngine">The Layout to use.</param>
        /// <param name="itemComparer">A comparison instance.</param>
        /// <param name="groupComparer">A compraison instance for groups.</param>
        public ButtonGrid(ScrollView scrollView, ButtonGridSelectionStrategy selectionStrategy, ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer, CompareButtonGroupUserObjects groupComparer)
            : this(scrollView, selectionStrategy, layoutEngine, itemComparer, groupComparer, CreateDefaultCaptionFactory(scrollView))
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrollView">The scroll view host.</param>
        /// <param name="selectionStrategy">The selection strategy to use.</param>
        /// <param name="layoutEngine">The Layout to use.</param>
        /// <param name="itemComparer">A comparison instance.</param>
        /// <param name="groupComparer">A compraison instance for groups.</param>
        /// <param name="captionFactory">The factory to use to make captions.</param>
        public ButtonGrid(ScrollView scrollView, ButtonGridSelectionStrategy selectionStrategy, ButtonGridLayout layoutEngine, IComparer<ButtonGridItem> itemComparer, CompareButtonGroupUserObjects groupComparer, ButtonGridCaptionFactory captionFactory)
        {
            this.selectionStrategy = selectionStrategy;
            NonEmptyGroupCount = 0;

            String read;
            bool boolValue;
            int intValue;
            SuppressLayout = false;

            this.scrollView = scrollView;
            scrollView.CanvasPositionChanged += scrollView_CanvasPositionChanged;
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

            ButtonSkin = scrollView.getUserString("ButtonSkin");
            if (ButtonSkin == null || ButtonSkin == String.Empty)
            {
                ButtonSkin = "ButtonGridButton";
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

            read = scrollView.getUserString("ItemPaddingX");
            if (read != null && NumberParser.TryParse(read, out intValue))
            {
                layoutEngine.ItemPaddingX = ScaleHelper.Scaled(intValue);
            }

            read = scrollView.getUserString("ItemPaddingY");
            if (read != null && NumberParser.TryParse(read, out intValue))
            {
                layoutEngine.ItemPaddingY = ScaleHelper.Scaled(intValue);
            }

            read = scrollView.getUserString("GroupPaddingY");
            if (read != null && NumberParser.TryParse(read, out intValue))
            {
                layoutEngine.GroupPaddingY = ScaleHelper.Scaled(intValue);
            }

            this.CaptionFactory = captionFactory;
        }

        /// <summary>
        /// Dispose function
        /// </summary>
        public void Dispose()
        {
            scrollView.CanvasPositionChanged -= scrollView_CanvasPositionChanged;
            clear();
        }

        /// <summary>
        /// Define a group.
        /// </summary>
        /// <param name="group">The name.</param>
        /// <param name="userObject">An optional user object.</param>
        public void defineGroup(String group, Object userObject = null)
        {
            ButtonGridGroup addGroup = findGroup(group);
            addGroup.UserObject = userObject;
        }

        /// <summary>
        /// Set a UserObject for a group.
        /// </summary>
        /// <param name="group">The name of the group.</param>
        /// <param name="userObject">The UserObject to set.</param>
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
        public ButtonGridItem addItem(String group, String caption, String imageResource = null, Object userObject = null)
        {
            ButtonGridGroup addGroup = findGroup(group);
            ButtonGridItem item = addGroup.addItem(caption);
            item.setImage(imageResource);
            item.UserObject = userObject;
            itemCount++;
            layout();
            if(ItemAdded != null)
            {
                ItemAdded.Invoke(this, item);
            }
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
            if (ItemAdded != null)
            {
                ItemAdded.Invoke(this, item);
            }
            return item;
        }

        /// <summary>
        /// Remove an item.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public void removeItem(ButtonGridItem item)
        {
            if (ItemRemoved != null)
            {
                ItemRemoved.Invoke(this, item);
            }
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
                //Alert that the items are removed
                if (ItemRemoved != null)
                {
                    foreach (ButtonGridItem item in group.Items)
                    {
                        ItemRemoved.Invoke(this, item);
                    }
                }
                group.Dispose();
            }
            selectionStrategy.itemsCleared();
            groups.Clear();
            itemCount = 0;
            layout();
        }

        /// <summary>
        /// Layout within existing parameters.
        /// </summary>
        public void layout()
        {
            if (!SuppressLayout)
            {
                IEnumerable<ButtonGridGroup> sortedGroups = groups;
                if (groupComparer != null)
                {
                    sortedGroups = groups.OrderBy(i => i, groupComparer);
                }

                layoutEngine.startLayout(this);
                foreach (ButtonGridGroup group in sortedGroups)
                {
                    group.layout(layoutEngine, itemComparer);
                }
                scrollView.CanvasSize = layoutEngine.FinalCanvasSize;
            }
        }

        /// <summary>
        /// Resize to the scroll view's view coord width and layout again.
        /// </summary>
        public void resizeAndLayout()
        {
            resizeAndLayout(scrollView.ViewCoord.width);
        }

        /// <summary>
        /// Set a new width for the grid and layout again.
        /// </summary>
        /// <param name="newWidth"></param>
        public void resizeAndLayout(int newWidth)
        {
            IntSize2 canvasSize = scrollView.CanvasSize;
            canvasSize.Width = newWidth;
            scrollView.CanvasSize = canvasSize;
            layout();
        }

        /// <summary>
        /// Get the item at a specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Find an item based on its caption.
        /// </summary>
        /// <param name="caption">The caption to search for.</param>
        /// <returns>The first item matching the caption.</returns>
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

        /// <summary>
        /// Find an item based on the UserObject assigned to it.
        /// </summary>
        /// <param name="userObject">The user object to search for.</param>
        /// <returns>The first item matching the user object.</returns>
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
        /// True to show groups with captions, false to hide the captions.
        /// </summary>
        public bool ShowGroupCaptions { get; set; }

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

        /// <summary>
        /// The width.
        /// </summary>
        public int Width
        {
            get
            {
                return scrollView.Width;
            }
        }

        /// <summary>
        /// The height.
        /// </summary>
        public int Height
        {
            get
            {
                return scrollView.Height;
            }
        }

        /// <summary>
        /// An enumerator over the items in the grid.
        /// </summary>
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
        /// The current ItemComparer to use for the items in the grid. Can be null.
        /// You must layout the grid again for changes here to apply.
        /// </summary>
        public IComparer<ButtonGridItem> ItemComprarer
        {
            get
            {
                return itemComparer;
            }
            set
            {
                itemComparer = value;
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

        public ButtonGridCaptionFactory CaptionFactory
        {
            get
            {
                return captionFactory;
            }
            set
            {
                if(this.captionFactory != null)
                {
                    this.captionFactory.setButtonGrid(null);
                }
                this.captionFactory = value;
                captionFactory.setButtonGrid(this);
            }
        }

        internal void itemActivated(ButtonGridItem item)
        {
            if (ItemActivated != null)
            {
                ItemActivated.Invoke(item);
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

        private static ButtonGridCaptionFactory CreateDefaultCaptionFactory(ScrollView scrollView)
        {
            switch(scrollView.getUserString("CaptionType"))
            {
                case "Horizontal":
                    return new HorizontalButtonGridCaptionFactory();
                case "TopSeparator":
                    return new TopSeparatorButtonGridCaptionFactory();
                case "BottomSeparator":
                    return new BottomSeparatorButtonGridCaptionFactory();
                case "TextOnly":
                    return new TextOnlyButtonGridCaptionFactory();
                default:
                    return new HorizontalButtonGridCaptionFactory();

            }
        }

        void scrollView_CanvasPositionChanged(Widget source, EventArgs e)
        {
            allowClickEvents = false;
        }

        /// <summary>
        /// This value is tracked to allow click events or not, it will
        /// be set to false whenever the scroll view scrolls, this way if
        /// the user is scrolling a view the item that is clicked after the scroll
        /// completes will not be activated. This prevents items activating
        /// if this button grid is scrolled with a finger.
        /// </summary>
        internal bool AllowClickEvents
        {
            get
            {
                return allowClickEvents;
            }
            set
            {
                allowClickEvents = value;
            }
        }
    }
}

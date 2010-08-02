using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Logging;

namespace MyGUIPlugin
{
    public class ButtonGridItem : IDisposable
    {
        private Button button;
        private ButtonGrid grid;
        public event EventHandler ItemClicked;

        internal ButtonGridItem(ButtonGridGroup group, ButtonGrid list)
        {
            this.grid = list;
            this.Group = group;
            button = list.ScrollView.createWidgetT("Button", list.ButtonSkin, 0, 0, list.ItemWidth, list.ItemHeight, Align.Top | Align.Left, "") as Button;
            button.MouseButtonClick += new MyGUIEvent(button_MouseButtonClick);
            button.MouseButtonDoubleClick += new MyGUIEvent(button_MouseButtonDoubleClick);
        }

        void button_MouseButtonClick(Widget source, EventArgs e)
        {
            grid.SelectedItem = this;
            if (ItemClicked != null)
            {
                ItemClicked.Invoke(this, EventArgs.Empty);
            }
        }

        void button_MouseButtonDoubleClick(Widget source, EventArgs e)
        {
            grid.itemActivated(this);
        }

        public void Dispose()
        {
            Gui.Instance.destroyWidget(button);
        }

        public String Caption
        {
            get
            {
                return button.Caption;
            }
            set
            {
                button.Caption = value;
            }
        }

        public Object UserObject { get; set; }

        public ButtonGrid ButtonGrid
        {
            get
            {
                return Group.Grid;
            }
        }

        internal int Width
        {
            get
            {
                return button.getWidth();
            }
        }

        internal void setPosition(Vector2 position)
        {
            button.setPosition((int)position.x, (int)position.y);
        }

        internal ButtonGridGroup Group { get; private set; }

        internal bool StateCheck
        {
            get
            {
                return button.StateCheck;
            }
            set
            {
                button.StateCheck = value;
            }
        }

        public void setImage(String imageResource)
        {
            StaticImage image = button.StaticImage;
            if (image != null)
            {
                image.setItemResource(imageResource);
            }
        }
    }

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
                captionText.setSize((int)FontManager.Instance.measureStringWidth(captionText.Font, name) + 30, (int)captionText.FontHeight);

                separator = grid.ScrollView.createWidgetT("Widget", grid.GroupSeparatorSkin, 0, 0, 10, 1, Align.Left | Align.Top, "");
                separator.setSize((int)(grid.ScrollView.CanvasSize.Width - captionText.getWidth()) - 10, 1);
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
        public Vector2 layout(Vector2 startPosition)
        {
            Vector2 currentPosition = startPosition;

            if (grid.ShowGroupCaptions)
            {
                currentPosition.y += 5;
                captionText.setPosition((int)currentPosition.x, (int)currentPosition.y);
                separator.setPosition((int)(currentPosition.x + captionText.getWidth()), (int)(currentPosition.y + captionText.getHeight() / 2));
                currentPosition.y += captionText.getHeight() + 5;
            }

            foreach (ButtonGridItem item in items)
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
    }

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
        private ButtonGridItem selectedItem;
        private List<ButtonGridGroup> groups = new List<ButtonGridGroup>();
        private int itemCount = 0;

        public event EventHandler SelectedValueChanged;
        public event EventHandler ItemActivated;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrollView"></param>
        public ButtonGrid(ScrollView scrollView)
        {
            String read;
            bool boolValue;
            int intValue;
            SuppressLayout = false;

            this.scrollView = scrollView;

            //Try to get properties from the widget itself.
            read = scrollView.getUserString("ItemHeight");
            if (read != null && int.TryParse(read, out intValue))
            {
                ItemHeight = intValue;
            }
            else
            {
                ItemHeight = 122;
            }

            read = scrollView.getUserString("ItemWidth");
            if (read != null && int.TryParse(read, out intValue))
            {
                ItemWidth = intValue;
            }
            else
            {
                ItemWidth = 122;
            }

            read = scrollView.getUserString("ItemPaddingX");
            if (read != null && int.TryParse(read, out intValue))
            {
                ItemPaddingX = intValue;
            }
            else
            {
                ItemPaddingX = 10;
            }

            read = scrollView.getUserString("ItemPaddingY");
            if (read != null && int.TryParse(read, out intValue))
            {
                ItemPaddingY = intValue;
            }
            else
            {
                ItemPaddingY = 2;
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
                GroupCaptionSkin = "StaticText";
            }

            GroupSeparatorSkin = scrollView.getUserString("GroupSeparatorSkin");
            if (GroupSeparatorSkin == null || GroupSeparatorSkin == String.Empty)
            {
                GroupSeparatorSkin = "Separator1";
            }
        }

        /// <summary>
        /// Dispose function
        /// </summary>
        public void Dispose()
        {
            clear();
        }

        /// <summary>
        /// Add an item.
        /// </summary>
        /// <param name="group">The group to add the item to.</param>
        /// <param name="caption">The caption for the item.</param>
        /// <returns></returns>
        public ButtonGridItem addItem(String group, String caption)
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
            ButtonGridItem item = addGroup.addItem(caption);
            itemCount++;
            layout();
            return item;
        }

        /// <summary>
        /// Add an item.
        /// </summary>
        /// <param name="group">The group to add the item to.</param>
        /// <param name="caption">The caption for the item.</param>
        /// <param name="imageResource">The ImageResource name for the item's image.</param>
        /// <returns>The newly created ButtonGridItem.</returns>
        public ButtonGridItem addItem(String group, String caption, String imageResource)
        {
            ButtonGridItem item = addItem(group, caption);
            item.setImage(imageResource);
            return item;
        }

        /// <summary>
        /// Remove an item.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public void removeItem(ButtonGridItem item)
        {
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
                group.clear();
            }
            selectedItem = null;
            groups.Clear();
            layout();
        }

        public void layout()
        {
            if (!SuppressLayout)
            {
                Vector2 currentPosition = new Vector2(0.0f, 0.0f);
                foreach (ButtonGridGroup group in groups)
                {
                    currentPosition = group.layout(currentPosition);
                }
                scrollView.CanvasSize = new Size2(scrollView.CanvasSize.Width, currentPosition.y);
            }
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

        /// <summary>
        /// The skin to use for buttons in the grid.
        /// </summary>
        public String ButtonSkin { get; set; }

        /// <summary>
        /// The width of each button in the grid.
        /// </summary>
        public int ItemWidth { get; set; }

        /// <summary>
        /// The height of each button in the grid.
        /// </summary>
        public int ItemHeight { get; set; }

        /// <summary>
        /// The padding in the x dimesion between items.
        /// </summary>
        public int ItemPaddingX { get; set; }

        /// <summary>
        /// The padding in the y dimension between items.
        /// </summary>
        public int ItemPaddingY { get; set; }

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
        /// The currently selected item.
        /// </summary>
        public ButtonGridItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (selectedItem != value)
                {
                    if (selectedItem != null)
                    {
                        selectedItem.StateCheck = false;
                    }
                    selectedItem = value;
                    if (selectedItem != null)
                    {
                        selectedItem.StateCheck = true;
                    }
                    if (SelectedValueChanged != null)
                    {
                        SelectedValueChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

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
        /// A UserObject. Not used directly by this class.
        /// </summary>
        public Object UserObject { get; set; }

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

        internal void itemActivated(ButtonGridItem item)
        {
            if (ItemActivated != null)
            {
                ItemActivated.Invoke(item, EventArgs.Empty);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class ButtonListItem : IDisposable
    {
        private Button button;
        private ButtonList list;
        private int index;

        internal ButtonListItem(ButtonList list)
        {
            this.list = list;
            button = list.ScrollView.createWidgetT("Button", list.ButtonSkin, 0, 0, list.ItemWidth, list.ItemHeight, Align.Top | Align.Left, "") as Button;
            button.MouseButtonClick += new MyGUIEvent(button_MouseButtonClick);
        }

        void button_MouseButtonClick(Widget source, EventArgs e)
        {
            list.SelectedItem = this;
        }

        public void Dispose()
        {
            Gui.Instance.destroyWidget(button);
        }

        public int Index
        {
            get
            {
                return index;
            }
            internal set
            {
                index = value;
                button.setPosition(0, list.computeYPosForIndex(index));
            }
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

        public void setImage(String imageResource)
        {
            StaticImage image = button.StaticImage;
            if (image != null)
            {
                image.setItemResource(imageResource);
            }
        }
    }

    /// <summary>
    /// This class will make a list by putting a bunch of buttons on a specified
    /// scroll view. This allows for more control over how the list is rendered
    /// than MyGUI gives us.
    /// </summary>
    /// <remarks>
    /// This class can read the following user variables off of the ScrollView:
    /// 
    /// ItemHeight - The height of one item (default 32)
    /// ItemPadding - The padding between items (default 0)
    /// ButtonSkin - The skin of the button to use (default "Button")
    /// </remarks>
    public class ButtonList : IDisposable
    {
        private ScrollView scrollView;
        private List<ButtonListItem> items = new List<ButtonListItem>();
        private ButtonListItem selectedItem;

        public event EventHandler SelectedValueChanged;

        public ButtonList(ScrollView scrollView)
        {
            this.scrollView = scrollView;

            //Try to get properties from the widget itself.
            String read = scrollView.getUserString("ItemHeight");
            int value;
            if(read != null && int.TryParse(read, out value))
            {
                ItemHeight = value;
            }
            else
            {
                ItemHeight = 32;
            }

            read = scrollView.getUserString("ItemPadding");
            if (read != null && int.TryParse(read, out value))
            {
                ItemPadding = value;
            }
            else
            {
                ItemPadding = 0;
            }
            
            ButtonSkin = scrollView.getUserString("ButtonSkin");
            if (ButtonSkin == null || ButtonSkin == String.Empty)
            {
                ButtonSkin = "Button";
            }
        }

        public void Dispose()
        {
            clear();
        }

        public ButtonListItem addItem(String caption)
        {
            ButtonListItem item = new ButtonListItem(this);
            item.Caption = caption;
            item.Index = items.Count;
            items.Add(item);
            scrollView.CanvasSize = new Size(ItemWidth, computeYPosForIndex(items.Count));
            return item;
        }

        public ButtonListItem addItem(String caption, String imageResource)
        {
            ButtonListItem item = addItem(caption);
            item.setImage(imageResource);
            return item;
        }

        public void removeItem(ButtonListItem item)
        {
            int removeIndex = item.Index;
            items.RemoveAt(removeIndex);
            for (int i = removeIndex; i < items.Count; ++i)
            {
                items[i].Index--;
            }
            item.Dispose();
        }

        public void clear()
        {
            foreach (ButtonListItem item in items)
            {
                item.Dispose();
            }
            selectedItem = null;
            items.Clear();
        }

        public String ButtonSkin { get; set; }

        public int ItemHeight { get; set; }

        public int ItemPadding { get; set; }

        public int ItemWidth
        {
            get
            {
                return (int)scrollView.CanvasSize.Width;
            }
        }

        public int ItemCount
        {
            get
            {
                return items.Count;
            }
        }

        public int SelectedIndex
        {
            get
            {
                if (selectedItem != null)
                {
                    return selectedItem.Index;
                }
                return -1;
            }
            set
            {
                SelectedItem = items[value];
            }
        }

        public ButtonListItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    if (SelectedValueChanged != null)
                    {
                        SelectedValueChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        internal ScrollView ScrollView
        {
            get
            {
                return scrollView;
            }
        }

        internal int computeYPosForIndex(int index)
        {
            return index * (ItemHeight + ItemPadding);
        }
    }
}

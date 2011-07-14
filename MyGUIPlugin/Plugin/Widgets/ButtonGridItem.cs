using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

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
            button.ForwardMouseWheelToParent = true;
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

        public int Width
        {
            get
            {
                return button.Width;
            }
        }

        public int Height
        {
            get
            {
                return button.Height;
            }
        }

        public int AbsoluteLeft
        {
            get
            {
                return button.AbsoluteLeft;
            }
        }

        public int AbsoluteTop
        {
            get
            {
                return button.AbsoluteTop;
            }
        }

        public int Left
        {
            get
            {
                return button.Left;
            }
        }

        public int Top
        {
            get
            {
                return button.Top;
            }
        }

        public Size2 TextSize
        {
            get
            {
                return button.getTextSize();
            }
        }

        internal void setPosition(Vector2 position, int width, int height)
        {
            button.setCoord((int)position.x, (int)position.y, width, height);
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
}

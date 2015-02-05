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
        public event EventDelegate<ButtonGridItem, MouseEventArgs> MouseButtonPressed;
        public event EventDelegate<ButtonGridItem, MouseEventArgs> MouseButtonReleased;
        public event EventDelegate<ButtonGridItem, MouseEventArgs> MouseDrag;

        internal ButtonGridItem(ButtonGridGroup group, ButtonGrid list)
        {
            this.grid = list;
            this.Group = group;
            button = list.ScrollView.createWidgetT("Button", list.ButtonSkin, 0, 0, list.ItemWidth, list.ItemHeight, Align.Top | Align.Left, "") as Button;
            button.ForwardMouseWheelToParent = true;
            button.MouseButtonClick += new MyGUIEvent(button_MouseButtonClick);
            button.MouseButtonDoubleClick += new MyGUIEvent(button_MouseButtonDoubleClick);
            button.MouseButtonPressed += new MyGUIEvent(button_MouseButtonPressed);
            button.MouseButtonReleased += new MyGUIEvent(button_MouseButtonReleased);
            button.MouseDrag += new MyGUIEvent(button_MouseDrag);
        }

        public void Dispose()
        {
            Gui.Instance.destroyWidget(button);
        }

        public void setImage(String imageResource)
        {
            ImageBox image = button.ImageBox;
            if (image != null)
            {
                image.setItemResource(imageResource);
            }
        }

        public void setImageSize(int width, int height, int fullWidth, int fullHeight)
        {
            ImageBox image = button.ImageBox;
            if (image != null)
            {
                image.setCoord((fullWidth - width) / 2, (fullHeight - height) / 2, width, height);
                image.setImageCoord(new IntCoord(0, 0, width, height));
            }
        }

        public Widget createWidgetT(string type, string skin, int left, int top, int width, int height, Align align, string name)
        {
            return button.createWidgetT(type, skin, left, top, width, height, align, name);
        }

        public IntSize2 getImageSize()
        {
            ImageBox image = button.ImageBox;
            if (image != null)
            {
                return new IntSize2(image.Width, image.Height);
            }
            return new IntSize2(100, 100);
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

        public String GroupName
        {
            get
            {
                return Group.Name;
            }
        }

        public ImageBox ImageBox
        {
            get
            {
                return button.ImageBox;
            }
        }

        public IntCoord Coord
        {
            get
            {
                return new IntCoord(button.Left, button.Top, button.Width, button.Height);
            }
        }

        internal void setPosition(IntVector2 position, int width, int height)
        {
            button.setCoord(position.x, position.y, width, height);
        }

        internal ButtonGridGroup Group { get; private set; }

        public bool StateCheck
        {
            get
            {
                return button.Selected;
            }
            internal set
            {
                button.Selected = value;
            }
        }

        void button_MouseButtonClick(Widget source, EventArgs e)
        {
            if (grid.AllowClickEvents)
            {
                grid.itemChosen(this);
                if (ItemClicked != null)
                {
                    ItemClicked.Invoke(this, EventArgs.Empty);
                }
            }
        }

        void button_MouseButtonDoubleClick(Widget source, EventArgs e)
        {
            if (grid.AllowClickEvents)
            {
                grid.itemActivated(this);
            }
        }

        void button_MouseDrag(Widget source, EventArgs e)
        {
            if (MouseDrag != null)
            {
                MouseDrag.Invoke(this, (MouseEventArgs)e);
            }
        }

        void button_MouseButtonReleased(Widget source, EventArgs e)
        {
            if (MouseButtonReleased != null)
            {
                MouseButtonReleased.Invoke(this, (MouseEventArgs)e);
            }
        }

        void button_MouseButtonPressed(Widget source, EventArgs e)
        {
			grid.resetClickScrollTracker();
            if (MouseButtonPressed != null)
            {
                MouseButtonPressed.Invoke(this, (MouseEventArgs)e);
            }
        }
    }
}

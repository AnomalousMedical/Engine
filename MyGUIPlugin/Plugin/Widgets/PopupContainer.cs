using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class assists in using any widget as a popup that will
    /// automatically close when it is no longer clicked.
    /// </summary>
    public class PopupContainer
    {
        private Widget widget;

        public PopupContainer(Widget widget)
        {
            this.widget = widget;
        }

        public void show(int left, int top)
        {
            LayerManager.Instance.upLayerItem(widget);
            widget.setPosition(left, top);
            Visible = true;
        }

        public void hide()
        {
            Visible = false;
        }

        public bool Visible
        {
            get
            {
                return widget.Visible;
            }
            private set
            {
                widget.Visible = value;
                if (value)
                {
                    Gui.Instance.MouseButtonPressed += MouseButtonPressed;
                }
                else
                {
                    Gui.Instance.MouseButtonPressed -= MouseButtonPressed;
                }
            }
        }

        void MouseButtonPressed(int x, int y, MouseButtonCode button)
        {
            int left = widget.getAbsoluteLeft();
            int top = widget.getAbsoluteTop();
            int right = left + widget.getWidth();
            int bottom = top + widget.getHeight();
            if (x < left || x > right || y < top || y > bottom)
            {
                Visible = false;
            }
        }

        public Widget Widget
        {
            get
            {
                return widget;
            }
        }

        public Object UserObject { get; set; }
    }
}

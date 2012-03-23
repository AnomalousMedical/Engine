using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class takes a PopupMenu and a Button and will show the PopupMenu
    /// when the button is clicked. It also will keep the menu on the screen as
    /// best as it can.
    /// </summary>
    public class ShowMenuButton
    {
        private Button button;
        private PopupMenu menu;

        public ShowMenuButton(Button button, PopupMenu menu)
        {
            this.button = button;
            this.menu = menu;

            button.MouseButtonClick += new MyGUIEvent(button_MouseButtonClick);
        }

        public bool Enabled
        {
            get
            {
                return button.Enabled;
            }
            set
            {
                button.Enabled = value;
            }
        }

        public Button Button
        {
            get
            {
                return button;
            }
        }

        void button_MouseButtonClick(Widget source, EventArgs e)
        {
            int left = source.AbsoluteLeft;
            int top = source.AbsoluteTop + source.Height;
            int guiWidth = RenderManager.Instance.ViewWidth;
            int guiHeight = RenderManager.Instance.ViewHeight;

            int right = left + menu.Width;
            int bottom = top + menu.Height;

            if (right > guiWidth)
            {
                left -= right - guiWidth;
                if (left < 0)
                {
                    left = 0;
                }
            }

            if (bottom > guiHeight)
            {
                top -= bottom - guiHeight;
                if (top < 0)
                {
                    top = 0;
                }
            }

            menu.setPosition(left, top);            
            LayerManager.Instance.upLayerItem(menu);
            menu.setVisibleSmooth(true);
        }
    }
}
